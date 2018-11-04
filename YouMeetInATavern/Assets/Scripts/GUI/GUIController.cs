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
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
        InputController.clockTickedEventHandler -= UpdateClock;
    }

    private void UpdateClock(int currentHour) {
        clockGUI.UpdateText(currentHour);
    }

    public void UpdateConverseGUI(Question question) {
        int endIDIndex = question.text.IndexOf('>');
        string unlockKey = question.text.Substring("<".Length, endIDIndex - "<".Length);
        UpdateConverseGUI(unlockKey);
    }

    public void UpdateConverseGUI(string dialogueID) {
        // update dialogue
        NPC npc = data.selectedCard.GetComponent<NPC>();
        string text = data.npc_dialogues[npc.key][dialogueID];
        if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
            Debug.LogFormat("Showing dialogue: NPC:{0}, dialogueID:{1}, dialogue:{2}", npc.key, dialogueID, text);
        }
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(text);

        QuestionGUIController questionGUIController = dialoguePanel.GetComponentInChildren<QuestionGUIController>();
        // load or hide questions and Continue/Goodbye button
        if (npc.isBeingIntroduced == true) {//
        // if (data.gameMode == GameData.GameMode.INTRODUCE) {
            questionGUIController.HideQuestions();
            questionGUIController.ShowContinueButton();
        } else {
            bool isQuestionsExist = questionGUIController.LoadQuestions(data.selectedCard);
            if (isQuestionsExist == true || data.isGoodbyeEnabled == false) {
                questionGUIController.HideGoodbyeButton();
            } else {
                questionGUIController.ShowGoodbyeButton();
            }
        }
        // activate the panel
        dialoguePanel.SetActive(true);
    }

    public void StartScenario(GameData data) {
        hudPanel.GetComponentInChildren<DeckGUI>().ResetDeck(data);
    }

    public void StopConverse() {
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

    public void EndDay() {
        //DeactivateAll();
        //nightPanel.SetActive(true);
        fader.FadeOut(data.fadeOutTime);
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
