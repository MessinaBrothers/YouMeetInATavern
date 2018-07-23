using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

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

    private static GUIController guiController; //TODO instead call updateGUIEventHandler for all GUIs to update themselves

    void Start() {
        guiController = FindObjectOfType<GUIController>();
    }

    void Update() {

    }

    public static void HandleQuestion(string key, string unlockKey) {
        questionEventHandler.Invoke(key, unlockKey);
        guiController.UpdateConverseGUI(unlockKey);
    }

    public static void HandleDialogue(string unlockKey) {
        Debug.LogFormat("Handling dialogue of key \"{0}\"", unlockKey);
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
        guiController.ConcludeScenario();
    }

    public static void ConfirmScenario() {
        print("Confirming choices");
    }

    // wrapper methods for Unity buttons
    // since they can't call static methods
    public void HandleStopConverseWrapper() { HandleStopConverse(); }
    public void HandleGoodbyeWrapper() { HandleGoodbye(); }
    public void ContinueDayWrapper() { ContinueDay(); }
    public void EndDayWrapper() { EndDay(); }
    public void ConfirmScenarioWrapper() { ConfirmScenario(); }
}