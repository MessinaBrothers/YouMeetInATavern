﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndConverseButton : MonoBehaviour {

    public void BroadcastKey() {
        InputController.HandleStopConverse();
    }
}
