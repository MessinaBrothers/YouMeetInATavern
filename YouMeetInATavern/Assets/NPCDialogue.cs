using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : DataUser {

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(Dialogue dialogue);

    public uint dialogueID;

    void Start() {

    }

    void Update() {

    }

    void OnEnable() {
        InputPlayer.playerInteractEventHandler += Chat;
    }

    void OnDisable() {
        InputPlayer.playerInteractEventHandler -= Chat;
    }

    private void Chat(GameObject interactable) {
        if (interactable == gameObject) {
            dialogueEventHandler.Invoke(data.dialogues[dialogueID]);
        }
    }
}
