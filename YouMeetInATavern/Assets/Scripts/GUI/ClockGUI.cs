using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockGUI : MonoBehaviour {

    private GameData data;

    private Text text;

    void Awake() {
        data = FindObjectOfType<GameData>();
        text = GetComponent<Text>();
    }

    public void UpdateText(int currentHour) {
        int hoursRemaining = data.tavernCloseHour - currentHour;
        
        string s = "";
        switch (hoursRemaining) {
            case 0:
                s = "Closing tavern...";
                break;
            case 1:
                s = "1 hour remaining.";
                break;
            default:
                s = hoursRemaining + " hours remaining.";
                break;
        }

        text.text = s;
    }
}
