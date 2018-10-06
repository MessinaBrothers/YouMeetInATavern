using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    private GameData data;

	void Start () {
        data = FindObjectOfType<GameData>();
	}
	
	void Update () {
		
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
                    print("no way jose!)");
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
