using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioParser : MonoBehaviour {

    void Start() {
        Parse(FindObjectOfType<GameData>());
    }

    public void Parse(GameData data) {
        TextAsset file = (TextAsset)Resources.Load("Scenarios");
        string[] lines = file.text.Split('\n');

        data.scenarios = new Scenario[lines.Length];

        bool startParse = false;
        for (int i = 0; i < lines.Length; i++) {
            if (startParse && lines[i].Length > 0) {
                ParseLine(data, lines[i]);
            // don't start parsing until we've reached the NPC table
            } else if (lines[i].StartsWith("ID,Name")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(GameData data, string line) {
        string[] lineData = line.Split(',');

        Scenario scenario = new Scenario();

        int index = 0;
        scenario.id = uint.Parse(lineData[index++]);
        scenario.name = lineData[index++];
        scenario.order = uint.Parse(lineData[index++]);
        scenario.openHours = int.Parse(lineData[index++]);

        // parse NPCs to introduce
        scenario.npcs = new List<string>();
        foreach (string s in lineData[index++].Split(GameData.PARSER_DELIMITER)) {
            scenario.npcs.Add(s.Trim());
        }

        // save scenario to game data
        data.scenarios[scenario.order] = scenario;
    }
}
