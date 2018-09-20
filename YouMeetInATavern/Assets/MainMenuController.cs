using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public float fadeOutTime;
    public FadeImage fadeImage;

    private MusicController musicController;

    void Start() {
        musicController = FindObjectOfType<MusicController>();
    }
    
    void OnEnable() {
        InputController.startGameEventHandler += FadeOut;
    }

    void OnDisable() {
        InputController.startGameEventHandler -= FadeOut;
    }

    private void FadeOut() {
        fadeImage.FadeOut(fadeOutTime);
        musicController.TransitionSilent(fadeOutTime);
    }
}
