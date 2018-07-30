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
        data.unlockedDialogueKeys.Add(data.chosenLocation.ToString());

        // get the applicable result
        ScenarioResult result = GetResult();

        // if CORRECT, increment scenario
        if (result.nextDialoguesKey.Contains("_CORRECT")) {
            data.nextScenarioIndex += 1;
        }

        // set dialogue based on selections
        SetResultsDescription(result);
        // set next NPC intro dialogues based on selections
        data.nextDialogueIntroKey = result.nextDialoguesKey;
        // handle rewards, if any

    }

    private ScenarioResult GetResult() {
        // iterate through results list
        for (int i = 0; i < data.scenarioResultsData[data.scenario.id].Count; i++) {
            ScenarioResult result = data.scenarioResultsData[data.scenario.id][i];
            
            if (IsUnlocked(result) == true) {
                // result is unlocked!
                return result;
            }
        }

        return null;
    }

    private void SetResultsDescription(ScenarioResult result) {
        // parse the reward
        int endIndex = result.description.IndexOf('>');
        string reward = result.description.Substring(1, endIndex - 1);
        if (reward.Length > 0) {
            data.npcsToIntroduce.Enqueue(reward);
        }
        string dialogue = result.description.Substring(endIndex + 1, result.description.Length - endIndex - 1);
        // save the results
        data.resultsDialogue = dialogue;
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
