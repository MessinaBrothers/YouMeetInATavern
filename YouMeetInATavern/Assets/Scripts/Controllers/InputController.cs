using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    
    // GAME FLOW
    public static event GameInitializedEventHandler gameInitializedEventHandler;
    public delegate void GameInitializedEventHandler();

    public static event GameModeChangedEventHandler gameModeChangedEventHandler;
    public delegate void GameModeChangedEventHandler(GameData.GameMode mode);

    public static event StartNewScenarioEventHandler newScenarioStartedEventHandler;
    public delegate void StartNewScenarioEventHandler(GameData data);

    public static event StartTavernEventHandler startTavernEventHandler;
    public delegate void StartTavernEventHandler();

    public static event StartDayEventHandler startDayEventHandler;
    public delegate void StartDayEventHandler();

    public static event EndDayEarlyEventHandler endDayEarlyEventHandler;
    public delegate void EndDayEarlyEventHandler();

    public static event EndDayEventHandler endDayEventHandler;
    public delegate void EndDayEventHandler();

    public static event StartConcludeScenarioEventHandler startConcludeScenarioEventHandler;
    public delegate void StartConcludeScenarioEventHandler();

    public static event ConfirmScenarioChoicesEventHandler confirmScenarioChoicesEventHandler;
    public delegate void ConfirmScenarioChoicesEventHandler();

    public static event EndResultsEventHandler endResultsEventHandler;
    public delegate void EndResultsEventHandler();

    // CARD INTERACTIONS
    public static event CardClickedEventHandler cardClickedEventHandler;
    public delegate void CardClickedEventHandler(GameObject card);

    public static event DialogueCardCreatedEventHandler dialogueCardCreatedEventHandler;
    public delegate void DialogueCardCreatedEventHandler(GameObject card);

    public static event CardEnteredDeckEventHandler cardEnteredDeckEventHandler;
    public delegate void CardEnteredDeckEventHandler(GameObject card);

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
    public delegate void QuestionEventHandler(string key, string unlockKey);

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(string unlockKey);

    public static event StopConverseEventHandler stopConverseEventHandler;
    public delegate void StopConverseEventHandler(GameObject card);

    // NPCS
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
        gameInitializedEventHandler.Invoke();
    }

    public static void StartNewScenario() {
        newScenarioStartedEventHandler.Invoke(data);
        guiController.StartScenario(data);
    }

    public static void ContinueDay() {
        startTavernEventHandler.Invoke();
        guiController.ContinueDay();
    }

    public static void StartDay() {
        startDayEventHandler.Invoke();
        guiController.StartDay();
    }

    public static void ChangeMode(GameData.GameMode mode) {
        gameModeChangedEventHandler.Invoke(mode);
    }

    public static void EndDay() {
        endDayEventHandler.Invoke();
    }

    public static void ConcludeScenario() {
        startConcludeScenarioEventHandler.Invoke();
        guiController.ConcludeScenario();
    }

    public static void ConfirmScenario() {
        confirmScenarioChoicesEventHandler.Invoke();
        guiController.ConfirmScenario();
    }

    public static void EndResults() {
        endResultsEventHandler.Invoke();
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

    public static void HandleQuestion(string key, string unlockKey) {
        questionEventHandler.Invoke(key, unlockKey);
        guiController.UpdateConverseGUI(unlockKey);
    }

    public static void HandleDialogue(string unlockKey) {
        dialogueEventHandler.Invoke(unlockKey);
        guiController.UpdateConverseGUI(unlockKey);
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

    public static void LeaveTavernEarly() {
        endDayEarlyEventHandler.Invoke();
        guiController.StopConverse();
    }

    public static void npcExitTavern(GameObject card) {
        npcLeftTavernEventHandler.Invoke(card);
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
    
    // wrapper methods for Unity buttons
    // since they can't call static methods
    public void HandleStopConverseWrapper() { HandleStopConverse(); }
    public void HandleGoodbyeWrapper() { HandleGoodbye(); }
    public void ContinueDayWrapper() { ContinueDay(); }
    public void LeaveTavernEarlyWrapper() { LeaveTavernEarly(); }
    public void EndDayWrapper() { EndDay(); }
    public void ConfirmScenarioWrapper() { ConfirmScenario(); }
    public void EndResultsWrapper() { EndResults(); }
    public void DeckClickWrapper() { DeckClick(); }
    public void DeckCloseWrapper() { DeckClose(); }
}