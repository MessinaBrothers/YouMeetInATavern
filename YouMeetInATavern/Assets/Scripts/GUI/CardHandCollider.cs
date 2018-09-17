using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandCollider : MonoBehaviour {

    public int id;
    
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            InputController.ClickCardHand(id);
        }
    }

    void OnMouseEnter() {
        InputController.HoverOverCardHand(id);
    }

    void OnMouseExit() {
        InputController.HoverExitCardHand(id);
    }
}
