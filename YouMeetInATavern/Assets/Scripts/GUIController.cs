using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject dialoguePanel, dayPanel, nightPanel;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        CardMoveController.npcInConversePosEventHandler += Converse;
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
    }

    public void UpdateConverseGUI(uint dialogueID) {
        // update dialogue
        NPC npc = data.selectedCard.GetComponent<NPC>();
        UpdateDialogue(npc.npcID, dialogueID);
        // update questions
        UpdateQuestions();
        // update Stop Converse button
        UpdateStopConverseButton(npc.isBeingIntroduced);
        // activate the panel
        dialoguePanel.SetActive(true);
    }

    public void StopConverse() {
        dialoguePanel.SetActive(false);
    }

    public void StartDay() {
        dialoguePanel.SetActive(false);
        dayPanel.SetActive(true);
        nightPanel.SetActive(false);
    }

    public void ContinueDay() {
        dayPanel.SetActive(false);
    }

    public void EndDay() {
        nightPanel.SetActive(true);
    }

    private void Converse(GameObject card) {
        UpdateConverseGUI(card.GetComponent<NPC>().nextDialogueID);
    }

    private void HandleQuestion(uint key) {
        // set the panel text
        uint npcID = data.selectedCard.GetComponent<NPC>().npcID;
        UpdateDialogue(npcID, key);
    }

    private void UpdateDialogue(uint npcID, uint dialogueID) {
        string text = data.npc_dialogues[npcID][dialogueID];
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(text);
    }

    private void UpdateQuestions() {
        dialoguePanel.GetComponentInChildren<QuestionGUIController>().LoadQuestions(data.selectedCard);
    }

    private void UpdateStopConverseButton(bool isIntro) {
        dialoguePanel.GetComponentInChildren<QuestionGUIController>().SetMode(isIntro);
    }
}
