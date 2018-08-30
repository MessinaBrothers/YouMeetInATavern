using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {

    void OnEnable() {
        InputController.newScenarioStartedEventHandler += ResetDeck;
    }

    void OnDisable() {
        InputController.newScenarioStartedEventHandler += ResetDeck;
    }

    private void ResetDeck(GameData data) {
        data.unlockedDialogueKeys.Clear();
    }
}
