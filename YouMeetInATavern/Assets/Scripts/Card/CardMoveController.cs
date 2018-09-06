using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMoveController : MonoBehaviour {

    public static event NPCInConversePosEventHandler npcInConversePosEventHandler;
    public delegate void NPCInConversePosEventHandler(GameObject card);

    public Transform offscreenPos, introPos, conversePos, exitPos;
    public Transform enterBarPos, enterTavernPos, enterExitPos;
    public Transform previewStartPos, previewEndPos, deckPos;
    public Transform deckCardRevealPos, deckCardOffscreenPos;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        NPCController.npcIntroStartEventHandler += Introduce;
        NPCController.npcIntroEndEventHandler += Converse;
        ConverseNPC.npcStartConverseEventHandler += Converse;
        InputController.stopConverseEventHandler += EnterTavern;
        InputController.clockTickedEventHandler += Goodbye;
        NPCController.npcStartInTaverneventHandler += PlaceInTavern;
        NPCController.npcRandomlyLeavesEventHandler += LeaveTavern;
        InputController.endDayEarlyEventHandler += LeaveTavernAll;
        InputController.dialogueCardCreatedEventHandler += PreviewCard;
    }

    void OnDisable() {
        NPCController.npcIntroStartEventHandler -= Introduce;
        NPCController.npcIntroEndEventHandler -= Converse;
        ConverseNPC.npcStartConverseEventHandler -= Converse;
        InputController.stopConverseEventHandler -= EnterTavern;
        InputController.clockTickedEventHandler -= Goodbye;
        NPCController.npcStartInTaverneventHandler -= PlaceInTavern;
        NPCController.npcRandomlyLeavesEventHandler -= LeaveTavern;
        InputController.endDayEarlyEventHandler -= LeaveTavernAll;
        InputController.dialogueCardCreatedEventHandler -= PreviewCard;
    }

    // generic move function
    public static void Move(GameObject card, Transform from, Transform to, float time) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(from, to, time, Wait);
    }

    private void Introduce(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(offscreenPos, introPos, data.cardIntroductionSpeed, Wait);
    }

    private void Converse(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;

        move.Set(card.transform, conversePos, data.cardConverseSpeed, StartDialogue);
        
        // disable wandering
        card.GetComponent<CardWander>().enabled = false;
    }

    private void PreviewCard(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitInfo;
        //Physics.Raycast(ray, out hitInfo, 100f);//, LayerMask.NameToLayer("DeckPlane"));
        //previewStartPos.position = hitInfo.point;
        //print(hitInfo.point);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("DeckPlane")) {
                previewStartPos.position = hit.point;
            }
        }
        
        move.Set(previewStartPos, previewEndPos, data.cardPreviewSpeed, PreviewWait);
    }

    private void PreviewWait(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(previewEndPos, previewEndPos, data.cardPreviewWaitTime, EnterDeck);
    }

    private void EnterDeck(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(previewEndPos, deckPos, data.cardPreviewEnterDeckSpeed, EnteredDeck);
    }

    private void EnteredDeck(GameObject card) {
        InputController.CardEnteredDeck(card);
    }

    private void EnterTavern(GameObject card) {
        NPC npc = card.GetComponent<NPC>();

        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;

        // set where to enter the tavern and the NPC wander planes
        Transform enterPos;
        CardWander wander = card.GetComponent<CardWander>();
        wander.enabled = false;

        if (npc.key == "NPC_BARTENDER") {
            enterPos = enterBarPos;
            wander.quads = data.wanderAreasBartender;
        } else if (npc.hourLeavesTavern - 1 == data.currentHour) {
            enterPos = enterExitPos;
            wander.quads = data.wanderAreasExit;
        } else {
            enterPos = enterTavernPos;
            wander.quads = data.wanderAreasTavern;
        }

        move.Set(card.transform, enterPos, data.cardEnterTavernSpeed, Wander);
    }

    private void LeaveTavernAll() {
        foreach (GameObject npc in data.npcsInTavern) {
            LeaveTavern(npc);
        }
    }

    private void Goodbye(int currentHour) {
        foreach (GameObject card in data.npcsInTavern) {
            NPC npc = card.GetComponent<NPC>();
            if (npc.hourLeavesTavern == currentHour) {
                LeaveTavern(card);
            } else if (npc.hourLeavesTavern - 1 == currentHour) {
                // NPCs about to leave go to exit area
                EnterTavern(card);
            } else if (card == data.selectedCard) {
                // return selected card to tavern
                EnterTavern(card);
            }

        }
    }

    private void LeaveTavern(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(card.transform, exitPos, data.cardLeaveTavernSpeed, ExitTavern);

        // disable wandering
        card.GetComponent<CardWander>().enabled = false;
    }

    private void PlaceInTavern(GameObject card) {
        card.transform.position = enterTavernPos.position;
        card.transform.rotation = enterTavernPos.rotation;
        Wander(card);
    }

    private void Wander(GameObject card) {
        card.GetComponent<CardMove>().enabled = false;
        card.GetComponent<CardWander>().enabled = true;
    }

    private static void Wait(GameObject card) {
        
    }

    private void ExitTavern(GameObject card) {
        data.npcsInTavern.Remove(card);
        InputController.npcExitTavern(card);
    }

    private void StartDialogue(GameObject card) {
        npcInConversePosEventHandler.Invoke(card);
    }
}
