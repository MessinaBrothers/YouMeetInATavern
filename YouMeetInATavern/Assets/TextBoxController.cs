using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour {

    public GameObject textPanel;
    public Text text;

    void Start() {
        textPanel.SetActive(false);
    }

    void Update() {

    }

    void OnEnable() {
        PlayerInput.playerInspectEventHandler += DisplayInfo;
    }

    void OnDisable() {
        PlayerInput.playerInspectEventHandler -= DisplayInfo;
    }

    private void DisplayInfo(GameObject go) {
        Inspectable inspectable = go.GetComponent<Inspectable>();
        if (inspectable != null) {
            textPanel.SetActive(true);
            text.text = inspectable.description;
        }
    }
}
