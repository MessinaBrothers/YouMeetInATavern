using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour {

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(int key);

    public int key;

    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        dialogueEventHandler.Invoke(key);

        //TODO observe the dialogue key somewhere
    }
}
