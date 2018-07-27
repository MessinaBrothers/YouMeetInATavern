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
    }

    void OnDisable() {
        InputController.questionEventHandler -= HandleQuestion;
        InputController.dialogueEventHandler -= HandleDialogue;
    }

    public void HandleQuestion(string key, string unlockKey) {
        // unlock the dialogue
        data.unlockedDialogueKeys.Add(unlockKey);
        // remove the question from the NPC so it never appears again
        data.npc_questions[data.selectedCard.GetComponent<NPC>().key].Remove(key);
    }

    public void HandleDialogue(string unlockKey) {
        // unlock the dialogue
        data.unlockedDialogueKeys.Add(unlockKey);
    }
}
