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
    }

    void OnDisable() {
        InputController.startGameEventHandler -= FadeOut;
    }

    private void FadeOut() {
        fadeImage.FadeOut(fadeOutTime);
        musicController.TransitionSilent(fadeOutTime);
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene() {
        yield return new WaitForSeconds(fadeOutTime);
        loadScreen.SetActive(true);
        SceneManager.LoadScene("Main");
    }
}
