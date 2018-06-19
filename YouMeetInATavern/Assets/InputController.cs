using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private DialogueInput dialogueInput;
    private PlayerInput playerInput;

    private MyInput currentInput;

    // prevents the same input responding to two input states
    private bool justSwitched;

    void Start() {
        dialogueInput = GetComponent<DialogueInput>();
        playerInput = GetComponent<PlayerInput>();

        StartPlayerInput();

        justSwitched = false;
    }

    void Update() {
        if (justSwitched) {
            justSwitched = false;
            return;
        }

        if (Input.GetButtonDown("Fire1")) {
            currentInput.Handle("Fire1");
        }
        if (Input.GetButtonDown("Fire2")) {
            currentInput.Handle("Fire2");
        }
        if (Input.GetButtonDown("Fire3")) {
            currentInput.Handle("Fire3");
        }
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
        currentInput = dialogueInput;
        justSwitched = true;
        //dialogueInput.enabled = true;
        //playerInput.enabled = false;
    }

    private void StartPlayerInput() {
        currentInput = playerInput;
        justSwitched = true;
        //dialogueInput.enabled = false;
        //playerInput.enabled = true;
    }
}
