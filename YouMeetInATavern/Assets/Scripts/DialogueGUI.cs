using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGUI : MonoBehaviour {

    public GameObject dialoguePanel;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        dialoguePanel.SetActive(false);
    }

    void Update() {

    }

    void OnEnable() {
        CardMoveController.npcInConversePosEventHandler += Converse;
    }

    void OnDisable() {
        CardMoveController.npcInConversePosEventHandler -= Converse;
    }

    private void Converse(GameObject card) {
        // get the next dialogue text
        NPC npc = card.GetComponent<NPC>();
        string text = data.npc_dialogues[npc.npcID][npc.nextDialogueID];
        // set the panel text
        dialoguePanel.GetComponentInChildren<DialoguePanel>().SetDialogue(text);
        // activate the panel
        dialoguePanel.SetActive(true);
    }
}
