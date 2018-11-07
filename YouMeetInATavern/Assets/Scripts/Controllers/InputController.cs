using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    
    // GAME FLOW
    public static event GameflowEndInitializeEventHandler gameflowEndInitialize;
    public delegate void GameflowEndInitializeEventHandler();
    
    public static event GameflowStartBeginTavernEventHandler gameflowStartBeginTavern;
    public delegate void GameflowStartBeginTavernEventHandler(GameData data, uint dayCount);
    
    public static event GameflowEndBeginTavernEventHandler gameflowEndBeginTavern;
    public delegate void GameflowEndBeginTavernEventHandler();
    
    public static event GameflowIntroduceNPCsEventHandler gameflowBeginIntroduceNPCs;
    public delegate void GameflowIntroduceNPCsEventHandler();

    public static event GameflowStartFinishTavernEventHandler gameflowStartFinishTavern;
    public delegate void GameflowStartFinishTavernEventHandler();
    
    public static event GameflowEndFinishTavernEventHandler gameflowEndFinishTavern;
    public delegate void GameflowEndFinishTavernEventHandler();

    public static event GameflowStartBeginConclusionEventHandler gameflowStartBeginConclusion;
    public delegate void GameflowStartBeginConclusionEventHandler();

    public static event GameflowStartFinishnConclusionEventHandler gameflowStartFinishConclusion;
    public delegate void GameflowStartFinishnConclusionEventHandler();

    public static event StartGameEventHandler gameflowStartGame;
    public delegate void StartGameEventHandler();

    public static event CheckAnswersEventHandler checkAnswersEventHandler;
    public delegate void CheckAnswersEventHandler();

    public static event ConfirmScenarioChoicesEventHandler confirmScenarioChoicesEventHandler;
    public delegate void ConfirmScenarioChoicesEventHandler();

    public static event EndResultsEventHandler endResultsEventHandler;
    public delegate void EndResultsEventHandler();
    
    public static event GameflowModeChangeEventHandler gameflowModeChange;
    public delegate void GameflowModeChangeEventHandler(GameData.GameMode mode);

    // CARD INTERACTIONS
    public static event CardClickedEventHandler cardClickedEventHandler;
    public delegate void CardClickedEventHandler(GameObject card);

    public static event DialogueCardCreatedEventHandler dialogueCardCreatedEventHandler;
    public delegate void DialogueCardCreatedEventHandler(GameObject card);

    public static event CardEnteredDeckEventHandler cardEnteredDeckEventHandler;
    public delegate void CardEnteredDeckEventHandler(GameObject card);

    // CARD HAND
    public static event HoverOverCardHandEventHandler hoverOverCardHandEventHandler;
    public delegate void HoverOverCardHandEventHandler(int id);

    public static event HoverExitCardHandEventHandler hoverExitCardHandEventHandler;
    public delegate void HoverExitCardHandEventHandler(int id);

    public static event CardHandClickedEventHandler cardHandClickedEventHandler;
    public delegate void CardHandClickedEventHandler(Vector3 position, int id);

    public static event HoverOverCardSelectedEventHandler hoverOverCardSelectedEventHandler;
    public delegate void HoverOverCardSelectedEventHandler(int id);

    public static event HoverExitCardSelectedEventHandler hoverExitCardSelectedEventHandler;
    public delegate void HoverExitCardSelectedEventHandler(int id);

    public static event CardSelectedClickedEventHandler cardSelectedClickedEventHandler;
    public delegate void CardSelectedClickedEventHandler(Vector3 position, int id);

    public static event UnusedDeckClickedEventHandler unusedDeckClickedEventHandler;
    public delegate void UnusedDeckClickedEventHandler(Vector3 position);

    public static event DiscardDeckClickedEventHandler discardDeckClickedEventHandler;
    public delegate void DiscardDeckClickedEventHandler(Vector3 position);

    // DECK INTERACTIONS
    public static event DeckHoverEventHandler deckHoverEventHandler;
    public delegate void DeckHoverEventHandler(bool isHover);

    public static event DeckClickedEventHandler deckClickedEventHander;
    public delegate void DeckClickedEventHandler();

    public static event DeckClosedEventHandler deckClosedEventHander;
    public delegate void DeckClosedEventHandler();

    public static event DeckCardSelectedEventHandler deckCardSelectedEventHandler;
    public delegate void DeckCardSelectedEventHandler(CardData cardData, int index);

    // DIALOGUE
    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(Question question);

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(string unlockKey);

    public static event DialogueSettingEventHandler dialogueSettingEventHandler;
    public delegate void DialogueSettingEventHandler(string arg);

    public static event StopConverseEventHandler stopConverseEventHandler;
    public delegate void StopConverseEventHandler(GameObject card);

    // NPCS

    public static event NPCIntroStartEventHandler npcIntroStartEventHandler;
    public delegate void NPCIntroStartEventHandler(GameObject card);

    public static event NPCIntroEndEventHandler npcIntroEndEventHandler;
    public delegate void NPCIntroEndEventHandler(GameObject card);

    public static event EndDayEarlyEventHandler endDayEarlyEventHandler;
    public delegate void EndDayEarlyEventHandler();

    public static event NPCLeavesEventHandler npcLeavesEventHandler;
    public delegate void NPCLeavesEventHandler();

    public static event NPCRemovedEventHandler npcLeftTavernEventHandler;
    public delegate void NPCRemovedEventHandler(GameObject card);

    // HEXES
    public static event HoverOverHexEventHandler hoverOverHexEventHandler;
    public delegate void HoverOverHexEventHandler(int id);

    public static event HoverExitHexEventHandler hoverExitHexEventHandler;
    public delegate void HoverExitHexEventHandler(int id);

    public static event HexClickedEventHandler hexClickedEventHandler;
    public delegate void HexClickedEventHandler(int id);

    // MISC
    public static event ChooseLocationEventHandler chooseLocationEventHandler;
    public delegate void ChooseLocationEventHandler(GameData.Location location);

    public static event ClockTickedEventHandler clockTickedEventHandler;
    public delegate void ClockTickedEventHandler(int currentHour);

    public static event TutorialScreenClickedEventHandler tutorialScreenClickedEventHandler;
    public delegate void TutorialScreenClickedEventHandler(GameObject currentScreen, GameObject nextScreen);

    public static event ClickConclusionBackgroundEventHandler conclusionBackgroundClicked;
    public delegate void ClickConclusionBackgroundEventHandler(Vector3 position);

    public static event ClickTavernEventHandler tavernClicked;
    public delegate void ClickTavernEventHandler(Vector3 position);

    private static GameData data;
    private static GUIController guiController; //TODO instead call updateGUIEventHandler for all GUIs to update themselves

    void Start() {
        guiController = FindObjectOfType<GUIController>();
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    // GAME FLOW

    public static void GameInitialized() {
        gameflowEndInitialize.Invoke();
    }

    public static void StartBeginTavern(uint dayCount) {
        gameflowStartBeginTavern.Invoke(data, dayCount);
        guiController.StartScenario(data, dayCount);
    }

    public static void EndBeginTavern() {
        gameflowEndBeginTavern.Invoke();
        guiController.FadeIn();
        guiController.LoadTavern();
    }

    public static void IntroduceNPCs() {
        gameflowBeginIntroduceNPCs.Invoke();
    }

    public static void ChangeMode(GameData.GameMode mode) {
        gameflowModeChange.Invoke(mode);
    }

    public static void StartFinishTavern() {
        gameflowStartFinishTavern.Invoke();
    }

    public static void EndFinishTavern() {
        gameflowEndFinishTavern.Invoke();
    }

    public static void ConcludeScenario() {
        gameflowStartBeginConclusion.Invoke();
        guiController.ConcludeScenario();
    }

    public static void FinishConclusion() {
        gameflowStartFinishConclusion.Invoke();
    }

    public static void CheckAnswers() {
        checkAnswersEventHandler.Invoke();
    }

    public static void ConfirmScenario() {
        confirmScenarioChoicesEventHandler.Invoke();
        guiController.ConfirmScenario();
    }

    public static void EndResults() {
        endResultsEventHandler.Invoke();
    }

    public static void StartGame() {
        gameflowStartGame.Invoke();
    }

    // CARD INTERACTIONS

    public static void HandleCardClick(GameObject card) {
        cardClickedEventHandler.Invoke(card);
    }

    public static void DialogueCardCreated(GameObject card) {
        dialogueCardCreatedEventHandler.Invoke(card);
    }

    public static void CardEnteredDeck(GameObject card) {
        if (cardEnteredDeckEventHandler != null) {
            cardEnteredDeckEventHandler.Invoke(card);
        }
    }

    // CARD HAND

    public static void HoverOverCardHand(int id) {
        hoverOverCardHandEventHandler.Invoke(id);
    }

    public static void HoverExitCardHand(int id) {
        hoverExitCardHandEventHandler.Invoke(id);
    }

    public static void ClickCardHand(Vector3 position, int id) {
        cardHandClickedEventHandler.Invoke(position, id);
    }

    public static void HoverOverCardSelected(int id) {
        hoverOverCardSelectedEventHandler.Invoke(id);
    }

    public static void HoverExitCardSelected(int id) {
        hoverExitCardSelectedEventHandler.Invoke(id);
    }

    public static void ClickCardSelected(Vector3 position, int id) {
        cardSelectedClickedEventHandler.Invoke(position, id);
    }

    public static void UnusedDeckSelected(Vector3 position) {
        unusedDeckClickedEventHandler.Invoke(position);
    }

    public static void DiscardDeckSelected(Vector3 position) {
        discardDeckClickedEventHandler.Invoke(position);
    }

    // DECK INTERACTIONS

    public static void DeckHover(bool isHover) {
        deckHoverEventHandler.Invoke(isHover);
    }

    public static void DeckClick() {
        deckClickedEventHander.Invoke();
        guiController.DisplayDeck();
    }

    public static void DeckClose() {
        deckClosedEventHander.Invoke();
        guiController.CloseDeck();
    }

    public static void SelectDeckCard(CardData cardData, int index) {
        deckCardSelectedEventHandler.Invoke(cardData, index);
    }

    // DIALOGUE

    public static void HandleQuestion(Question question) {
        questionEventHandler.Invoke(question);
        guiController.UpdateConverseGUI(question);
    }

    public static void HandleDialogue(string unlockKey) {
        dialogueEventHandler.Invoke(unlockKey);
        guiController.UpdateConverseGUI(unlockKey);
    }

    public static void HandleDialogueSetting(string arg) {
        dialogueSettingEventHandler.Invoke(arg);
    }

    public static void HandleStopConverse() {
        stopConverseEventHandler.Invoke(data.selectedCard);
        guiController.StopConverse();
    }

    public static void HandleGoodbye() {
        npcLeavesEventHandler.Invoke();
        guiController.StopConverse();
    }

    // NPCS

    public static void NPCIntroStart(GameObject card) {
        npcIntroStartEventHandler.Invoke(card);
    }

    public static void NPCIntroEnd(GameObject card) {
        npcIntroEndEventHandler.Invoke(card);
    }

    public static void LeaveTavernEarly() {
        endDayEarlyEventHandler.Invoke();
        guiController.StopConverse();
    }

    public static void NPCExitTavern(GameObject card) {
        npcLeftTavernEventHandler.Invoke(card);
    }

    public static void ClickConclusionBackground(Vector3 position) {
        conclusionBackgroundClicked.Invoke(position);
    }

    public static void ClickTavern(Vector3 position) {
        tavernClicked.Invoke(position);
    }

    // HEXES

    public static void HoverOverHex(int id) {
        hoverOverHexEventHandler.Invoke(id);
    }

    public static void HoverExitHex(int id) {
        hoverExitHexEventHandler.Invoke(id);
    }

    public static void ClickHex(int id) {
        hexClickedEventHandler.Invoke(id);
    }

    // MISC
    public void ChooseLocation(GameData.Location location) {
        chooseLocationEventHandler.Invoke(location);
    }

    public static void TickClock(int currentHour) {
        clockTickedEventHandler.Invoke(currentHour);
    }

    public static void ClickTutorialScreen(GameObject currentScreen, GameObject nextScreen) {
        tutorialScreenClickedEventHandler.Invoke(currentScreen, nextScreen);
    }
    
    // wrapper methods for Unity buttons
    // since they can't call static methods
    public void HandleStopConverseWrapper() { HandleStopConverse(); }
    public void HandleGoodbyeWrapper() { HandleGoodbye(); }
    public void LeaveTavernEarlyWrapper() { LeaveTavernEarly(); }
    public void ConfirmScenarioWrapper() { CheckAnswers(); }
    public void EndResultsWrapper() { EndResults(); }
    public void StartGameWrapper() { StartGame(); }
    public void DeckClickWrapper() { DeckClick(); }
    public void DeckCloseWrapper() { DeckClose(); }
}