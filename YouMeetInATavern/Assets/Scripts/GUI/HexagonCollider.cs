using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCollider : MonoBehaviour {

    public int id;

    void OnMouseEnter() {
        InputController.HoverOverHex(id);
    }

    void OnMouseExit() {
        InputController.HoverExitHex(id);
    }
}
