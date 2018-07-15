using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(uint key);

    void Start() {

    }

    void Update() {

    }

    public static void HandleQuestion(uint key) {
        questionEventHandler.Invoke(key);
        GUIController.UpdateGUI(key);
    }
}
