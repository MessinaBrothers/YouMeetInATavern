﻿using System;
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

        foreach (String responseKey in dialogue.playerResponseKeys) {
            isQuestionExist = true;

            GameObject button = questionButtons[buttonIndex];
            button.SetActive(true);
            buttonIndex -= 1;

            SetQuestionText(button, responseKey);
            // what happens when you have more questions than buttons available? Escape
            if (buttonIndex < 0) {
                return true;
            }
        }

        return isQuestionExist;
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

    private void SetQuestionText(GameObject button, string key) {
        Dialogue dialogue = data.key_dialoguesNEW[key];

        // set the question dialogue
        QuestionButton questionButton = button.GetComponent<QuestionButton>();
        questionButton.dialogue = dialogue;

        button.GetComponentInChildren<Text>().text = dialogue.text;
    }
}
