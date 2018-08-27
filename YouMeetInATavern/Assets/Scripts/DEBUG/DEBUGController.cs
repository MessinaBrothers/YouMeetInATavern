using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUGController : MonoBehaviour {

    public bool UNLOCK_ALL_KEYS;

    public GameObject debugPanel;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        print("PRESS F1 FOR DEBUG COMMANDS");
        
        string s = "***** DEBUG COMMANDS *****";
        s += "\nF1: show/hide debug panel";
        s += "\nF2: go to Conclude Scenario";
        s += "\nF3: show Deck screen";
        debugPanel.GetComponentInChildren<Text>().text = s;

        debugPanel.SetActive(false);
    }

    void Update() {
        if (UNLOCK_ALL_KEYS) {
            UNLOCK_ALL_KEYS = false;
            UnlockAllKeys();
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            InputController.ConcludeScenario();
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            InputController.ContinueDay();
            InputController.DeckClick();
        }
    }

    private void UnlockAllKeys() {
        foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
            data.unlockedDialogueKeys.Add(kvp.Key);
        }
    }
}
