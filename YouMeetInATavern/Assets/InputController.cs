using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private InputDialogue dialogueInput;
    private InputPlayer playerInput;

    private MyInput currentInput;

    // prevents the same input responding to two input states
    private bool justSwitched;

    void Start() {
        dialogueInput = GetComponent<InputDialogue>();
        playerInput = GetComponent<InputPlayer>();

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
        if (Input.GetButtonDown("Jump")) {
            currentInput.Handle("Jump");
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        currentInput.Handle(h, v);
    }

    void OnEnable() {
        NPCDialogue.dialogueEventHandler += StartDialogueInput;
        InputDialogue.endDialogueEventHandler += StartPlayerInput;
    }

    void OnDisable() {
        NPCDialogue.dialogueEventHandler -= StartDialogueInput;
        InputDialogue.endDialogueEventHandler -= StartPlayerInput;
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
