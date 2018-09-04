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
        text.text = string.Format("{0} hour{1} until close", hoursRemaining, hoursRemaining == 1 ? "" : "s");
    }
}
