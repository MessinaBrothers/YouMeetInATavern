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
        InputController.dialogueEventHandler += HandleDialogue;
        InputController.dialogueSettingEventHandler += HandleSetting;
        InputController.endResultsEventHandler += ClearQuestions;
    }

    void OnDisable() {
        InputController.questionEventHandler -= HandleQuestion;
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.dialogueSettingEventHandler -= HandleSetting;
        InputController.endResultsEventHandler -= ClearQuestions;
    }

    private void HandleSetting(string arg) {
        print("Handling setting: " + arg);
        if (arg == "GOODBYE=FALSE") {
            data.isGoodbyeEnabled = false;
        } else if (arg == "GOODBYE=TRUE") {
            data.isGoodbyeEnabled = true;
        }
    }

    public void HandleQuestion(Question question) {
        int endIDIndex = question.text.IndexOf('>');
        string unlockKey = question.text.Substring("<".Length, endIDIndex - "<".Length);

        // unlock the dialogue
        DeckController.Add(unlockKey);

        // mark as asked all questions containing the question key so they never appear again
        foreach (Question q in data.npc_questions[data.selectedCard.GetComponent<NPC>().key]) {
            if (question.key == q.key) {
                q.isAskedByPlayer = true;
            }
        }
    }

    public void HandleDialogue(string unlockKey) {
        // unlock the dialogue
        DeckController.Add(unlockKey);
    }

    private void ClearQuestions() {
        // for each NPC
        foreach (KeyValuePair<string, List<Question>> kvp in data.npc_questions) {
            foreach (Question question in kvp.Value) {
                question.isAskedByPlayer = false;
            }
        }
    }
}
