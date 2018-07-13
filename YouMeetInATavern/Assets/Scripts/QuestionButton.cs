using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

    public static event QuestionEventHandler questionEventHandler;
    public delegate void QuestionEventHandler(uint key);

    public uint key;

    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        questionEventHandler.Invoke(key);
    }
}
