using System.Collections;
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
        NPCController.npcCreatedEventHandler += AddNPC;
        NPCController.npcRemovedEventHandler += RemoveNPC;
        InputController.startDayEventHandler += NewDay;
        InputController.endDayEventHandler += EndDay;
    }

    void OnDisable() {
        NPCController.npcCreatedEventHandler -= AddNPC;
        NPCController.npcRemovedEventHandler -= RemoveNPC;
        InputController.startDayEventHandler -= NewDay;
        InputController.endDayEventHandler -= EndDay;
    }

    private void AddNPC(GameObject card) {

    }

    private void RemoveNPC(GameObject card) {
        if (data.npcs.Count == 0) {
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
