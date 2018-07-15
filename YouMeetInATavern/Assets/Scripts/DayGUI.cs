using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayGUI : MonoBehaviour {

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
    }

    void OnEnable() {
        print("Enabling DayGUI");
        string toDisplay = "Day " + data.dayCount;
        GetComponentInChildren<Text>().text = toDisplay;
    }
}
