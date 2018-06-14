using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : DataUser {

    void Start() {
        ParseCSV("Dialogue");
    }

    void Update() {

    }

    private void ParseCSV(string fileName) {
        TextAsset textFile = (TextAsset)Resources.Load(fileName, typeof(TextAsset));

        string[] textLines = textFile.text.Split('\n');
        for (int i = 1; i < textLines.Length - 1; i++) {
            string[] textData = textLines[i].Split(',');
            //print(textLines[i]);
            //for (int j = 0; j < textData.Length; j++) print(textData[j]);

            int index = 0;
            uint id = uint.Parse(textData[index++]);
            Dialogue.Type type = textData[index++].Equals("Prompt") ? Dialogue.Type.PROMPT : Dialogue.Type.CHOICE;
            string text = textData[index++];
            // account for commas in text dialogue e.g. "Guards, help!"
            if (textData[index].Contains("\"")) text += ',' + textData[index++];
            uint next0 = textData[index].Length == 0 ? GameData.INVALID_UID : uint.Parse(textData[index++]);
            uint next1 = textData[index].Length == 0 ? GameData.INVALID_UID : uint.Parse(textData[index++]);
            uint next2 = textData[index].Length == 0 ? GameData.INVALID_UID : uint.Parse(textData[index++]);
            string reward = textData[index++];

            Dialogue dialogue = new Dialogue(id, type, text, next0, next1, next2, reward);
            print(dialogue);
            data.dialogues.Add(id, dialogue);
        }
    }
}
