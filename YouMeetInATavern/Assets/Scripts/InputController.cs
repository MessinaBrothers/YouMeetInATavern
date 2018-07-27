using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public static event GameInitializedEventHandler gameInitializedEventHandler;
    public delegate void GameInitializedEventHandler();

    public static event CardClickedEventHandler cardClickedEventHandler;
    public delegate void CardClickedEventHandler(GameObject card);

    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(string key, string unlockKey);

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(string unlockKey);

    public static event StopConverseEventHandler stopConverseEventHandler;
    public delegate void StopConverseEventHandler();

    public static event NPCLeavesEventHandler npcLeavesEventHandler;
    public delegate void NPCLeavesEventHandler();

    public static event StartTavernEventHandler startTavernEventHandler;
    public delegate void StartTavernEventHandler();

    public static event StartDayEventHandler startDayEventHandler;
    public delegate void StartDayEventHandler();

    public static event EndDayEventHandler endDayEventHandler;
    public delegate void EndDayEventHandler();

    public static event StartConcludeScenarioEventHandler startConcludeScenarioEventHandler;
    public delegate void StartConcludeScenarioEventHandler();

    public static event ChooseLocationEventHandler chooseLocationEventHandler;
    public delegate void ChooseLocationEventHandler(GameData.Location location);

    public static event ConfirmScenarioChoicesEventHandler confirmScenarioChoicesEventHandler;
    public delegate void ConfirmScenarioChoicesEventHandler();

    public static event EndResultsEventHandler endResultsEventHandler;
    public delegate void EndResultsEventHandler();

    private static GUIController guiController; //TODO instead call updateGUIEventHandler for all GUIs to update themselves

    void Start() {
        guiController = FindObjectOfType<GUIController>();
    }

    void Update() {

    }

    public static void GameInitialized() {
        gameInitializedEventHandler.Invoke();
    }

    public static void HandleCardClick(GameObject card) {
        cardClickedEventHandler.Invoke(card);
    }

    public static void HandleQuestion(string key, string unlockKey) {
        questionEventHandler.Invoke(key, unlockKey);
        guiController.UpdateConverseGUI(unlockKey);
    }

    public static void HandleDialogue(string unlockKey) {
        dialogueEventHandler.Invoke(unlockKey);
        guiController.UpdateConverseGUI(unlockKey);
    }

    public static void HandleStopConverse() {
        stopConverseEventHandler.Invoke();
        guiController.StopConverse();
    }

    public static void HandleGoodbye() {
        npcLeavesEventHandler.Invoke();
        guiController.StopConverse();
    }

    public static void ContinueDay() {
        startTavernEventHandler.Invoke();
        guiController.ContinueDay();
    }

    public static void StartDay() {
        startDayEventHandler.Invoke();
        guiController.StartDay();
    }

    public static void EndDay() {
        endDayEventHandler.Invoke();
    }

    public static void ConcludeScenario() {
        startConcludeScenarioEventHandler.Invoke();
        guiController.ConcludeScenario();
    }

    public void ChooseLocation(GameData.Location location) {
        chooseLocationEventHandler.Invoke(location);
    }

    public static void ConfirmScenario() {
        confirmScenarioChoicesEventHandler.Invoke();
        guiController.ConfirmScenario();
    }

    public static void EndResults() {
        endResultsEventHandler.Invoke();
    }

    // wrapper methods for Unity buttons
    // since they can't call static methods
    public void HandleStopConverseWrapper() { HandleStopConverse(); }
    public void HandleGoodbyeWrapper() { HandleGoodbye(); }
    public void ContinueDayWrapper() { ContinueDay(); }
    public void EndDayWrapper() { EndDay(); }
    public void ConfirmScenarioWrapper() { ConfirmScenario(); }
    public void EndResultsWrapper() { EndResults(); }
}