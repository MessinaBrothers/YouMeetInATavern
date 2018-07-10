using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGUI : MonoBehaviour {

    public GameObject dialoguePanel;

    void Start() {
        dialoguePanel.SetActive(false);
    }

    void Update() {

    }

    void OnEnable() {
        ConverseNPC.npcStartedTalkingEventHandler += Converse;
    }

    void OnDisable() {
        ConverseNPC.npcStartedTalkingEventHandler -= Converse;
    }

    private void Converse(GameObject card) {
        //TODO display card's specific dialogue
        dialoguePanel.SetActive(true);
    }
}
