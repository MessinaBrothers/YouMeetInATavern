﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset file = (TextAsset)Resources.Load("Dialogue");

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
        string dialogueID = data[2];

        string dialogue = "";
        for (int i = 3; i < data.Length; i++) {
            dialogue += data[i];
        }
        dialogue = dialogue.Replace("\"", "");

        //Debug.LogFormat("NPC={0}, Q={1}, ID={2}, \"{3}\"", npcID, isQuestion, dialogueID, dialogue);
        if (isQuestion == true) {
            SaveQuestion(npcID, dialogueID, dialogue);
        } else {
            SaveDialogue(npcID, dialogueID, dialogue);
        }
    }

    private void SaveQuestion(uint npcID, string prereqID, string text) {
        // get the npc's list of questions
        Dictionary<string, string> questions = GetList(npcID, data.npc_questions);
        questions.Add(prereqID, text);
    }

    private void SaveDialogue(uint npcID, string dialogueID, string text) {
        // get the npc's list of dialogues
        Dictionary<string, string> dialogues = GetList(npcID, data.npc_dialogues);
        dialogues.Add(dialogueID, text);
    }

    private Dictionary<string, string> GetList(uint id, Dictionary<uint, Dictionary<string, string>> listOfLists) {
        Dictionary<string, string> list;
        if (listOfLists.ContainsKey(id) == true) {
            // retrieve the existing list
            list = listOfLists[id];
        } else {
            // create a new list
            list = new Dictionary<string, string>();
            // add it to the list of lists
            listOfLists.Add(id, list);
        }
        return list;
    }
}
