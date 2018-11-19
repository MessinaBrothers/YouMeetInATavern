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

    public void UpdateConverseGUI(string dialogueID) {
        // get the NPC
        NPC npc = data.selectedCard.GetComponent<NPC>();

        // get the Dialogue
        Dialogue dialogue = data.key_dialoguesNEW[dialogueID];

        // set the text
        string formattedText = dialogue.text;
        //if (dialogue.clickableDialogueKeys.Count == 0) {
        //    formattedText = string.Format("<{0}>{1}", data.npcKey_defaultDialogueKey[npc.key], formattedText);
        //} else {
        //}
        foreach (string clickableKey in dialogue.clickableDialogueKeys) {
            string clickableText = data.key_dialoguesNEW[clickableKey].text;
            int clickableIndex = formattedText.IndexOf(clickableText);
            string toInsert = string.Format("<{0}>", clickableKey);
            formattedText = dialogue.text.Insert(clickableIndex, toInsert);
        }
        if (formattedText.StartsWith("<") == false) {
            formattedText = string.Format("<{0}>{1}", data.npcKey_defaultDialogueKey[npc.key], formattedText);
        }
        print(formattedText);
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(formattedText);
        if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
            Debug.LogFormat("Showing dialogue: NPC:{0}, dialogueID:{1}, dialogue:{2}", npc.key, dialogueID, formattedText);
        }

        // load or hide questions and Continue/Goodbye button
        PlayerResponseGUIController questionGUIController = GetComponent<PlayerResponseGUIController>();
        questionGUIController.HideAllButtons();

        switch (dialogue.type) {
            case Dialogue.TYPE.INTRO:
                questionGUIController.LoadQuestions(dialogue);
                break;
            case Dialogue.TYPE.START:
                questionGUIController.ShowGoodbyeButton();
                break;
            default:
                
                questionGUIController.ShowGoodbyeButton();
                break;
        }

        if (dialogue.nextDialogueKey == "" && dialogue.playerResponseKeys.Count == 0) {
            questionGUIController.ShowContinueButton();
        }

        //if (npc.isBeingIntroduced == true) {//
        //// if (data.gameMode == GameData.GameMode.INTRODUCE) {
        //    questionGUIController.HideQuestions();
        //    questionGUIController.ShowContinueButton();
        //} else {
        //    bool isQuestionsExist = questionGUIController.LoadQuestions(data.selectedCard);
        //    if (isQuestionsExist == true || data.isGoodbyeEnabled == false) {
        //        questionGUIController.HideGoodbyeButton();
        //    } else {
        //        questionGUIController.ShowGoodbyeButton();
        //    }
        //}

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
