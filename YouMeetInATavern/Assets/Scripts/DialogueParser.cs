using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour {

    void Start() {
        Parse();
    }

    void Update() {

    }

    private void Parse() {
        TextAsset file = (TextAsset)Resources.Load("Dialogue");
        print(file);

        string[] lines = file.text.Split("\n"[0]);

        bool startParse = false;
        for (int i = 0; i < lines.Length; i++) {
            if (startParse && lines[i].Length > 0) {
                ParseLine(lines[i]);
            // don't start parsing until we've reached the NPC table
            } else if (lines[i].StartsWith("NPC,Q")) {
                startParse = true;
            }
        }
    }

    private void ParseLine(string line) {
        string[] data = line.Split(',');

        uint npcID = uint.Parse(data[0]);
        bool isQuestion = int.Parse(data[1]) == 0 ? false : true;
        uint dialogueID = uint.Parse(data[2]);

        string dialogue = "";
        for (int i = 3; i < data.Length; i++) {
            dialogue += data[i];
        }
        dialogue = dialogue.Replace("\"", "");


        //Debug.LogFormat("NPC={0}, Q={1}, ID={2}, \"{3}\"", npcID, isQuestion, dialogueID, dialogue);
    }
}
