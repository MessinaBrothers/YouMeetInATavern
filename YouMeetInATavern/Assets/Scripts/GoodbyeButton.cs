using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodbyeButton : MonoBehaviour {

    public void BroadcastKey() {
        InputController.HandleGoodbye();
    }
}
