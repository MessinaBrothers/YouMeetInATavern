using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(uint key, uint unlockKey);

    public static event StopConverseEventHandler stopConverseEventHandler;
    public delegate void StopConverseEventHandler();

    public static event NPCLeavesEventHandler npcLeavesEventHandler;
    public delegate void NPCLeavesEventHandler();

    public static event StartTavernEventHandler startTavernEventHandler;
    public delegate void StartTavernEventHandler();

    private static GUIController guiController; //TODO instead call updateGUIEventHandler for all GUIs to update themselves

    void Start() {
        guiController = FindObjectOfType<GUIController>();
    }

    void Update() {

    }

    public static void HandleQuestion(uint key, uint unlockKey) {
        questionEventHandler.Invoke(key, unlockKey);
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
}