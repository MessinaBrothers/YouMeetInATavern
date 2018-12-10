using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveTavernEarlyButton : MonoBehaviour {
    
    private Button button;

    private GameData data;

    void Start() {
        button = GetComponent<Button>();
        data = FindObjectOfType<GameData>();
        
        button.interactable = false;
    }

    void OnEnable() {
        InputController.gameflowModeChange += ChangeMode;
    }

    void OnDisable() {
        InputController.gameflowModeChange -= ChangeMode;
    }

    private void ChangeMode(GameData.GameMode mode) {
        if ((mode == GameData.GameMode.TAVERN) && (data.isLeaveButtonEnabled == true)) {
            button.interactable = true;
        } else {
            button.interactable = false;
        }
    }
}
