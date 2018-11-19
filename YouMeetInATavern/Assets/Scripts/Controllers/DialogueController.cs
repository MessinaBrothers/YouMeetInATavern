using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.questionEventHandler += HandleQuestion;
        InputController.createCardEventHandler += CreateCard;
        InputController.dialogueSettingEventHandler += HandleSetting;
    }

    void OnDisable() {
        InputController.questionEventHandler -= HandleQuestion;
        InputController.createCardEventHandler -= CreateCard;
        InputController.dialogueSettingEventHandler -= HandleSetting;
    }

    private void HandleSetting(string arg) {
        //print("Handling setting: " + arg);
        if (arg == "GOODBYE=FALSE") {
            data.isGoodbyeEnabled = false;
        } else if (arg == "GOODBYE=TRUE") {
            data.isGoodbyeEnabled = true;
        }
    }

    public void HandleQuestion(Dialogue question) {
        //int endIDIndex = question.text.IndexOf('>');
        //string unlockKey = question.text.Substring("<".Length, endIDIndex - "<".Length);

        //// unlock the dialogue
        //DeckController.Add(unlockKey);
    }

    public void CreateCard(string unlockKey) {
        // unlock the dialogue
        DeckController.Add(unlockKey);
    }
}
