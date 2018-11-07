using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPuffController : MonoBehaviour {

    public GameObject puffPrefab;

    void OnEnable() {
        InputController.conclusionBackgroundClicked += HandleConclusionBackgroundClick;
    }

    void OnDisable() {
        InputController.conclusionBackgroundClicked -= HandleConclusionBackgroundClick;
    }

    private void HandleConclusionBackgroundClick(Vector3 position) {
        GameObject puff = Instantiate(puffPrefab, position, Quaternion.identity, transform);
        puff.transform.LookAt(Camera.main.transform);
    }
}
