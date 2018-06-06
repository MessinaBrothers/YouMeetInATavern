using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour {

    public float textSpeed;

    public GameObject textPanel;
    public Text text;

    // animations must run even when game is paused, so we avoid Time.deltaTime
    private float lastRealTimeSinceStartup;
    private string stringToDisplay, currentDisplay;
    private float displayCharTime, displayCharTimer;

    void Start() {
        textPanel.SetActive(false);
        stringToDisplay = "";
        currentDisplay = "";
    }

    void Update() {
        if (stringToDisplay.Length > 0) {
            float deltaTime = Time.realtimeSinceStartup - lastRealTimeSinceStartup;
            displayCharTimer += deltaTime;
            while (displayCharTimer >= displayCharTime && stringToDisplay.Length > 0) {
                displayCharTimer -= displayCharTime;
                currentDisplay += stringToDisplay[0];
                stringToDisplay = stringToDisplay.Remove(0, 1);
                text.text = currentDisplay;
            }
        }
        lastRealTimeSinceStartup = Time.realtimeSinceStartup;
    }

    void OnEnable() {
        PlayerInput.playerInspectEventHandler += DisplayInfo;
        PlayerInput.textContinueEventHandler += Continue;
    }

    void OnDisable() {
        PlayerInput.playerInspectEventHandler -= DisplayInfo;
        PlayerInput.textContinueEventHandler -= Continue;
    }

    private void Continue() {
        textPanel.SetActive(false);
        stringToDisplay = "";
    }

    private void DisplayInfo(GameObject go) {
        Inspectable inspectable = go.GetComponent<Inspectable>();
        if (inspectable != null) {
            // display the panel
            textPanel.SetActive(true);

            // initiate displaying the text
            stringToDisplay = inspectable.description;
            displayCharTimer = 0;
            displayCharTime = 1 / textSpeed;

            // clear current strings
            currentDisplay = "";
            text.text = "";
        }
    }
}
