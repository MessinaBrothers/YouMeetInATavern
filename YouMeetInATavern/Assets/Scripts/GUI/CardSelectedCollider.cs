using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectedCollider : MonoBehaviour {

    public int id;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == gameObject) {
                    InputController.ClickCardSelected(hit.point, id);
                }
            }
        }
    }

    void OnMouseEnter() {
        InputController.HoverOverCardSelected(id);
    }

    void OnMouseExit() {
        InputController.HoverExitCardSelected(id);
    }
}
