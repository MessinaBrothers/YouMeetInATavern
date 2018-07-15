using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.questionEventHandler += HandleQuestion;
    }

    void OnDisable() {
        InputController.questionEventHandler -= HandleQuestion;
    }

    public void HandleQuestion(uint key) {
        data.isDialogueIndexUnlocked[key] = true;
    }
}
