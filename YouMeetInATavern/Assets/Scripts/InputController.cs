using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public static event GameInitializedEventHandler gameInitializedEventHandler;
    public delegate void GameInitializedEventHandler();

    public static event GameModeChangedEventHandler gameModeChangedEventHandler;
    public delegate void GameModeChangedEventHandler(GameData.GameMode mode);

    public static event CardClickedEventHandler cardClickedEventHandler;
    public delegate void CardClickedEventHandler(GameObject card);

    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(string key, string unlockKey);

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(string unlockKey);

    public static event StopConverseEventHandler stopConverseEventHandler;
    public delegate void StopConverseEventHandler(GameObject card);

    public static event NPCLeavesEventHandler npcLeavesEventHandler;
    public delegate void NPCLeavesEventHandler();

    public static event NPCRemovedEventHandler npcLeftTavernEventHandler;
    public delegate void NPCRemovedEventHandler(GameObject card);

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

    public static event ChooseLocationEventHandler chooseLocationEventHandler;
    public delegate void ChooseLocationEventHandler(GameData.Location location);

    public static event ConfirmScenarioChoicesEventHandler confirmScenarioChoicesEventHandler;
    public delegate void ConfirmScenarioChoicesEventHandler();

    public static event EndResultsEventHandler endResultsEventHandler;
    public delegate void EndResultsEventHandler();

    private static GameData data;
    private static GUIController guiController; //TODO instead call updateGUIEventHandler for all GUIs to update themselves

    void Start() {
        guiController = FindObjectOfType<GUIController>();
        data = FindObjectOfType<GameData>();
    }

    void Update() {
        //DEBUG CONTROLS
        if (Input.GetKeyDown(KeyCode.F1)) {
            DEBUGConcludeScenario();
        }
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
        stopConverseEventHandler.Invoke(data.selectedCard);
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

    public static void ChangeMode(GameData.GameMode mode) {
        gameModeChangedEventHandler.Invoke(mode);
    }

    public static void LeaveTavernEarly() {
        endDayEarlyEventHandler.Invoke();
        guiController.StopConverse();
    }

    public static void npcExitTavern(GameObject card) {
        npcLeftTavernEventHandler.Invoke(card);
    }

    public static void EndDay() {
        endDayEventHandler.Invoke();
    }

    public static void DEBUGConcludeScenario() {
        GameData data = FindObjectOfType<GameData>();
        foreach (KeyValuePair<string, ItemData> kvp in data.itemData) {
            data.unlockedDialogueKeys.Add(kvp.Key);
        }
        foreach (KeyValuePair<string, NPCData> kvp in data.npcData) {
            data.unlockedDialogueKeys.Add(kvp.Key);
        }
        ConcludeScenario();
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
    public void LeaveTavernEarlyWrapper() { LeaveTavernEarly(); }
    public void EndDayWrapper() { EndDay(); }
    public void ConfirmScenarioWrapper() { ConfirmScenario(); }
    public void EndResultsWrapper() { EndResults(); }
}