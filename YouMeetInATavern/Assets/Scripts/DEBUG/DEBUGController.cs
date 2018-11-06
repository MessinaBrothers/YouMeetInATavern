﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DEBUGController : MonoBehaviour {

    public bool UNLOCK_ALL_KEYS;
    public bool START_CHOOSE_SCENARIO;

    public GameObject debugPanel, debugUnlockCardsPanel;

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
    }

    void Start() {
        print("PRESS F1 FOR DEBUG COMMANDS");
        
        string s = "***** DEBUG COMMANDS *****";
        s += "\nF1: Debug panel";
        s += "\nF2: Conclude scenario";
        s += "\nF3: Deck screen";
        s += "\nF4: Toggle mic";
        s += "\nF5: Restart game";
        s += "\nF6: Unlock cards";
        debugPanel.GetComponentInChildren<Text>().text = s;

        debugUnlockCardsPanel.GetComponentInChildren<DEBUGUnlockCardsGUI>().Load(data);

        debugPanel.SetActive(false);
        debugUnlockCardsPanel.SetActive(false);
    }

    void Update() {
        if (UNLOCK_ALL_KEYS) {
            UNLOCK_ALL_KEYS = false;
            UnlockAllKeys();
        }
        if (START_CHOOSE_SCENARIO) {
            START_CHOOSE_SCENARIO = false;
            ConcludeScenario();
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            ConcludeScenario();
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            InputController.DeckClick();
        }
        if (Input.GetKeyDown(KeyCode.F4)) {
            // Leave blank (F4 is for muting mic!)
        }
        if (Input.GetKeyDown(KeyCode.F5)) {
            SceneManager.LoadScene("Main");
        }
        if (Input.GetKeyDown(KeyCode.F6)) {
            debugUnlockCardsPanel.SetActive(!debugUnlockCardsPanel.activeSelf);
        }
    }

    private void ConcludeScenario() {
        data.npcsInTavern.Clear();
        data.npcsToIntroduce.Clear();
        InputController.StartFinishTavern();
        InputController.EndFinishTavern();
        InputController.ConcludeScenario();
    }

    private void UnlockAllKeys() {
        foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
            DeckController.Add(kvp.Key);
        }
    }

    public void UnlockScenarioKeys() {
        // unlock the cards used to beat the scenario
        uint scenarioID = data.scenario.id;
        ScenarioResult result = data.scenarioResultsData[scenarioID][0];
        foreach (string key in result.unlocks) {
            if (key.StartsWith("LOCATION_") == false) {
                DeckController.Add(key);
            }
        }
    }
}
