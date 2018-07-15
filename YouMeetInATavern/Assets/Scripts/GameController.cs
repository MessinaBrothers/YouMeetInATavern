using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        InputController.StartDay();
    }

    void Update() {

    }
}
