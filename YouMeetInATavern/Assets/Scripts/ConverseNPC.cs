﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverseNPC : MonoBehaviour {

    public static event NPCStartConverseEventHandler npcStartConverseEventHandler;
    public delegate void NPCStartConverseEventHandler(GameObject card);

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        CardClickable.cardClickedEventHandler += HandleCardClick;
        IntroduceNPC.npcIntroEndEventHandler += HandleIntroduction;
        DialogueButton.dialogueEventHandler += HandleDialogue;
        InputController.stopConverseEventHandler += Stop;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        IntroduceNPC.npcIntroEndEventHandler -= HandleIntroduction;
        DialogueButton.dialogueEventHandler -= HandleDialogue;
        InputController.stopConverseEventHandler -= Stop;
    }

    private void HandleIntroduction(GameObject card) {
        // go straight from introduction mode into converse mode
        Converse(card);
    }

    private void HandleCardClick(GameObject card) {
        // start any conversation with NPC if in tavern mode
        if (data.gameMode == GameData.GameMode.TAVERN) {
            Converse(card);
            card.GetComponent<CardSFX>().PlayBeginConverse();
            npcStartConverseEventHandler.Invoke(card);
        } else if (data.gameMode == GameData.GameMode.CONVERSE) {
            // if already conversing, play a sound
            card.GetComponent<CardSFX>().PlayGreeting();
        }
    }

    private void HandleDialogue(uint key) {
        
    }

    private void Converse(GameObject card) {
        data.gameMode = GameData.GameMode.CONVERSE;
        data.selectedCard = card;
    }

    private void Stop() {
        GameObject card = data.selectedCard;

        data.gameMode = GameData.GameMode.TAVERN;
        card.GetComponent<CardSFX>().PlayGoodbye();

        // after introduction, change its next dialogue
        if (card.GetComponent<NPC>().isBeingIntroduced == true) {
            NPC npc = card.GetComponent<NPC>();
            npc.isBeingIntroduced = false;
            npc.nextDialogueID = 1;
        }
    }
}
