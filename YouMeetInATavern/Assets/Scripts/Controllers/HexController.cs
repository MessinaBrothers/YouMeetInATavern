using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour {

    public GameObject model;

    public float lowerY, riseY;

    private Vector3 position;
    private int id;
    private bool isFloating;

    void Start() {
        id = GetComponentInChildren<HexagonCollider>().id;
        position = model.transform.localPosition;
    }

    void Update() {
        position.y = isFloating ? riseY : lowerY;
        model.transform.localPosition = position;
    }

    void OnEnable() {
        InputController.hoverOverHexEventHandler += RiseUp;
        InputController.hoverExitHexEventHandler += Lower;
    }

    void OnDisable() {
        InputController.hoverOverHexEventHandler -= RiseUp;
        InputController.hoverExitHexEventHandler -= Lower;
    }

    private void RiseUp(int id) {
        if (this.id == id) {
            isFloating = true;
        }
    }

    private void Lower(int id) {
        if (this.id == id) {
            isFloating = false;
        }
    }
}
