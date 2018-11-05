using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveTavernEarlyButton : MonoBehaviour {
    
    private Button button;

    void Start() {
        button = GetComponent<Button>();
        
        button.interactable = false;
    }

    void OnEnable() {
        InputController.gameflowModeChange += ChangeMode;
    }

    void OnDisable() {
        InputController.gameflowModeChange -= ChangeMode;
    }

    private void ChangeMode(GameData.GameMode mode) {
        button.interactable = mode == GameData.GameMode.TAVERN;
    }
}
