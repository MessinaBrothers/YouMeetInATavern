using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResponseGUIController : MonoBehaviour {

    public GameObject continueButton, goodbyeButton;
    public GameObject[] questionButtons;

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <returns>Returns true if questions exist.</returns>
    public bool LoadQuestions(Dialogue dialogue) {
        bool isQuestionExist = false;

        int buttonIndex = questionButtons.Length - 1;

        // check inquiries for any questions
        foreach (string inquiryKey in dialogue.inquiryKeys) {
            foreach (string responseKey in data.key_dialoguesNEW[inquiryKey].playerResponseKeys) {
                if (GraphUtility.IsTagInquiryPass(data, inquiryKey)) {
                    isQuestionExist = true;

                    ShowQuestionButton(buttonIndex, responseKey);
                    buttonIndex -= 1;

                    // what happens when you have more questions than buttons available? Escape
                    if (buttonIndex < 0) {
                        return true;
                    }
                }
            }
        }

        // show default questions
        foreach (String responseKey in dialogue.playerResponseKeys) {
            isQuestionExist = true;
            
            ShowQuestionButton(buttonIndex, responseKey);
            buttonIndex -= 1;

            // what happens when you have more questions than buttons available? Escape
            if (buttonIndex < 0) {
                return true;
            }
        }

        return isQuestionExist;
    }

    public void ShowQuestionButton(int index, string key) {
        GameObject button = questionButtons[index];
        button.SetActive(true);
        
        Dialogue dialogue = data.key_dialoguesNEW[key];

        // set the question dialogue
        QuestionButton questionButton = button.GetComponent<QuestionButton>();
        questionButton.dialogue = dialogue;

        button.GetComponentInChildren<Text>().text = dialogue.text;
    }

    public void ShowContinueButton(Dialogue dialogue) {
        continueButton.GetComponent<QuestionButton>().dialogue = dialogue;
        ShowContinueButton();
    }

    public void ShowContinueButton() {
        continueButton.SetActive(true);
        goodbyeButton.SetActive(false);
    }

    public void ShowGoodbyeButton() {
        continueButton.SetActive(false);
        goodbyeButton.SetActive(true);
    }

    public void HideAllButtons() {
        foreach (GameObject button in questionButtons) {
            button.SetActive(false);
        }
        continueButton.SetActive(false);
        goodbyeButton.SetActive(false);
    }
}
