using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexesController : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.hexClickedEventHandler += SetLocation;
    }

    void OnDisable() {
        InputController.hexClickedEventHandler -= SetLocation;
    }

    private void SetLocation(int id) {
        data.chosenLocation = data.hexIndex_location[id];
    }
}
