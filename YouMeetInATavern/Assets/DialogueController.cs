using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    void Start() {
        ParseCSV("Dialogue");
    }

    void Update() {

    }

    private void ParseCSV(string fileName) {
        TextAsset textFile = (TextAsset)Resources.Load(fileName, typeof(TextAsset));

        string[] textLines = textFile.text.Split('\n');
        for (int i = 1; i < textLines.Length - 1; i++) {
            print(textLines[i]);
            string[] textData = textLines[i].Split(',');
        }
    }
}
