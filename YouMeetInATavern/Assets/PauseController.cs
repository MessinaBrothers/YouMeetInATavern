using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

    void Start() {

    }

    void Update() {

    }

    void OnEnable() {
        PlayerInput.playerInspectEventHandler += PauseGame;
        NPCDialogue.dialogueEventHandler += PauseGame;
        PlayerInput.textContinueEventHandler += Continue;
        DialogueInput.endDialogueEventHandler += Continue;
    }

    void OnDisable() {
        PlayerInput.playerInspectEventHandler -= PauseGame;
        NPCDialogue.dialogueEventHandler -= PauseGame;
        PlayerInput.textContinueEventHandler -= Continue;
        DialogueInput.endDialogueEventHandler -= Continue;
    }

    private void PauseGame(GameObject go) {
        Inspectable inspectable = go.GetComponent<Inspectable>();
        if (inspectable != null) {
            Time.timeScale = 0;
        }
    }

    private void PauseGame(Dialogue dialogue) {
        Time.timeScale = 0;
    }

    private void Continue() {
        Time.timeScale = 1;
    }
}
