using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCamImageController : MonoBehaviour {

    private GameData data;
    private FadeImage fader;

    void Start() {
        data = GameData.GetInstance();
        fader = GetComponent<FadeImage>();
    }

    void OnEnable() {
        InputController.gameflowEndBeginTavern += FadeIn;
    }

    void OnDisable() {
        InputController.gameflowEndBeginTavern -= FadeIn;
    }

    private void FadeIn() {
        fader.FadeOut(data.fadeInTime);
    }
}
