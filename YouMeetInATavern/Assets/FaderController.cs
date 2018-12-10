using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderController : MonoBehaviour {

    private GameData data;
    private FadeImage fader;

    void Start() {
        data = GameData.GetInstance();
        fader = GetComponent<FadeImage>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.gameflowEndBeginTavern += FadeIn;
        InputController.gameflowStartBeginConclusion += FadeIn;
        InputController.confirmScenarioChoicesEventHandler += FadeIn;
        InputController.gameflowStartFinishTavern += FadeIn;

        InputController.gameflowStartGame += FadeOut;
        InputController.gameflowStartFinishConclusion += FadeOut;
    }

    void OnDisable() {
        InputController.gameflowEndBeginTavern -= FadeIn;
        InputController.gameflowStartBeginConclusion -= FadeIn;
        InputController.confirmScenarioChoicesEventHandler -= FadeIn;
        InputController.gameflowStartFinishTavern -= FadeIn;

        InputController.gameflowStartGame -= FadeOut;
        InputController.gameflowStartFinishConclusion -= FadeOut;
    }

    private void FadeIn() {
        fader.FadeIn(data.fadeInTime);
    }

    private void FadeOut() {
        fader.FadeOut(data.fadeOutTime);
    }
}
