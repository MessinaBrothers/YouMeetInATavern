using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPuffController : MonoBehaviour {

    public GameObject puffPrefab;

    void OnEnable() {
        InputController.conclusionBackgroundClicked += CreatePuff;
        InputController.tavernClicked += CreatePuff;
    }

    void OnDisable() {
        InputController.conclusionBackgroundClicked -= CreatePuff;
        InputController.tavernClicked -= CreatePuff;
    }

    private void CreatePuff(Vector3 position) {
        GameObject puff = Instantiate(puffPrefab, position, Quaternion.identity, transform);
        puff.transform.LookAt(Camera.main.transform);
    }
}
