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
            } else if (lines[i].StartsWith("ID,Unlocks")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(string line) {
        string[] lineData = line.Split(',');

        ScenarioResult scenario = new ScenarioResult();

        int index = 0;
        scenario.scenarioKey = uint.Parse(lineData[index++]);

        // parse unlocks
        string unlocks = lineData[index++];
        scenario.unlocks = new List<string>(unlocks.Split(GameData.PARSER_DELIMITER));

        // parse next dialogues
        scenario.nextDialoguesKey = lineData[index++];

        // parse description
        string description = "";
        for (int i = index; i < lineData.Length; i++) {
            description += lineData[i] + ",";
        }
        description = description.Replace("\"", "");
        description = description.Substring(0, description.Length - ",".Length);
        scenario.description = description;

        // save results to game data
        List<ScenarioResult> results;
        if (data.scenarioResultsData.ContainsKey(scenario.scenarioKey)) {
            results = data.scenarioResultsData[scenario.scenarioKey];
        } else {
            results = new List<ScenarioResult>();
            data.scenarioResultsData.Add(scenario.scenarioKey, results);
        }
        results.Add(scenario);

        //Debug.LogFormat("Saved scenario results with key [{0}], unlocks of [{1}], and description of [{2}]", scenario.scenarioKey, unlocks, scenario.description);
    }
}
