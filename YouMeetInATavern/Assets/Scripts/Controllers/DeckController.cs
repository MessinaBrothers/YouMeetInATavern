using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {

    private static GameData data;

    void Start() {
        data = GameData.FindObjectOfType<GameData>();
    }

    void OnEnable() {
        InputController.newScenarioStartedEventHandler += ResetDeck;
    }

    void OnDisable() {
        InputController.newScenarioStartedEventHandler += ResetDeck;
    }

    private void ResetDeck(GameData data) {
        print("Resetting deck...");
        data.unlockedDialogueKeys.Clear();
    }

    public static void Add(string reward) {
        data.unlockedDialogueKeys.Add(reward);
    }

    public static bool Contains(string key) {
        return data.unlockedDialogueKeys.Contains(key);
    }

    public static int GetCount() {
        return data.unlockedDialogueKeys.Count;
    }
}
