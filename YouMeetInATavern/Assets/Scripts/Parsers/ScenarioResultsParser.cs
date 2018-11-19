using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioResultsParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset file = (TextAsset)Resources.Load("Scenario Results");
        string[] lines = file.text.Split('\n');

        bool startParse = false;
        for (int i = 0; i < lines.Length; i++) {
            if (startParse && lines[i].Length > 0) {
                ParseLine(lines[i]);
                // don't start parsing until we've reached the NPC table
            } else if (lines[i].StartsWith("ID,Location")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(string line) {
        string[] lineData = line.Split(',');

        ScenarioResult scenarioResult = new ScenarioResult();

        int index = 0;
        scenarioResult.scenarioKey = lineData[index++];

        // parse unlocks
        //string unlocks = lineData[index++];
        //scenarioResult.unlocks = new List<string>(unlocks.Split(GameData.PARSER_DELIMITER));
        List<string> unlocks = new List<string>();
        for (int i = 0; i < 5; i++) {
            if (lineData[index] != "") unlocks.Add(lineData[index]);
            index += 1;
        }
        scenarioResult.unlocks = unlocks;

        // parse next dialogues
        scenarioResult.nextDialoguesKey = lineData[index++];

        // parse description
        string description = "";
        for (int i = index; i < lineData.Length; i++) {
            description += lineData[i] + ",";
        }
        description = description.Replace("\"", "");
        description = description.Substring(0, description.Length - ",".Length);
        scenarioResult.description = description;

        // save results to game data
        List<ScenarioResult> results;
        if (data.scenarioKey_scenarioResult.ContainsKey(scenarioResult.scenarioKey)) {
            results = data.scenarioKey_scenarioResult[scenarioResult.scenarioKey];
        } else {
            results = new List<ScenarioResult>();
            data.scenarioKey_scenarioResult.Add(scenarioResult.scenarioKey, results);
        }
        results.Add(scenarioResult);

        //Debug.LogFormat("Saved scenario results with key [{0}], unlocks of [{1}], and description of [{2}]", scenario.scenarioKey, unlocks, scenario.description);
    }
}
