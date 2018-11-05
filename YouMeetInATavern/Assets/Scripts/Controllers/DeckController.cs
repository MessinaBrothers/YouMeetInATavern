using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {

    private static GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void OnEnable() {
        InputController.gameflowEndBeginDay += ResetDeck;
    }

    void OnDisable() {
        InputController.gameflowEndBeginDay += ResetDeck;
    }

    private void ResetDeck() {
        if (data.dayCount == 0) data.unlockedDialogueKeys.Clear();
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
