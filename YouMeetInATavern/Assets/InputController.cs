using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private int playerID;

    private InputDialogue inputDialogue;
    private InputPlayer inputPlayer;
    private InputKnockback inputKnockback;

    private MyInput currentInput;

    // prevents the same input responding to two input states
    private bool justSwitched;

    void Start() {
        playerID = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityID>().id;

        inputDialogue = GetComponent<InputDialogue>();
        inputPlayer = GetComponent<InputPlayer>();
        inputKnockback = GetComponent<InputKnockback>();

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
        InputKnockback.knockbackDoneEventHandler += StartPlayerInput;
        Health.receiveDamageEventHandler += GetHit;
    }

    void OnDisable() {
        NPCDialogue.dialogueEventHandler -= StartDialogueInput;
        InputDialogue.endDialogueEventHandler -= StartPlayerInput;
        InputKnockback.knockbackDoneEventHandler -= StartPlayerInput;
        Health.receiveDamageEventHandler -= GetHit;
    }

    private void GetHit(int id, int damageAmount, GameObject weaponObject) {
        if (id == playerID) {
            currentInput = inputKnockback;
            inputKnockback.Knockback(weaponObject.transform.position);
            justSwitched = true;
        }
    }

    private void StartDialogueInput(Dialogue dialogue) {
        currentInput = inputDialogue;
        justSwitched = true;
        //dialogueInput.enabled = true;
        //playerInput.enabled = false;
    }

    private void StartPlayerInput() {
        currentInput = inputPlayer;
        justSwitched = true;
        //dialogueInput.enabled = false;
        //playerInput.enabled = true;
    }
}
