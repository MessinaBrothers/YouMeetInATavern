using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

    public uint key;

    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        InputController.HandleQuestion(key);
    }
}
