using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour {

    private GameData data;

    private GUIController guiController;
    private MusicController musicController;

    void Start() {
        data = FindObjectOfType<GameData>();
        guiController = FindObjectOfType<GUIController>();
        musicController = FindObjectOfType<MusicController>();
    }

    void OnEnable() {
        NPCController.npcEnteredTavernEventHandler += AddNPC;
        InputController.npcLeftTavernEventHandler += RemoveNPC;
        InputController.startDayEventHandler += NewDay;
        InputController.endDayEventHandler += EndDay;
    }

    void OnDisable() {
        NPCController.npcEnteredTavernEventHandler -= AddNPC;
        InputController.npcLeftTavernEventHandler -= RemoveNPC;
        InputController.startDayEventHandler -= NewDay;
        InputController.endDayEventHandler -= EndDay;
    }

    private void AddNPC(GameObject card) {

    }

    private void RemoveNPC(GameObject card) {
        if (data.npcsInTavern.Count == 0) {
            guiController.EndDay();
            musicController.TransitionSilent(data.fadeOutTime);
        }
    }

    private void NewDay() {
        data.dayCount += 1;
    }

    private void EndDay() {
        InputController.ConcludeScenario();
        
        // End scenario after data.daycount number of days
        //if (data.scenario.endsOnDay == data.dayCount) {
        //    InputController.ConcludeScenario();
        //} else {
        //    InputController.StartDay();
        //}
    }
}
