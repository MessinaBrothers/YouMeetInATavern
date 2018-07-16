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
    }

    void OnDisable() {
        NPCController.npcCreatedEventHandler -= AddNPC;
        NPCController.npcRemovedEventHandler -= RemoveNPC;
        InputController.startDayEventHandler -= NewDay;
    }

    private void AddNPC(GameObject card) {

    }

    private void RemoveNPC(GameObject card) {
        if (data.npcs.Count == 0) {
            guiController.EndDay();
        }
    }

    private void NewDay() {
        data.dayCount += 1;
    }
}
