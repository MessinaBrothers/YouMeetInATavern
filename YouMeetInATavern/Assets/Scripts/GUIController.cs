﻿using System;
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

    private void Converse(GameObject card) {
        UpdateGUI(card.GetComponent<NPC>().nextDialogueID);
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
        dialoguePanel.GetComponentInChildren<EndConverseButton>().SetMode(isIntro);
    }
}
