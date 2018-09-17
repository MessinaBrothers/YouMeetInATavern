using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectedCollider : MonoBehaviour {

    public int id;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            InputController.ClickCardSelected(id);
        }
    }

    void OnMouseEnter() {
        InputController.HoverOverCardSelected(id);
    }

    void OnMouseExit() {
        InputController.HoverExitCardSelected(id);
    }
}
