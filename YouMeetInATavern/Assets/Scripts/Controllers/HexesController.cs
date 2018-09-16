using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexesController : MonoBehaviour {

    public GameData.Location[] locations;

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
        data.chosenLocation = locations[id];
    }
}
