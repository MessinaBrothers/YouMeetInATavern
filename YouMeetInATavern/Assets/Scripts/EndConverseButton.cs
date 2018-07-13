using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndConverseButton : MonoBehaviour {

    public static event EndConverseEventHandler endConverseEventHandler;
    public delegate void EndConverseEventHandler();
    
    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        endConverseEventHandler.Invoke();
    }
}
