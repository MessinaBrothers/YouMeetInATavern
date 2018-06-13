using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private DialogueInput dialogueInput;
    private PlayerInput playerInput;

    void Start() {
        dialogueInput = GetComponent<DialogueInput>();
        playerInput = GetComponent<PlayerInput>();

        StartPlayerInput();
    }

    void Update() {

    }

    void OnEnable() {
        NPCDialogue.dialogueEventHandler += StartDialogueInput;
        DialogueInput.endDialogueEventHandler += StartPlayerInput;
    }

    void OnDisable() {
        NPCDialogue.dialogueEventHandler -= StartDialogueInput;
        DialogueInput.endDialogueEventHandler -= StartPlayerInput;
    }

    private void StartDialogueInput(Dialogue dialogue) {
        dialogueInput.enabled = true;
        playerInput.enabled = false;
    }

    private void StartPlayerInput() {
        dialogueInput.enabled = false;
        playerInput.enabled = true;
    }
}
