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
        // get the applicable result
        Dialogue result = GetResult();

        // if success, increment scenario
        if (result.tags.Contains("FULL_SUCCESS") || result.tags.Contains("PARTIAL_SUCCESS")) {
            data.nextScenarioIndex += 1;
        }

        // set dialogue based on selections
        SetResultsDescription(result.text);
        // set next NPC intro dialogues based on selections

        // handle rewards, if any

        // reset answers
        data.chosenAnswerKeys.Clear();
        // TODO reset answers at beginning of scenario results screen
        // use answers to get NPCs to respond
    }

    private Dialogue GetResult() {
        // add location to answers
        List<string> chosenAnswers = new List<string>(data.chosenAnswerKeys) {
            data.chosenLocation.ToString()
        };

        string resultsID = data.scenarioKey_resultsKey[data.scenario.id];

        return GetResult(data.key_results[resultsID].nextDialogueKey, chosenAnswers);
    }

    private Dialogue GetResult(string dialogueKey, List<string> chosenAnswers) {
        Dialogue dialogue = data.key_results[dialogueKey];

        // if this dialogue is the last in line, return it
        if (dialogue.nextDialogueKey == null) {
            return dialogue;
        } else {
            // if all unlocks appear in the answer, return the dialogue
            foreach (string cardKey in chosenAnswers) {
                // if answer does NOT appear in the dialogue card list, go to the next dialogue
                if (dialogue.unlockCardKeys.Contains(cardKey) == false) {
                    return GetResult(dialogue.nextDialogueKey, chosenAnswers);
                }
            }
            // if all answers were in the dialogue, return the dialogue
            return dialogue;
        }
    }

    private void SetResultsDescription(string result) {
        data.resultsDialogue = result;
        //// parse the reward
        //int endIndex = result.description.IndexOf('>');
        //string reward = result.description.Substring(1, endIndex - 1);
        //if (reward.Length > 0) {
        //    //data.npcsToIntroduce.Enqueue(reward);
        //    DeckController.Add(reward);
        //}
        //string dialogue = result.description.Substring(endIndex + 1, result.description.Length - endIndex - 1);
        //// save the results
        //data.resultsDialogue = dialogue;
    }

    // if ANY unlock key does not exist in the unlocked keys, returns false. Otherwise, returns true
    // ASSUMES results are in order from most specific to least specific
    // e.g. ITEM_HORSE, NPC_STABLEBOY result is compared before ITEM_HORSE result
    // if above example was reversed, former result would never appear, as latter result would always trigger first
    private bool IsUnlocked(ScenarioResult result, List<string> chosenAnswers) {
        // if result is the default result, always return true
        if (result.unlocks[0] == GameData.SCENARIO_RESULT_DEFAULT) {
            return true;
        }
        
        foreach (string unlockKey in result.unlocks) {
            if (chosenAnswers.Contains(unlockKey) == false) {
                return false;
            }
        }

        return true;
    }
}
