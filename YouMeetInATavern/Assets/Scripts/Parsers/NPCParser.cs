using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset file = (TextAsset)Resources.Load("NPCs");

        string[] lines = file.text.Split("\n"[0]);

        bool startParse = false;
        for (int i = 0; i < lines.Length; i++) {
            if (startParse) {
                // stop parsing if no data found on a line e.g. if data table is split
                if (lines[i].StartsWith("NPC_")) ParseLine(lines[i]);
            // don't start parsing until we've reached the NPC table
            } else if (lines[i].StartsWith("Unlock Tag,Name")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(string line) {
        string[] lineData = line.Trim().Split(',');

        CardData npc = new CardData();

        int index = 0;
        npc.unlockTag = lineData[index++];
        npc.name = lineData[index++];
        npc.imageFile = lineData[index++];
        npc.sfxIntro = lineData[index++];

        npc.sfxGoodbyes = new string[1];
        npc.sfxGoodbyes[0] = lineData[index++];

        npc.sfxOnClicks = new string[2];
        npc.sfxOnClicks[0] = lineData[index++];
        npc.sfxOnClicks[1] = lineData[index++];

        npc.sfxGreetings = new string[2];
        npc.sfxGreetings[0] = lineData[index++];
        npc.sfxGreetings[1] = lineData[index++];

        // save npc data to game data
        data.cardData.Add(npc.unlockTag, npc);
    }
}
