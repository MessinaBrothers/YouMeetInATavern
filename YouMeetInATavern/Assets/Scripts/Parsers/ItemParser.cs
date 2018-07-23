using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset file = (TextAsset)Resources.Load("Items");

        string[] lines = file.text.Split("\n"[0]);

        bool startParse = false;
        for (int i = 0; i < lines.Length; i++) {
            if (startParse && lines[i].Length > 0) {
                ParseLine(lines[i]);
                // don't start parsing until we've reached the NPC table
            } else if (lines[i].StartsWith("Name,Image")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(string line) {
        string[] lineData = line.Trim().Split(',');

        ItemData item = new ItemData();

        int index = 0;
        item.name = lineData[index++];
        item.imageFile = lineData[index++];

        item.sfxOnClicks = new string[2];
        item.sfxOnClicks[0] = lineData[index++];
        item.sfxOnClicks[1] = lineData[index++];

        // save npc data to game data
        data.itemData.Add(item.name, item);
    }
}
