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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <returns>Returns true if questions exist.</returns>
    public bool LoadQuestions(GameObject card) {
        NPC npc = card.GetComponent<NPC>();

        HideQuestions();

        // ignore unlocked questions if NPC is being introduced
        if (npc.isBeingIntroduced == true) {
            return false;
        }
        
        List<Question> questions = data.npc_questions[npc.key];

        int buttonIndex = 0;
        bool isQuestionExist = false;

        foreach (Question question in questions) {
            if (DeckController.Contains(question.key) && question.isAskedByPlayer == false) {
                isQuestionExist = true;
                // activate the next button
                GameObject button = questionButtons[buttonIndex];
                button.SetActive(true);
                buttonIndex += 1;
                
                SetQuestionText(button, question);

                // what happens when you have more questions than buttons available? Escape
                if (buttonIndex >= questionButtons.Length) {
                    return true;
                }
            }
        }

        return isQuestionExist;
    }

    public void HideQuestions() {
        // disable all question buttons
        foreach (GameObject button in questionButtons) {
            button.SetActive(false);
        }
    }

    public void ShowContinueButton() {
        continueButton.SetActive(true);
        goodbyeButton.SetActive(false);
    }

    public void ShowGoodbyeButton() {
        continueButton.SetActive(false);
        goodbyeButton.SetActive(true);
    }

    public void HideGoodbyeButton() {
        continueButton.SetActive(false);
        goodbyeButton.SetActive(false);
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
