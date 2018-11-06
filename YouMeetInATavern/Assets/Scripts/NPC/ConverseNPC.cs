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
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.npcIntroEndEventHandler += HandleIntroduction;
        InputController.dialogueEventHandler += HandleDialogue;
        InputController.stopConverseEventHandler += Stop;
        InputController.npcLeavesEventHandler += Goodbye;
    }

    void OnDisable() {
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.npcIntroEndEventHandler -= HandleIntroduction;
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.stopConverseEventHandler -= Stop;
        InputController.npcLeavesEventHandler -= Goodbye;
    }

    private void HandleIntroduction(GameObject card) {
        // go straight from introduction mode into converse mode
        Converse(card);
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.TAVERN) {
            // if in tavern, start any conversation with NPC
            npcStartConverseEventHandler.Invoke(card);
        }
    }

    private void HandleDialogue(string key) {

    }

    private void Converse(GameObject card) {
    }

    private void Stop(GameObject card) {
        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = false;

        if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
            Debug.LogFormat("Checking scenario-specific dialogue: NPC:{0}, scenario id:{1}", npc.key, data.scenario.id);
        }

        // set the next dialogue

        if (data.npc_dialogues[npc.key].ContainsKey(GameData.DIALOGUE_SCENARIO_PREFIX + data.scenario.id)) {
            // if the NPC has dialogue specific to THIS scenario, use it
            npc.nextDialogueID = GameData.DIALOGUE_SCENARIO_PREFIX + data.scenario.id;

            if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
                Debug.LogFormat("An NPC has scenario-specific dialogue: NPC:{0}, scenario id:{1}", npc.key, data.scenario.id);
            }

        } else {
            npc.nextDialogueID = GameData.DIALOGUE_DEFAULT;
        }
    }

    private void Goodbye() {
        GameObject card = data.selectedCard;
        card.GetComponent<CardSFX>().PlayGoodbye();
    }
}
