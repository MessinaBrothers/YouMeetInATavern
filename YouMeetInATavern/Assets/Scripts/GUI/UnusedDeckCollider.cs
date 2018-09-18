using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnusedDeckCollider : MonoBehaviour {

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            InputController.UnusedDeckSelected();
        }
    }
}
