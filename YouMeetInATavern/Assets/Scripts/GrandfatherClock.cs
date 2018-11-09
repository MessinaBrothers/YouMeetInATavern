using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandfatherClock : MonoBehaviour {

    public GameObject minuteHand, hourHand;

    public float degreesPerHour;

    public float midnightRotX, midnightRotY, midnightRotZ;

    public float secondsToUpdateOneHour;
    private float timer;

    private GameData data;
    private float oldHour, newHour;

    void Awake() {
        data = FindObjectOfType<GameData>();

        timer = secondsToUpdateOneHour;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UpdateClockHands((int)newHour + 1);
        }

        if (timer < secondsToUpdateOneHour) {
            timer += Time.deltaTime;
            //hourHand.transform.localRotation = Quaternion.Euler(
            //    midnightRotX + Mathf.Lerp(oldHour, newHour, timer / secondsToUpdateOneHour) * degreesPerHour,
            //    midnightRotY,
            //    midnightRotZ);
            SetClockHand(hourHand, Mathf.Lerp(oldHour, newHour, timer / secondsToUpdateOneHour) * degreesPerHour);
            SetClockHand(minuteHand, Mathf.Lerp(oldHour, newHour, timer / secondsToUpdateOneHour) * degreesPerHour * 12f);
            if (timer >= secondsToUpdateOneHour) oldHour = newHour;
        }
    }
    
    void OnEnable() {
        InputController.gameflowEndBeginTavern += SetClockHands;
        InputController.clockTickedEventHandler += UpdateClockHands;
    }

    void OnDisable() {
        InputController.gameflowEndBeginTavern -= SetClockHands;
        InputController.clockTickedEventHandler -= UpdateClockHands;
    }

    private void SetClockHands() {
        print("Setting clock hands");
        oldHour = data.currentHour;
        newHour = data.currentHour;
        SetClockHand(hourHand, newHour * degreesPerHour);
    }

    private void SetClockHand(GameObject go, float degreesX) {
        go.transform.localRotation = Quaternion.Euler(midnightRotX + degreesX, midnightRotY, midnightRotZ);
    }

    private void UpdateClockHands(int currentHour) {
        oldHour += (newHour - oldHour) * timer / secondsToUpdateOneHour;
        UpdateHourHand(currentHour);
    }

    private void UpdateHourHand(float currentHour) {
        timer = 0;
        newHour = currentHour;
        //hourHand.transform.localRotation = Quaternion.Euler(midnightRotX + currentHour * degreesPerHour, midnightRotY, midnightRotZ);
    }
}
