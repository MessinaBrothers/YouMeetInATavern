using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public GameObject hudPanel, dialoguePanel, deckPanel, nightPanel, concludeScenarioPanel, resultsPanel;
    public FadeImage fader, deckCamImage;
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
        InputController.gameflowStartFinishTavern += StartFinishTavern;
        InputController.gameflowStartFinishConclusion += FadeOut;
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
        InputController.clockTickedEventHandler -= UpdateClock;
        InputController.gameflowStartFinishTavern -= StartFinishTavern;
        InputController.gameflowStartFinishConclusion -= FadeOut;
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

        foreach (string cardID in dialogue.unlockCardKeys) {
            print("unlocking: " + cardID);
            InputController.CreateCard(cardID);
        }

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
            formattedText = dialogue.text.Insert(clickableEndIndex, toInsertAfter);

            // prepare the next dialogue key to go to after clicking on the keywords
            string nextKey = clickableDialogue.nextDialogueKey;
            string toInsert = string.Format("<{0}>", nextKey);

            // insert the keyword key
            formattedText = dialogue.text.Insert(clickableStartIndex, toInsert);
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

        // if starting or ending a conversation, show goodbye
        if (dialogue.isEndOfConversation) {
            playerResponseGUIController.ShowGoodbyeButton();
        // if no options, show continue
        } else if (dialogue.playerResponseKeys.Count == 0) {
            if (dialogueKey == defaultDialogueKey) {
                playerResponseGUIController.ShowContinueButton();
            } else {
                playerResponseGUIController.ShowContinueButton(dialogue);
            }
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

    public void FadeIn() {
        fader.FadeIn(data.fadeInTime);
        deckCamImage.FadeOut(data.fadeInTime);
    }

    public void FadeOut() {
        fader.FadeOut(data.fadeOutTime);
    }

    public void LoadTavern() {
        DeactivateAll();
        //dayPanel.SetActive(true);
        hudPanel.SetActive(true);
        hudPanel.GetComponentInChildren<DeckGUI>().ReloadDeck(data);
    }

    public void StartFinishTavern() {
        //DeactivateAll();
        //nightPanel.SetActive(true);
        FadeOut();
        deckCamImage.FadeIn(data.fadeOutTime);

    }

    public void ConcludeScenario() {
        DeactivateAll();
        concludeScenarioPanel.SetActive(true);
        fader.FadeIn(data.fadeInTime);
    }

    public void ConfirmScenario() {
        DeactivateAll();
        resultsPanel.SetActive(true);
        resultsPanel.GetComponentInChildren<UnityEngine.UI.Text>().text = data.resultsDialogue;
        fader.FadeIn(data.fadeOutTime);
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
