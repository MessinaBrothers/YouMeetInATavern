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

        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = false;
        npc.nextDialogueID = 1;
    }

    private void Goodbye() {
        data.gameMode = GameData.GameMode.TAVERN;

        GameObject card = data.selectedCard;
        card.GetComponent<CardSFX>().PlayGoodbye();

        // decrement npc count
        data.tavernNPCCount -= 1;
        if (data.tavernNPCCount == 0) {
            print("End DAY");
        }
    }
}
