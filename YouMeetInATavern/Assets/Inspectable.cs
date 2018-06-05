using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : MonoBehaviour {

    public string description;

    void Start() {

    }

    void Update() {

    }

    void OnEnable() {
        PlayerInput.playerInspectEventHandler += Inspect;
    }

    void OnDisable() {
        PlayerInput.playerInspectEventHandler -= Inspect;
    }

    private void Inspect(GameObject inspectable) {
        if (inspectable == this) {
            print(description);
        }
    }
}
