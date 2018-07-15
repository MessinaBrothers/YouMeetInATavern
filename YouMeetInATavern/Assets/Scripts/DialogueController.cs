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
    }

    void OnDisable() {
        InputController.questionEventHandler -= HandleQuestion;
    }

    public void HandleQuestion(uint key, uint unlockKey) {
        // unlock the dialogue
        data.isDialogueIndexUnlocked[unlockKey] = true;
        // remove the question from the NPC so it never appears again
        data.npc_questions[data.selectedCard.GetComponent<NPC>().npcID].Remove(key);
    }
}
