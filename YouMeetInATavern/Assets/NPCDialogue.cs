using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour {

    private DataInvestigate data;

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(Dialogue dialogue);

    public uint dialogueID;

    void Start() {
        data = FindObjectOfType<DataInvestigate>();
    }

    void Update() {

    }

    public void StartDialgue() {
        dialogueEventHandler.Invoke(data.dialogues[dialogueID]);
    }
}
