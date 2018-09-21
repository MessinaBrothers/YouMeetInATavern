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
        
        data.nextDialogueIntroKey = GameData.DIALOGUE_INTRO;

        InputController.GameInitialized();

        // load scenario
        LoadScenario();
    }

    void OnEnable() {
        InputController.endResultsEventHandler += LoadScenario;
        InputController.gameModeChangedEventHandler += ChangeMode;
        InputController.fadedInEventHandler += IntroduceNPCs;
        InputController.fadedOutEventHandler += EndDay;
    }

    void OnDisable() {
        InputController.endResultsEventHandler -=  LoadScenario;
        InputController.gameModeChangedEventHandler -= ChangeMode;
        InputController.fadedInEventHandler -= IntroduceNPCs;
        InputController.fadedOutEventHandler -= EndDay;
    }

    private void LoadScenario() {
        // reset day if this is a new scenario
        if (data.scenario == null || data.scenario.id != data.scenarios[data.nextScenarioIndex].id) {
            data.dayCount = 0;

            data.scenario = data.scenarios[data.nextScenarioIndex];

            InputController.StartNewScenario();
        }
        
        InputController.StartDay();

        data.currentHour = data.tavernOpenHour;
        InputController.TickClock(data.currentHour);
    }

    private void ChangeMode(GameData.GameMode mode) {
        data.gameMode = mode;
    }

    private void IntroduceNPCs() {
        InputController.IntroduceNPCs();
    }

    private void EndDay() {
        InputController.EndDay();
    }
}
