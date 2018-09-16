using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour {

    private int id;

    public bool isFloating;

    void Start() {
        id = GetComponentInChildren<HexagonCollider>().id;
    }

    void Update() {

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
