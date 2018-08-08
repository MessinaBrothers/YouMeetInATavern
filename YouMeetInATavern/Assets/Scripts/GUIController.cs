using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject dialoguePanel, deckPanel, dayPanel, nightPanel, concludeScenarioPanel, resultsPanel;

    private GameData data;

    void Awake() {
        DeactivateAll();
    }

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        print("Game initialized. Loading GUI...");

        data = FindObjectOfType<GameData>();

        deckPanel.GetComponent<ViewDeckController>().Load(data);
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
        string text = data.npc_dialogues[npc.key][dialogueID];
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(text);

        QuestionGUIController questionGUIController = dialoguePanel.GetComponentInChildren<QuestionGUIController>();
        if (dialogueID == GameData.DIALOGUE_DEFAULT) {
            // update questions
            questionGUIController.LoadQuestions(data.selectedCard);
        } else {
            // hide questions
            questionGUIController.HideQuestions();
        }
        // update Stop Converse button
        questionGUIController.SetMode(npc.isBeingIntroduced);
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

    public void DisplayDeck() {
        DeactivateAll();
        deckPanel.GetComponent<ViewDeckController>().Display();
        deckPanel.SetActive(true);
    }

    private void DeactivateAll() {
        dialoguePanel.SetActive(false);
        deckPanel.SetActive(false);
        dayPanel.SetActive(false);
        nightPanel.SetActive(false);
        concludeScenarioPanel.SetActive(false);
        resultsPanel.SetActive(false);
    }

    private void Converse(GameObject card) {
        UpdateConverseGUI(card.GetComponent<NPC>().nextDialogueID);
    }
}
