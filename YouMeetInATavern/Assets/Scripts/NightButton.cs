using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightButton : MonoBehaviour {

    public void BroadcastKey() {
        InputController.StartDay();
    }

}
