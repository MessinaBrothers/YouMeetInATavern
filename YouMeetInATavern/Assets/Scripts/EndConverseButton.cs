using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndConverseButton : MonoBehaviour {

    public void BroadcastKey() {
        InputController.HandleStopConverse();
    }

    public void SetMode(bool isIntro) {
        if (isIntro == true) {
            GetComponentInChildren<Text>().text = "Continue";
        } else {
            GetComponentInChildren<Text>().text = "Goodbye";
        }
    }
}
