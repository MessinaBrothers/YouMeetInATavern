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
        InputController.checkAnswersEventHandler += CheckAnswers;
        InputController.fadedInEventHandler += FadedIn;
        InputController.fadedOutEventHandler += FadedOut;
    }

    void OnDisable() {
        InputController.endResultsEventHandler -=  LoadScenario;
        InputController.gameModeChangedEventHandler -= ChangeMode;
        InputController.checkAnswersEventHandler -= CheckAnswers;
        InputController.fadedInEventHandler -= FadedIn;
        InputController.fadedOutEventHandler -= FadedOut;
    }

    private void LoadScenario() {
        // reset day if this is a new scenario
        if (data.scenario == null || data.scenario.id != data.scenarios[data.nextScenarioIndex].id) {
            data.dayCount = 0;

            data.scenario = data.scenarios[data.nextScenarioIndex];

            InputController.StartNewScenario();
        }

        InputController.ChangeMode(GameData.GameMode.INTRODUCE);
        
        InputController.StartDay();

        data.tavernOpenHour = data.tavernCloseHour - data.scenario.openHours;
        data.currentHour = data.tavernOpenHour;
        InputController.TickClock(data.currentHour);
    }

    private void ChangeMode(GameData.GameMode mode) {
        data.gameMode = mode;
    }

    private void CheckAnswers() {

    }

    private void LockAnswers() {
        
    }

    private void FadedIn() {
        switch (data.gameMode) {
            case GameData.GameMode.INTRODUCE:
                StartCoroutine(StartGame());
                break;
            default:
                break;
        }
    }

    private IEnumerator StartGame() {
        yield return new WaitForSeconds(data.tutorialIntroPauseItem / data.DEBUG_SPEED_EDITOR);
        InputController.IntroduceNPCs();
    }

    private void FadedOut() {
        switch (data.gameMode) {
            case GameData.GameMode.RESULTS:
                InputController.ConfirmScenario();
                break;
            default:
                InputController.EndDay();
                break;
        }
    }
}
