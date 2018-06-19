using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInput : MyInput {

    public GameObject axePrefab;

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(Dialogue dialogue);

    public static event EndDialogueEventHandler endDialogueEventHandler;
    public delegate void EndDialogueEventHandler();

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {

    }

    public override void Handle(string input) {
        
    }

    void OnEnable() {
        TextBoxController.playerDialogueChoiceEventHandler += HandleInput;
    }

    void OnDisable() {
        TextBoxController.playerDialogueChoiceEventHandler -= HandleInput;
    }

    public void EndDialogue() {
        endDialogueEventHandler.Invoke();
    }

    private void HandleInput(uint dialogueIndex) {
        Dialogue dialogue = data.dialogues[dialogueIndex];
        // give player rewards
        if (dialogue.reward.Length > 0) ParseReward(dialogue.reward);

        // display next dialogue
        if (dialogue.nextDialogues.Count > 0) {
            dialogueEventHandler.Invoke(data.dialogues[dialogue.nextDialogues[0]]);
        } else {
            EndDialogue();
        }
    }

    private void ParseReward(string reward) {
        switch (reward) {
            case "axe":
                player.GetComponentInChildren<ItemSlot>().EquipItem(Instantiate(axePrefab));
                break;
            default:
                Debug.LogFormat("ERROR: failed to parse '{0}'. No such reward exists", reward);
                break;
        }
    }
}
