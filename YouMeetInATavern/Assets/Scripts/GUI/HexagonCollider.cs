using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCollider : MonoBehaviour {

    public int id;

    public float lowerPosition, risePosition;
    public float lowerScale, riseScale;

    private Vector3 position;
    private Vector3 scale;

    void Start() {
        position = transform.localPosition;
        scale = transform.localScale;
    }

    void OnMouseEnter() {
        InputController.HoverOverHex(id);
        
        Set(risePosition, riseScale);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            InputController.ClickHex(id);
        }
    }

    void OnMouseExit() {
        InputController.HoverExitHex(id);

        Set(lowerPosition, lowerScale);
    }

    void Set(float positionY, float scaleZ) {
        position.y = positionY;
        transform.localPosition = position;

        scale.z = scaleZ;
        transform.localScale = scale;
    }
}
