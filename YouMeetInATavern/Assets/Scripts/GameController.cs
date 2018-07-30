using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private GameData data;

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        print("Game initialized. Loading scenario...");

        data = FindObjectOfType<GameData>();

        // load scenario
        LoadScenario();
        
        data.nextDialogueIntroKey = GameData.DIALOGUE_INTRO;

        InputController.GameInitialized();
    }

    void OnEnable() {
        InputController.endResultsEventHandler += LoadScenario;
    }

    void OnDisable() {
        InputController.endResultsEventHandler -=  LoadScenario;
    }

    private void LoadScenario() {
        data.scenario = data.scenarios[data.nextScenarioIndex];

        data.dayCount = 0;

        InputController.StartDay();
    }
}
