using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public GameObject clickBlocker;
    public GameObject tutorialConcludeHints;

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
        InputController.gameflowStartBeginTavern += LoadDay;
        InputController.tutorialScreenClickedEventHandler += NextTutorialScreen;
        InputController.checkAnswersEventHandler += CheckAnswers;
    }

    void OnDisable() {
        InputController.gameflowStartBeginTavern -= LoadDay;
        InputController.tutorialScreenClickedEventHandler -= NextTutorialScreen;
        InputController.checkAnswersEventHandler -= CheckAnswers;
    }

    private void LoadDay(GameData data, uint dayCount) {
        if (dayCount == 0) {
            data.fadeInTime = data.scenario.fadeInTime;
            data.introPauseTime = data.scenario.introPauseTime;
            data.isLeaveButtonEnabled = true;

            switch (data.scenario.id) {
                case "introduction":
                    townHex.SetActive(true);
                    tutorialConcludeHints.SetActive(true);
                    clickBlocker.SetActive(true);
                    data.isLeaveButtonEnabled = false;
                    break;
                case "stolen_necklace":
                    townHex.SetActive(true);
                    break;
                case "kobolds":
                    townHex.SetActive(true);
                    break;
                default:
                    break;
            }
        } else {
            data.fadeInTime = data.fadeInTimeDefault;
            data.introPauseTime = data.introPauseTimeDefault;
        }
    }

    private void NextTutorialScreen(GameObject currentScreen, GameObject nextScreen) {
        currentScreen.SetActive(false);
        if (nextScreen == null) {
            clickBlocker.SetActive(false);
        } else {
            nextScreen.SetActive(true);
        }
    }

    private void CheckAnswers() {
        if (data.gameMode == GameData.GameMode.CONCLUDE) {
            switch (data.scenario.id) {
                case "introduction":
                    if (data.chosenAnswerKeys.Contains("NPC_BARTENDER") == false) {
                        PopUpDialogueFactory.Create("Don't leave without me!", 3, midcardTransform);
                    } else {
                        InputController.ChangeMode(GameData.GameMode.RESULTS);
                        InputController.FinishConclusion();
                    }
                    break;
                default:
                    InputController.ChangeMode(GameData.GameMode.RESULTS);
                    InputController.FinishConclusion();
                    break;
            }
        }
    }
}
