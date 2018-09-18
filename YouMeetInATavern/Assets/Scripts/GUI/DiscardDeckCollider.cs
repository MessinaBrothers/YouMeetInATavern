﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeckCollider : MonoBehaviour {

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            InputController.DiscardDeckSelected();
        }
    }
}
