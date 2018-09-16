using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour {

    public GameObject model;

    public float riseSpeed, lowerSpeed;
    public float lowerY, riseY;

    private Vector3 position;
    private int id;
    private bool isFloating, isChosen;
    private float currentY;

    void Start() {
        id = GetComponentInChildren<HexagonCollider>().id;
        position = model.transform.localPosition;
        currentY = position.y;
    }

    void Update() {
        // increase or decrease Y
        if (isFloating || isChosen) {
            currentY += riseSpeed * Time.deltaTime;
        } else {
            currentY -= lowerSpeed * Time.deltaTime;
        }
        currentY = Mathf.Clamp(currentY, lowerY, riseY);

        // set position
        position.y = currentY;
        model.transform.localPosition = position;
    }

    void OnEnable() {
        InputController.hoverOverHexEventHandler += RiseUp;
        InputController.hoverExitHexEventHandler += Lower;
        InputController.hexClickedEventHandler += Select;
    }

    void OnDisable() {
        InputController.hoverOverHexEventHandler -= RiseUp;
        InputController.hoverExitHexEventHandler -= Lower;
        InputController.hexClickedEventHandler -= Select;
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

    private void Select(int id) {
        isChosen = (this.id == id);
    }
}
