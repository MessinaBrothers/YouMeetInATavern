using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrigger : MonoBehaviour {

    public GameObject[] toEnable;

    void OnEnable() {
        TavernDoor.leaveBarEventHandler += LeaveTavern;
    }

    void OnDisable() {
        TavernDoor.leaveBarEventHandler -= LeaveTavern;
    }

    private void LeaveTavern() {
        foreach (GameObject go in toEnable) {
            go.SetActive(true);
        }
    }
}
