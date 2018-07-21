using System.Collections;
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
        NPCController.npcIntroEndEventHandler += HandleIntroduction;
        DialogueButton.dialogueEventHandler += HandleDialogue;
        InputController.stopConverseEventHandler += Stop;
        InputController.npcLeavesEventHandler += Goodbye;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        NPCController.npcIntroEndEventHandler -= HandleIntroduction;
        DialogueButton.dialogueEventHandler -= HandleDialogue;
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
            card.GetComponent<CardSFX>().PlayBeginConverse();
            npcStartConverseEventHandler.Invoke(card);
        } else if (data.gameMode == GameData.GameMode.CONVERSE) {
            // if already conversing, play a sound
            card.GetComponent<CardSFX>().PlayGreeting();
        }
    }

    private void HandleDialogue(string key) {
        
    }

    private void Converse(GameObject card) {
    }

    private void Stop() {
        GameObject card = data.selectedCard;

        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = false;
        npc.nextDialogueID = GameData.DIALOGUE_DEFAULT;
    }

    private void Goodbye() {
        GameObject card = data.selectedCard;
        card.GetComponent<CardSFX>().PlayGoodbye();
    }
}
