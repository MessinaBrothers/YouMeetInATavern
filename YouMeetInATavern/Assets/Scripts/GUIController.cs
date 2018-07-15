using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject dialoguePanel;

    private GameData data;

    private static uint dialogueIDToDisplay;

    void Start() {
        data = FindObjectOfType<GameData>();

        dialoguePanel.SetActive(false);
    }

    void Update() {
        if (dialogueIDToDisplay != GameData.DIALOGUE_INVALID) {
            // TODO update display and questions
            print("Update GUI text and buttons of key " + dialogueIDToDisplay);
            dialogueIDToDisplay = GameData.DIALOGUE_INVALID;
        }
    }

    void OnEnable() {
        CardMoveController.npcInConversePosEventHandler += Converse;
        InputController.questionEventHandler += HandleQuestion;
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
        InputController.questionEventHandler -= HandleQuestion;
    }

    public static void UpdateGUI(uint dialogueID) {
        dialogueIDToDisplay = dialogueID;
    }

    private void Converse(GameObject card) {
        // set the next dialogue text
        NPC npc = card.GetComponent<NPC>();
        SetDialogueText(npc.npcID, npc.nextDialogueID);
        // set the question texts
        dialoguePanel.GetComponentInChildren<QuestionController>().LoadQuestions(card);
        // activate the panel
        dialoguePanel.SetActive(true);
    }

    private void HandleQuestion(uint key) {

        // set the panel text
        uint npcID = data.selectedCard.GetComponent<NPC>().npcID;
        SetDialogueText(npcID, key);
    }

    private void SetDialogueText(uint npcID, uint dialogueID) {
        string text = data.npc_dialogues[npcID][dialogueID];
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(text);
    }
}
