using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

    public uint key;

    public void BroadcastKey() {
        InputController.HandleQuestion(key);
    }
}
