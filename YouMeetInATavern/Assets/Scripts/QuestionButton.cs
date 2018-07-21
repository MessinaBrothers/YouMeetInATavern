using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

    public string key;
    public string unlockKey;

    public void BroadcastKey() {
        InputController.HandleQuestion(key, unlockKey);
    }
}
