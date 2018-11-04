﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public GameObject clickBlocker;
    public GameObject tutorialConcludeHints;
    public GameObject leaveTavernButton;

    public Transform midcardTransform;

    public GameObject townHex, forestHex, roadHex, mountainHex, waterHex;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        townHex.SetActive(false);
        forestHex.SetActive(false);
        roadHex.SetActive(false);
        mountainHex.SetActive(false);
        waterHex.SetActive(false);
    }

    void Update() {

    }

    void OnEnable() {
        InputController.newScenarioStartedEventHandler += LoadScenario;
        InputController.tutorialScreenClickedEventHandler += NextTutorialScreen;
        InputController.checkAnswersEventHandler += CheckAnswers;
    }

    void OnDisable() {
        InputController.newScenarioStartedEventHandler -= LoadScenario;
        InputController.tutorialScreenClickedEventHandler -= NextTutorialScreen;
        InputController.checkAnswersEventHandler -= CheckAnswers;
    }

    private void LoadScenario(GameData data) {
        data.fadeInTime = data.scenario.fadeInTime;
        data.introPauseTime = data.scenario.introPauseTime;

        switch (data.scenario.id) {
            case 5:
                townHex.SetActive(true);
                tutorialConcludeHints.SetActive(true);
                clickBlocker.SetActive(true);
                leaveTavernButton.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void NextTutorialScreen(GameObject currentScreen, GameObject nextScreen) {
        currentScreen.SetActive(false);
        if (nextScreen == null) {
            clickBlocker.SetActive(false);
        } else {
            nextScreen.SetActive(true);
            leaveTavernButton.SetActive(true);
        }
    }

    private void CheckAnswers() {
        switch (data.scenario.id) {
            case 5:
                if (data.chosenAnswerKeys.Contains("NPC_BARTENDER") == false) {
                    PopUpDialogueFactory.Create("Don't leave without me!", 3, midcardTransform);
                } else {
                    LockAnswers();
                }
                break;
            default:
                LockAnswers();
                break;
        }
    }

    private void LockAnswers() {
        InputController.ChangeMode(GameData.GameMode.RESULTS);
        InputController.LockAnswers();
    }
}
