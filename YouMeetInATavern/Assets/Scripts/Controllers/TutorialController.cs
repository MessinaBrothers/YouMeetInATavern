using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public Transform midcardTransform;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.checkAnswersEventHandler += CheckAnswers;
    }

    void OnDisable() {
        InputController.checkAnswersEventHandler -= CheckAnswers;
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
