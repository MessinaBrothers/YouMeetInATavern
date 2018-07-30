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

    public void HandleQuestion(string key, string unlockKey) {
        // unlock the dialogue
        data.unlockedDialogueKeys.Add(unlockKey);
        // remove the question from the NPC so it never appears again
        // data.npc_questions[data.selectedCard.GetComponent<NPC>().key].Remove(key);
        // mark the question as asked
        data.npc_questions[data.selectedCard.GetComponent<NPC>().key][key].isAskedByPlayer = true;
    }

    public void HandleDialogue(string unlockKey) {
        // unlock the dialogue
        data.unlockedDialogueKeys.Add(unlockKey);
    }

    private void ClearQuestions() {
        // for each NPC
        foreach (KeyValuePair<string, Dictionary<string, Question>> kvpNPC in data.npc_questions) {
            // for each question
            foreach (KeyValuePair<string, Question> kvpQuestions in kvpNPC.Value) {
                kvpQuestions.Value.isAskedByPlayer = false;
            }
        }
    }
}
