using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : DataUser {

    public static event PlayerDialogueChoiceEventHandler playerDialogueChoiceEventHandler;
    public delegate void PlayerDialogueChoiceEventHandler(uint dialogueIndex);
    
    public float textSpeed;

    public GameObject textPanel;
    public Text text;
    public GameObject[] choiceButtons;
    public Button continueButton;

    // animations must run even when game is paused, so we avoid Time.deltaTime
    private float lastRealTimeSinceStartup;
    private string stringToDisplay, currentDisplay;
    private float displayCharTime, displayCharTimer;

    void Start() {
        textPanel.SetActive(false);
        stringToDisplay = "";
        currentDisplay = "";

        foreach (GameObject choiceButton in choiceButtons) {
            choiceButton.GetComponentInChildren<Text>().text = "";
        }
    }

    void Update() {
        if (stringToDisplay.Length > 0) {
            float deltaTime = Time.realtimeSinceStartup - lastRealTimeSinceStartup;
            displayCharTimer += deltaTime;
            while (displayCharTimer >= displayCharTime && stringToDisplay.Length > 0) {
                displayCharTimer -= displayCharTime;
                currentDisplay += stringToDisplay[0];
                stringToDisplay = stringToDisplay.Remove(0, 1);
                text.text = currentDisplay;
            }

            // when finished, display any choice buttons with text
            if (stringToDisplay.Length == 0) {
                bool hasChoices = false;
                foreach (GameObject choiceButton in choiceButtons) {
                    if (choiceButton.GetComponentInChildren<Text>().text.Length > 0) {
                        hasChoices = true;
                        choiceButton.SetActive(true);
                    }
                }
                if (hasChoices == true) {
                    // disable the contine button
                    continueButton.gameObject.SetActive(false);
                    // select the first button
                    Button firstButton = choiceButtons[0].GetComponent<Button>();
                    firstButton.Select();
                    firstButton.OnSelect(null);
                } else {
                    // enable the continue button
                    continueButton.gameObject.SetActive(true);
                    continueButton.Select();
                    continueButton.OnSelect(null);
                }
            }

        }
        lastRealTimeSinceStartup = Time.realtimeSinceStartup;
    }

    void OnEnable() {
        PlayerInput.playerInspectEventHandler += DisplayInfo;
        NPCDialogue.dialogueEventHandler += DisplayDialogue;
        DialogueInput.dialogueEventHandler += DisplayDialogue;
        PlayerInput.textContinueEventHandler += Continue;
        DialogueInput.endDialogueEventHandler += Continue;
    }

    void OnDisable() {
        PlayerInput.playerInspectEventHandler -= DisplayInfo;
        NPCDialogue.dialogueEventHandler -= DisplayDialogue;
        DialogueInput.dialogueEventHandler -= DisplayDialogue;
        PlayerInput.textContinueEventHandler -= Continue;
        DialogueInput.endDialogueEventHandler -= Continue;
    }

    private void ChoiceOnClick(uint i) {
        playerDialogueChoiceEventHandler.Invoke(i);
    }

    private void DisplayInfo(GameObject go) {
        Inspectable inspectable = go.GetComponent<Inspectable>();
        if (inspectable != null) {
            StartDisplay(inspectable.description);
        }
    }

    private void DisplayDialogue(Dialogue dialogue) {
        StartDisplay(dialogue.text);

        // clear all choice texts
        foreach (GameObject choiceButton in choiceButtons) {
            choiceButton.GetComponentInChildren<Text>().text = "";
        }

        for (int i = 0; i < dialogue.nextDialogues.Count; i++) {
            // if there are more choices and buttons, ERROR
            if (i == choiceButtons.Length) {
                int choicesNotDisplayed = dialogue.nextDialogues.Count - choiceButtons.Length;
                string choicePostText = choicesNotDisplayed == 1 ? " is" : "s are";
                Debug.LogError(string.Format("Dialogue choice count is greater than button count. {0} choice{1} not displayed.", choicesNotDisplayed, choicePostText));
                return;
            } else {
                GameObject choiceButton = choiceButtons[i];
                // update the button text
                choiceButton.GetComponentInChildren<Text>().text = data.dialogues[dialogue.nextDialogues[i]].text;

                Button button = choiceButton.GetComponent<Button>();
                // remove current listeners
                button.onClick.RemoveAllListeners();
                // add a listener 
                uint index = dialogue.nextDialogues[i]; // because Microsoft https://www.viva64.com/en/b/0468/
                button.onClick.AddListener(delegate {
                    ChoiceOnClick(index);
                });
            }
        }
    }

    private void StartDisplay(string textToDisplay) {
        // display the panel
        textPanel.SetActive(true);

        // initiate displaying the text
        stringToDisplay = textToDisplay;
        displayCharTimer = 0;
        displayCharTime = 1 / textSpeed;

        // clear current strings
        currentDisplay = "";
        text.text = "";
        foreach (GameObject choiceButton in choiceButtons) {
            choiceButton.SetActive(false);
        }
        continueButton.gameObject.SetActive(false);
    }

    public void Continue() {
        textPanel.SetActive(false);
        stringToDisplay = "";
    }
}
