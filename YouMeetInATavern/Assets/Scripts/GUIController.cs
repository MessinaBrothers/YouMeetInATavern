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

    public void UpdateConverseGUI(string dialogueID) {
        // update dialogue
        NPC npc = data.selectedCard.GetComponent<NPC>();
        string text = data.npc_dialogues[npc.npcID][dialogueID];
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(text);
        // update questions
        dialoguePanel.GetComponentInChildren<QuestionGUIController>().LoadQuestions(data.selectedCard);
        // update Stop Converse button
        dialoguePanel.GetComponentInChildren<QuestionGUIController>().SetMode(npc.isBeingIntroduced);
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
}
