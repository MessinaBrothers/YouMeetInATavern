﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionGUIController : MonoBehaviour {

    public GameObject continueButton, goodbyeButton;
    public GameObject[] questionButtons;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    public void LoadQuestions(GameObject card) {
        NPC npc = card.GetComponent<NPC>();

        HideQuestions();

        // ignore unlocked questions if NPC is being introduced
        if (npc.isBeingIntroduced == true) {
            return;
        }

        // get the list of questions
        Dictionary<string, Question> questions = data.npc_questions[npc.key];

        int buttonIndex = 0;
        
        // for each question
        foreach (KeyValuePair<string, Question> kvp in questions) {
            // if the question is unlocked and not already asked
            if (data.unlockedDialogueKeys.Contains(kvp.Key) && kvp.Value.isAskedByPlayer == false) {
                // activate the next button
                GameObject button = questionButtons[buttonIndex];
                button.SetActive(true);
                buttonIndex += 1;

                // set the button text to the question text
                SetQuestionText(button, kvp.Key, kvp.Value.text);

                // what happens when you have more questions than buttons available? Escape
                if (buttonIndex >= questionButtons.Length) {
                    return;
                }
            }
        }
    }

    public void HideQuestions() {
        // disable all question buttons
        foreach (GameObject button in questionButtons) {
            button.SetActive(false);
        }
    }

    public void SetMode(bool isIntro) {
        if (isIntro) {
            continueButton.SetActive(true);
            goodbyeButton.SetActive(false);
        } else {
            continueButton.SetActive(false);
            goodbyeButton.SetActive(true);
        }
    }

    private void SetQuestionText(GameObject button, string key, string text) {
        // set the question id
        QuestionButton questionButton = button.GetComponent<QuestionButton>();
        questionButton.key = key;

        // set the unlock dialogue id
        int endIDIndex = text.IndexOf('>');
        string unlockKey = text.Substring("<".Length, endIDIndex - "<".Length);
        questionButton.unlockKey = unlockKey;

        // set the button text
        string toDisplay = text.Substring(endIDIndex + ">".Length);
        button.GetComponentInChildren<Text>().text = toDisplay;
    }
}