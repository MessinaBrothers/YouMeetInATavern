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
        uint day = 1;
        for (int i = index; i < lineData.Length; i++) {
            if (lineData[i].Trim().Length == 0) continue;
            
            // get existing list of NPCs to introduce (or create one)
            List<string> npcsToIntroduce;
            if (scenario.day_introductions.ContainsKey(day)) {
                npcsToIntroduce = scenario.day_introductions[day];
            } else {
                npcsToIntroduce = new List<string>();
                scenario.day_introductions.Add(day, npcsToIntroduce);
            }

            // split the NPC IDs
            string[] npcIDs = lineData[i].Split('-');

            // for each NPC id, add it to the list of NPCs for the day
            for (int j = 0; j < npcIDs.Length; j++) {
                npcsToIntroduce.Add(npcIDs[j]);
            }

            // increment the day
            day += 1;
        }

        // save scenario to game data
        data.scenarios.Add(scenario.id, scenario);
    }
}
