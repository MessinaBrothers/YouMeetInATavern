using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    public GameObject dialoguePanel;

    void Start() {

    }

    void Update() {

    }

    void OnEnable() {
        DialogueButton.dialogueEventHandler += HandleDialogue;
    }

    void OnDisable() {
        DialogueButton.dialogueEventHandler -= HandleDialogue;
    }

    private void HandleDialogue(int key) {
        print("Handling dialogue with key " + key);
        if (key == 0) {
            dialoguePanel.SetActive(false);
        }
    }
}
