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
        InputPlayer.playerInspectEventHandler += PauseGame;
        NPCDialogue.dialogueEventHandler += PauseGame;
        InputPlayer.textContinueEventHandler += UnpauseGame;
        InputDialogue.endDialogueEventHandler += UnpauseGame;
    }

    void OnDisable() {
        InputPlayer.playerInspectEventHandler -= PauseGame;
        NPCDialogue.dialogueEventHandler -= PauseGame;
        InputPlayer.textContinueEventHandler -= UnpauseGame;
        InputDialogue.endDialogueEventHandler -= UnpauseGame;
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

    public void UnpauseGame() {
        Time.timeScale = 1;
    }
}
