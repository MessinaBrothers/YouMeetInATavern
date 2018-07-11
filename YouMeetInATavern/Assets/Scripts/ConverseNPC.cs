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
        IntroduceNPC.npcIntroEndEventHandler += HandleIntroduction;
        DialogueButton.dialogueEventHandler += HandleDialogue;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        IntroduceNPC.npcIntroEndEventHandler -= HandleIntroduction;
        DialogueButton.dialogueEventHandler -= HandleDialogue;
    }

    private void HandleIntroduction(GameObject card) {
        // go straight from introduction mode into converse mode
        Converse(card);
    }

    private void HandleCardClick(GameObject card) {
        // start any conversation with NPC if in tavern mode
        if (data.gameMode == GameData.GameMode.TAVERN) {
            Converse(card);
            npcStartConverseEventHandler.Invoke(card);
            // if already conversing, play a sound
        } else if (data.gameMode == GameData.GameMode.CONVERSE) {
            card.GetComponent<CardSFX>().PlayGreeting();
        }
    }

    private void HandleDialogue(int key) {
        if (key == GameData.DIALOGUE_DEFAULT) {
            data.gameMode = GameData.GameMode.TAVERN;
        }
    }

    private void Converse(GameObject card) {
        print("conversing");
        data.gameMode = GameData.GameMode.CONVERSE;
        data.selectedCard = card;
    }
}
