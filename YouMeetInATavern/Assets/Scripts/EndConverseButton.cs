using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndConverseButton : MonoBehaviour {

    public void BroadcastKey() {
        InputController.HandleStopConverse();
    }
}
