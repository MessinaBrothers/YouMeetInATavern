﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour {

    private GameData data;

    private GUIController guiController;

    void Start() {
        data = FindObjectOfType<GameData>();
        guiController = FindObjectOfType<GUIController>();
    }

    void OnEnable() {
        NPCController.npcEnteredTavernEventHandler += AddNPC;
        NPCController.npcRemovedEventHandler += RemoveNPC;
        InputController.startDayEventHandler += NewDay;
        InputController.endDayEventHandler += EndDay;
    }

    void OnDisable() {
        NPCController.npcEnteredTavernEventHandler -= AddNPC;
        NPCController.npcRemovedEventHandler -= RemoveNPC;
        InputController.startDayEventHandler -= NewDay;
        InputController.endDayEventHandler -= EndDay;
    }

    private void AddNPC(GameObject card) {

    }

    private void RemoveNPC(GameObject card) {
        if (data.npcsInTavern.Count == 0) {
            guiController.EndDay();
        }
    }

    private void NewDay() {
        data.gameMode = GameData.GameMode.INTRODUCE;
        data.dayCount += 1;
    }

    private void EndDay() {
        if (data.scenario.endsOnDay == data.dayCount) {
            InputController.ConcludeScenario();
        } else {
            InputController.StartDay();
        }

    }
}
