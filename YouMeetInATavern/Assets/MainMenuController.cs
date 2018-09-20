using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject loadScreen;
    public FadeImage fadeImage;
    public float fadeOutTime;

    private MusicController musicController;

    void Start() {
        musicController = FindObjectOfType<MusicController>();

        loadScreen.SetActive(false);
    }
    
    void OnEnable() {
        InputController.startGameEventHandler += FadeOut;
        InputController.fadedOutEventHandler += StartGame;
    }

    void OnDisable() {
        InputController.startGameEventHandler -= FadeOut;
        InputController.fadedOutEventHandler -= StartGame;
    }

    private void FadeOut() {
        fadeImage.FadeOut(fadeOutTime);
        musicController.TransitionSilent(fadeOutTime);
    }

    private void StartGame() {
        loadScreen.SetActive(true);
        SceneManager.LoadScene("Main");
    }
}
