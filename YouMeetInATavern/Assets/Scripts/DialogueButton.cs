using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour {

    public string unlockKey;

    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        InputController.HandleDialogue(unlockKey);
    }
}
