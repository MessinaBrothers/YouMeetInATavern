using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    public GameObject deck;

    // when hiding the HUD, some children may be inactive
    // when showing the HUD, we shouldn't activate children previously that were inactive
    // therefore, we have to track if children were active previous to hiding them
    private GameObject[] children;
    private bool[] isChildActive;

    void Awake() {
        children = new GameObject[transform.childCount];
        isChildActive = new bool[transform.childCount];

        for (int i = 0; i < children.Length; i++) {
            children[i] = transform.GetChild(i).gameObject;
            isChildActive[i] = true;
        }
    }

    void OnEnable() {
        InputController.deckClickedEventHander += HideHUD;
        InputController.deckClosedEventHander += ShowHUD;
    }

    void OnDisable() {
        InputController.deckClickedEventHander -= HideHUD;
        InputController.deckClosedEventHander -= ShowHUD;
    }

    private void HideHUD() {
        for (int i = 0; i < children.Length; i++) {
            isChildActive[i] = children[i].activeSelf;
            children[i].SetActive(false);
        }
    }

    private void ShowHUD() {
        for (int i = 0; i < children.Length; i++) {
            if (isChildActive[i] == true) {
                children[i].SetActive(true);
            }
        }
    }
}
