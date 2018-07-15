using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject dialoguePanel;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        dialoguePanel.SetActive(false);
    }

    void Update() {

    }

    void OnEnable() {
        CardMoveController.npcInConversePosEventHandler += Converse;
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
    }

    public void UpdateGUI(uint dialogueID) {
        // TODO update display and questions
        print("Update GUI text and buttons of key " + dialogueID);
    }

    public void StopConverse() {
        dialoguePanel.SetActive(false);
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
