using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset file = (TextAsset)Resources.Load("Scenarios");
        string[] lines = file.text.Split('\n');

        bool startParse = false;
        for (int i = 0; i < lines.Length; i++) {
            if (startParse && lines[i].Length > 0) {
                ParseLine(lines[i]);
            // don't start parsing until we've reached the NPC table
            } else if (lines[i].StartsWith("ID,Name")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(string line) {
        string[] lineData = line.Split(',');

        Scenario scenario = new Scenario();

        int index = 0;
        scenario.id = uint.Parse(lineData[index++]);
        scenario.name = lineData[index++];
        scenario.endsOnDay = uint.Parse(lineData[index++]);

        // parse NPCs to introduce
        scenario.npcs = new List<string>();
        foreach (string s in lineData[index++].Split(GameData.PARSER_DELIMITER)) {
            scenario.npcs.Add(s.Trim());
        }
        
        // save scenario to game data
        data.scenarios.Add(scenario.id, scenario);
    }
}
