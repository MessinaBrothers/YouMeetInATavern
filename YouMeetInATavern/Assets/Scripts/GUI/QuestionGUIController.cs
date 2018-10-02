using System;
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
        
        List<Question> questions = data.npc_questions[npc.key];

        int buttonIndex = 0;

        foreach (Question question in questions) {
            if (DeckController.Contains(question.key) && question.isAskedByPlayer == false) {
                // activate the next button
                GameObject button = questionButtons[buttonIndex];
                button.SetActive(true);
                buttonIndex += 1;
                
                SetQuestionText(button, question);

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

    private void SetQuestionText(GameObject button, Question question) {
        // set the question id
        QuestionButton questionButton = button.GetComponent<QuestionButton>();
        questionButton.question = question;

        // set the button text
        int endIDIndex = question.text.IndexOf('>');
        string toDisplay = question.text.Substring(endIDIndex + ">".Length);
        button.GetComponentInChildren<Text>().text = toDisplay;
    }
}
