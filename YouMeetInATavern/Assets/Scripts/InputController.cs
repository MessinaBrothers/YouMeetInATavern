using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(uint key);

    public static event StopConverseEventHandler stopConverseEventHandler;
    public delegate void StopConverseEventHandler();

    private static GUIController guiController; //TODO instead call updateGUIEventHandler for all GUIs to update themselves

    void Start() {
        guiController = FindObjectOfType<GUIController>();
    }

    void Update() {

    }

    public static void HandleQuestion(uint key) {
        questionEventHandler.Invoke(key);
        guiController.UpdateGUI(key);
    }

    public static void HandleStopConverse() {
        stopConverseEventHandler.Invoke();
        guiController.StopConverse();
    }
}