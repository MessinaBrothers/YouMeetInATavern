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
        EndConverseButton.endConverseEventHandler += Stop;
    }

    void OnDisable() {
        DialogueButton.dialogueEventHandler -= HandleDialogue;
        EndConverseButton.endConverseEventHandler -= Stop;
    }

    private void HandleDialogue(uint key) {
        if (key == 0) {
            //  dialoguePanel.SetActive(false);
        }
    }

    private void Stop(GameObject card) {
        dialoguePanel.SetActive(false);
    }
}
