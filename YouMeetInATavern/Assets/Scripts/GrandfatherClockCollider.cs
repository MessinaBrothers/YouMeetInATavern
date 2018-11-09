using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrandfatherClockCollider : MonoBehaviour {

    void Update() {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // =~ inverts a binary number
            int layerMask = ~LayerMask.GetMask("ClickBlocker");

            if (Physics.Raycast(ray, out hit, 10000f, layerMask)) {
                if (hit.collider.gameObject == gameObject) {
                    InputController.ClickItem(hit.collider.gameObject, hit.point);
                }
            }
        }
    }
}
