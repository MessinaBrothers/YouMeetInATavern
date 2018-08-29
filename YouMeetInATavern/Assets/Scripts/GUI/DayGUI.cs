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
        //uint daysLeft = data.scenario.endsOnDay - (data.dayCount - 1);
        //string toDisplay = string.Format("{0} Day{1} Left", daysLeft, daysLeft == 1 ? "" : "s");

        string toDisplay = string.Format("Day {0}", data.dayCount);

        GetComponentInChildren<Text>().text = toDisplay;
    }
}
