using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    void Start() {
        // always Starts last. See: Edit > Project Settings > Script Execution Order
        print("Game initialized. Loading scenario...");

        // load scenario
        GameData data = FindObjectOfType<GameData>();
        data.scenario = data.scenarios[0];
        
        data.nextDialogueIntroKey = GameData.DIALOGUE_INTRO;

        InputController.GameInitialized();

        // start the first day
        InputController.StartDay();
    }

    void Update() {

    }
}
