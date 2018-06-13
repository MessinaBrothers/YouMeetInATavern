using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInput : DataUser {

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(Dialogue dialogue);

    public static event EndDialogueEventHandler endDialogueEventHandler;
    public delegate void EndDialogueEventHandler();

    void Start() {

    }

    void Update() {

    }

    void OnEnable() {
        TextBoxController.playerDialogueChoiceEventHandler += HandleInput;
    }

    void OnDisable() {
        TextBoxController.playerDialogueChoiceEventHandler -= HandleInput;
    }

    private void HandleInput(uint dialogueIndex) {
        Dialogue dialogue = data.dialogues[dialogueIndex];
        // give player rewards
        // display next dialogue
        if (dialogue.nextDialogues.Count > 0) {
            dialogueEventHandler.Invoke(data.dialogues[dialogue.nextDialogues[0]]);
        } else {
            endDialogueEventHandler.Invoke();
        }
    }
}
