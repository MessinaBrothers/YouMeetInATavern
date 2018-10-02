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
        InputController.endResultsEventHandler += ClearQuestions;
    }

    void OnDisable() {
        InputController.questionEventHandler -= HandleQuestion;
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.endResultsEventHandler -= ClearQuestions;
    }

    public void HandleQuestion(Question question) {
        int endIDIndex = question.text.IndexOf('>');
        string unlockKey = question.text.Substring("<".Length, endIDIndex - "<".Length);

        // unlock the dialogue
        DeckController.Add(unlockKey);
        // remove the question from the NPC so it never appears again
        // data.npc_questions[data.selectedCard.GetComponent<NPC>().key].Remove(key);
        // mark the question as asked
        question.isAskedByPlayer = true;
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
