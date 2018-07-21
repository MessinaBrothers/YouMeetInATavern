using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour {

    public static event DialogueEventHandler dialogueEventHandler;
    public delegate void DialogueEventHandler(string key);

    public string key;

    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        dialogueEventHandler.Invoke(key);
    }
}
