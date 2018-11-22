using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
    }

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        print("Game initialized. Loading scenario...");
        
        data.nextDialogueIntroKey = GameData.DIALOGUE_INTRO;

        InputController.GameInitialized();

        // load scenario
        LoadScenario();
    }

    void OnEnable() {
        InputController.endResultsEventHandler += LoadScenario;
        InputController.gameflowModeChange += ModeChanged;
        InputController.npcLeftTavernEventHandler += CheckEmptyTavern;
        InputController.gameflowStartFinishConclusion += FinishConclusion;
    }

    void OnDisable() {
        InputController.endResultsEventHandler -=  LoadScenario;
        InputController.gameflowModeChange -= ModeChanged;
        InputController.npcLeftTavernEventHandler -= CheckEmptyTavern;
        InputController.gameflowStartFinishConclusion -= FinishConclusion;
    }

    private void LoadScenario() {
        // reset day if this is a new scenario
        if (data.scenario == null || data.scenario.id != data.scenarios[data.nextScenarioIndex].id) {
            data.dayCount = 0;

            data.scenario = data.scenarios[data.nextScenarioIndex];
        } else {
            data.dayCount += 1;
        }

        InputController.StartBeginTavern(data.dayCount);

        InputController.ChangeMode(GameData.GameMode.INTRODUCE);

        data.tavernOpenHour = data.tavernCloseHour - data.scenario.openHours;
        data.currentHour = data.tavernOpenHour;

        InputController.EndBeginTavern();

        StartCoroutine(IntroduceNPCs());

        InputController.TickClock(data.currentHour);
    }

    private void ModeChanged(GameData.GameMode mode) {
        data.gameMode = mode;
    }

    private void FinishConclusion() {
        StartCoroutine(ConfirmScenario());
    }

    private void CheckEmptyTavern(GameObject card) {
        if (data.npcsInTavern.Count == 0) {
            InputController.StartFinishTavern();
            StartCoroutine(ConcludeScenario());
        }
    }

    private IEnumerator IntroduceNPCs() {
        yield return new WaitForSeconds((data.fadeInTime + data.introPauseTime) / data.DEBUG_SPEED_EDITOR);
        InputController.IntroduceNPCs();
    }

    private IEnumerator ConcludeScenario() {
        yield return new WaitForSeconds(data.fadeOutTime / data.DEBUG_SPEED_EDITOR);
        InputController.EndFinishTavern();
        InputController.ConcludeScenario();
    }

    private IEnumerator ConfirmScenario() {
        yield return new WaitForSeconds(data.fadeOutTime / data.DEBUG_SPEED_EDITOR);
        InputController.ConfirmScenario();
    }
}
