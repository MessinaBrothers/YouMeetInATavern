using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject hudPanel, dialoguePanel, deckPanel, nightPanel, concludeScenarioPanel, resultsPanel;
    public ClockGUI clockGUI;

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
        clockGUI = GetComponentInChildren<ClockGUI>();
        DeactivateAll();
    }

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        print("Game initialized. Loading GUI...");

        deckPanel.GetComponent<ViewDeckController>().Load(data);
        hudPanel.GetComponentInChildren<DeckGUI>().Load(data);
    }

    void Update() {

    }

    void OnEnable() {
        CardMoveController.npcInConversePosEventHandler += Converse;
        InputController.clockTickedEventHandler += UpdateClock;
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
        InputController.clockTickedEventHandler -= UpdateClock;
    }

    private void UpdateClock(int currentHour) {
        clockGUI.UpdateText(currentHour);
    }

    public void UpdateConverseGUI(Dialogue question) {
        UpdateConverseGUI(question.nextDialogueKey);
    }

    public void UpdateConverseGUI(string dialogueKey) {
        // get the NPC
        NPC npc = data.selectedCard.GetComponent<NPC>();

        // get the Dialogue
        Dialogue dialogue = data.key_dialoguesNEW[dialogueKey];

        // set the text
        string formattedText = dialogue.text;
        string defaultDialogueKey = data.npcKey_defaultDialogueKey[npc.key];
        string toInsertAfter = string.Format("<{0}>", defaultDialogueKey);
        // for each clickable dialogue
        foreach (string clickableKey in dialogue.clickableDialogueKeys) {
            Dialogue clickableDialogue = data.key_dialoguesNEW[clickableKey];

            // get indices
            int clickableStartIndex = formattedText.IndexOf(clickableDialogue.text);
            int clickableEndIndex = clickableStartIndex + clickableDialogue.text.Length;

            // insert default key after clickable key
            formattedText = formattedText.Insert(clickableEndIndex, toInsertAfter);

            // prepare the next dialogue key to go to after clicking on the keywords
            string nextKey = clickableDialogue.nextDialogueKey;
            string toInsert = string.Format("<{0}>", nextKey);

            // insert the keyword key
            formattedText = formattedText.Insert(clickableStartIndex, toInsert);
        }

        // make sure the text starts with a key
        if (formattedText[0] != '<') {
            formattedText = toInsertAfter + formattedText;
        }
        
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(formattedText);

        if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
            Debug.LogFormat("Showing dialogue: NPC:{0}, dialogueID:{1}, dialogue:{2}", npc.key, dialogueKey, formattedText);
        }

        // load or hide questions
        PlayerResponseGUIController playerResponseGUIController = GetComponent<PlayerResponseGUIController>();
        playerResponseGUIController.HideAllButtons();
        playerResponseGUIController.LoadQuestions(dialogue);

        // if ending a conversation, show goodbye
        if (dialogue.isEndOfConversation) {
            playerResponseGUIController.ShowGoodbyeButton();
        }
        // if there's more conversation, show continue button
        if (GraphUtility.IsType(data, dialogue.nextDialogueKey, Dialogue.TYPE.NPC_SAYS)) {
            playerResponseGUIController.ShowContinueButton(dialogue);
        // if it's default dialogue, say goodbye
        } else if (dialogueKey == defaultDialogueKey) {
            playerResponseGUIController.ShowGoodbyeButton();
        }

        // preserve player responses for default dialogue
        if (dialogueKey != defaultDialogueKey) {
            Dialogue defaultDialogue = data.key_dialoguesNEW[defaultDialogueKey];

            defaultDialogue.playerResponseKeys = dialogue.playerResponseKeys;
            defaultDialogue.isEndOfConversation = dialogue.isEndOfConversation;
        }

        // activate the panel
        dialoguePanel.SetActive(true);
    }

    public void StartScenario(GameData data, uint dayCount) {
        if (dayCount == 0) hudPanel.GetComponentInChildren<DeckGUI>().ResetDeck(data);
    }

    public void EndDialogue() {
        DeactivateAll();
        hudPanel.SetActive(true);
    }

    public void LoadTavern() {
        DeactivateAll();
        //dayPanel.SetActive(true);
        hudPanel.SetActive(true);
        hudPanel.GetComponentInChildren<DeckGUI>().ReloadDeck(data);
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
        deckPanel.GetComponent<ViewDeckController>().Display();
        deckPanel.SetActive(true);
    }

    public void CloseDeck() {
        deckPanel.GetComponent<ViewDeckController>().Close();
        deckPanel.SetActive(false);
    }

    private void DeactivateAll() {
        hudPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        deckPanel.SetActive(false);
        nightPanel.SetActive(false);
        concludeScenarioPanel.SetActive(false);
        resultsPanel.SetActive(false);
    }

    private void Converse(GameObject card) {
        UpdateConverseGUI(card.GetComponent<NPC>().nextDialogueID);
    }
}
