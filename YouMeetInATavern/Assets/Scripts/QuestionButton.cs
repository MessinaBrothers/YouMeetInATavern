using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

    public uint key;
    public uint unlockKey;

    public void BroadcastKey() {
        InputController.HandleQuestion(key, unlockKey);
    }
}
