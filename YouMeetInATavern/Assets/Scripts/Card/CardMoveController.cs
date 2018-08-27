using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMoveController : MonoBehaviour {

    public static event NPCInConversePosEventHandler npcInConversePosEventHandler;
    public delegate void NPCInConversePosEventHandler(GameObject card);

    public Transform offscreenPos, introPos, conversePos, enterTavernPos, exitPos;
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
        InputController.stopConverseEventHandler += Stop;
        InputController.npcLeavesEventHandler += Goodbye;
        NPCController.npcStartInTaverneventHandler += PlaceInTavern;
        NPCController.npcRandomlyLeavesEventHandler += LeaveTavern;
        InputController.endDayEarlyEventHandler += LeaveTavernAll;
        InputController.dialogueCardCreatedEventHandler += PreviewCard;
    }

    void OnDisable() {
        NPCController.npcIntroStartEventHandler -= Introduce;
        NPCController.npcIntroEndEventHandler -= Converse;
        ConverseNPC.npcStartConverseEventHandler -= Converse;
        InputController.stopConverseEventHandler -= Stop;
        InputController.npcLeavesEventHandler -= Goodbye;
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
        move.Set(offscreenPos, introPos, 1, Wait);
    }

    private void Converse(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;

        move.Set(card.transform, conversePos, 1, StartDialogue);
        
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


        move.Set(previewStartPos, previewEndPos, 0.75f, PreviewWait);
    }

    private void PreviewWait(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(previewEndPos, previewEndPos, .5f, EnterDeck);
    }

    private void EnterDeck(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(previewEndPos, deckPos, 0.5f, EnteredDeck);
    }

    private void EnteredDeck(GameObject card) {
        InputController.CardEnteredDeck(card);
    }

    private void Stop(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(conversePos, enterTavernPos, 1, Wander);
    }

    private void LeaveTavernAll() {
        foreach (GameObject npc in data.npcsInTavern) {
            LeaveTavern(npc);
        }
    }

    private void Goodbye() {
        CardMove move = data.selectedCard.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(conversePos, exitPos, 1, ExitTavern);
    }

    private void LeaveTavern(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(card.transform, exitPos, 1, ExitTavern);

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
