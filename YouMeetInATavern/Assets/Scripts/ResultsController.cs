using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsController : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void OnEnable() {
        InputController.confirmScenarioChoicesEventHandler += DisplayResults;
    }

    void OnDisable() {
        InputController.confirmScenarioChoicesEventHandler -= DisplayResults;
    }

    private void DisplayResults() {
        // add location to unlocks (HACK)
        data.unlockedDialogueKeys.Add("LOCATION_" + data.chosenLocation);
        // set dialogue based on selections
        SetResultsDescription();
        // handle rewards, if any
    }

    private void SetResultsDescription() {
        // iterate through results list
        for (int i = 0; i < data.scenarioResultsData[data.scenario.id].Count; i++) {
            ScenarioResult result = data.scenarioResultsData[data.scenario.id][i];

            bool isUnlocked = IsUnlocked(result);

            if (isUnlocked) {
                // result is unlocked!
                // save the results
                data.resultsDialogue = result.description;
                return;
            }

        }

    }

    // if ANY unlock key does not exist in the unlocked keys, returns false. Otherwise, returns true
    // ASSUMES results are in order from most specific to least specific
    // e.g. ITEM_HORSE, NPC_STABLEBOY result is compared before ITEM_HORSE result
    // if above example was reversed, former result would never appear, as latter result would always trigger first
    private bool IsUnlocked(ScenarioResult result) {
        if (result.unlocks[0] != GameData.DIALOGUE_DEFAULT) {
            foreach (string unlock in result.unlocks) {
                if (data.unlockedDialogueKeys.Contains(unlock) == false) {
                    print("Unlock key of " + unlock + " does not exist!");
                    return false;
                }
            }
        }
        return true;
    }
}
