using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnusedDeckCollider : MonoBehaviour {

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == gameObject) {
                    InputController.UnusedDeckSelected(hit.point);
                }
            }
        }
    }
}
