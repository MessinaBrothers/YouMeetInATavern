﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionGUIController : MonoBehaviour {

    public GameObject[] questionButtons;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }
    
    public void LoadQuestions(GameObject card) {
        NPC npc = card.GetComponent<NPC>();

        // disable all question buttons
        foreach (GameObject button in questionButtons) {
            button.SetActive(false);
        }

        // ignore unlocked questions if NPC is being introduced
        if (npc.isBeingIntroduced == true) {
            return;
        }

        // get the list of questions
        Dictionary<uint, string> questions = data.npc_questions[npc.npcID];

        int buttonIndex = 0;

        // for each question
        foreach (KeyValuePair<uint, string> kvp in questions) {
            // if the question is unlocked
            if (data.isDialogueIndexUnlocked[kvp.Key]) {
                // activate the next button
                GameObject button = questionButtons[buttonIndex];
                button.SetActive(true);
                buttonIndex += 1;

                // set the button text to the question text
                SetQuestionText(button, kvp.Key, kvp.Value);

                // what happens when you have more questions than buttons available? Escape
                if (buttonIndex >= questionButtons.Length) {
                    return;
                }
            }
        }
    }

    private void SetQuestionText(GameObject button, uint key, string text) {
        // set the question id
        QuestionButton questionButton = button.GetComponent<QuestionButton>();
        questionButton.key = key;

        // set the unlock dialogue id
        int endIDIndex = text.IndexOf('>');
        uint unlockKey = uint.Parse(text.Substring("<".Length, endIDIndex - "<".Length));
        questionButton.unlockKey = unlockKey;

        // set the button text
        string toDisplay = text.Substring(endIDIndex + ">".Length);
        button.GetComponentInChildren<Text>().text = toDisplay;
    }
}
