﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHandCollider : MonoBehaviour {

    public int id;
    
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == gameObject) {
                    InputController.ClickCardHand(hit.point, id);
                }
            }
        }
    }

    void OnMouseEnter() {
        if (EventSystem.current.IsPointerOverGameObject() == false) InputController.HoverOverCardHand(id);
    }

    void OnMouseExit() {
        InputController.HoverExitCardHand(id);
    }
}
