using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private GameData data;

    private uint nextScenarioIndex;

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        print("Game initialized. Loading scenario...");

        data = FindObjectOfType<GameData>();

        // load scenario
        nextScenarioIndex = 0;
        NextScenario();
        
        data.nextDialogueIntroKey = GameData.DIALOGUE_INTRO;

        InputController.GameInitialized();
    }

    void OnEnable() {
        InputController.endResultsEventHandler += NextScenario;
    }

    void OnDisable() {
        InputController.endResultsEventHandler -=  NextScenario;
    }

    private void NextScenario() {
        data.scenario = data.scenarios[nextScenarioIndex];
        nextScenarioIndex += 1;

        data.dayCount = 0;

        InputController.StartDay();
    }
}
