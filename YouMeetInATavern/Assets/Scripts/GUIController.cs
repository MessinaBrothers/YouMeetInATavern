using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject dialoguePanel, dayPanel, nightPanel, concludeScenarioPanel, resultsPanel;

    private GameData data;

    void Awake() {
        dayPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        nightPanel.SetActive(false);
        concludeScenarioPanel.SetActive(false);
    }

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
        DeactivateAll();
    }

    public void StartDay() {
        DeactivateAll();
        dayPanel.SetActive(true);
    }

    public void ContinueDay() {
        DeactivateAll();
    }

    public void EndDay() {
        DeactivateAll();
        nightPanel.SetActive(true);
    }

    public void ConcludeScenario() {
        DeactivateAll();
        concludeScenarioPanel.SetActive(true);
    }

    public void ConfirmScenario() {
        DeactivateAll();
        resultsPanel.SetActive(true);
        resultsPanel.GetComponentInChildren<UnityEngine.UI.Text>().text = data.resultsDialogue;
    }

    private void DeactivateAll() {
        dayPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        nightPanel.SetActive(false);
        concludeScenarioPanel.SetActive(false);
        resultsPanel.SetActive(false);
    }

    private void Converse(GameObject card) {
        UpdateConverseGUI(card.GetComponent<NPC>().nextDialogueID);
    }
}
