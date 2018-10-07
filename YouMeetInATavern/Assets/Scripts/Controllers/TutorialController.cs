using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

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
        InputController.checkAnswersEventHandler += CheckAnswers;
        InputController.newScenarioStartedEventHandler += LoadHexes;
    }

    void OnDisable() {
        InputController.checkAnswersEventHandler -= CheckAnswers;
        InputController.newScenarioStartedEventHandler -= LoadHexes;
    }

    private void LoadHexes(GameData data) {
        switch (data.scenario.id) {
            case 5:
                townHex.SetActive(true);
                break;
            default:
                break;
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
