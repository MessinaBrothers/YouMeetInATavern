using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveController : MonoBehaviour {

    public static event NPCInConversePosEventHandler npcInConversePosEventHandler;
    public delegate void NPCInConversePosEventHandler(GameObject card);

    public Transform offscreenPos, introPos, conversePos, enterTavernPos, exitPos;

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

    private void Wait(GameObject card) {
        
    }

    private void ExitTavern(GameObject card) {
        data.npcsInTavern.Remove(card);
        InputController.npcExitTavern(card);
    }

    private void StartDialogue(GameObject card) {
        npcInConversePosEventHandler.Invoke(card);
    }
}
