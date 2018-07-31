using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveTavernEarlyButton : MonoBehaviour {

    private GameData data;
    private Button button;

    void Start() {
        data = FindObjectOfType<GameData>();
        button = GetComponent<Button>();
        
        button.interactable = false;
    }

    void OnEnable() {
        InputController.gameModeChangedEventHandler += ChangeMode;
    }

    void OnDisable() {
        InputController.gameModeChangedEventHandler -= ChangeMode;
    }

    private void ChangeMode(GameData.GameMode mode) {
        button.interactable = mode == GameData.GameMode.TAVERN;
    }
}
