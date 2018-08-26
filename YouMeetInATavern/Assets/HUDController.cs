using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    public GameObject deck;

    void OnEnable() {
        InputController.deckClickedEventHander += HideHUD;
        InputController.deckClosedEventHander += ShowHUD;
    }

    void OnDisable() {
        InputController.deckClickedEventHander -= HideHUD;
        InputController.deckClosedEventHander -= ShowHUD;
    }

    private void HideHUD() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }

    private void ShowHUD() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }
}
