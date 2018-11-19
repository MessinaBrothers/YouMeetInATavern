using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class DialogueParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset xmlRawFile = (TextAsset)Resources.Load("dialogue_xml");
        XDocument xmlDoc = XDocument.Load(new StringReader(xmlRawFile.text));

        string yedBullcrap = "{http://graphml.graphdrawing.org/xmlns}";

        XElement root = xmlDoc.Element(yedBullcrap + "graphml");
        XElement graph = root.Element(yedBullcrap + "graph");
        
        foreach (XElement graphNodes in graph.Elements(yedBullcrap + "node")) {
            // get the NPC key
            string npcKey = "";
            foreach (XElement xe in graphNodes.Elements(yedBullcrap + "data")) {
                if (npcKey.Length == 0 && xe.Value.StartsWith("nodetype=")) {
                    npcKey = xe.Value.Replace("nodetype=", "");
                }
            }

            // for each NPC
            XElement npcGraph = graphNodes.Element(yedBullcrap + "graph");
            foreach (XElement npcScenarioGraph in npcGraph.Elements(yedBullcrap + "node")) {
                // for each scenario in NPC
                XElement xe = npcScenarioGraph.Element(yedBullcrap + "graph");
                foreach (XElement npcScenarioDialogue in xe.Elements(yedBullcrap + "node")) {
                    // parse the dialogue ID
                    string dialogueKey = "";
                    dialogueKey = npcScenarioDialogue.Attribute("id").ToString().Replace("id=", "");

                    // parse the dialogue type and text
                    string dialogueText = "";
                    string dialogueType = "";
                    foreach (XElement xe1 in npcScenarioDialogue.Elements(yedBullcrap + "data")) {
                        if (xe1.Value.StartsWith("nodetype=")) {
                            dialogueType = xe1.Value.Replace("nodetype=", "");
                        } else {
                            dialogueText = xe1.Value;
                        }
                    }

                    // create the new Dialogue
                    Dialogue dialogue = new Dialogue(dialogueKey, dialogueText);

                    data.key_dialoguesNEW.Add(dialogueKey, dialogue);

                    // if intro or starting dialogue, add it to the respective lists
                    if (dialogueType.StartsWith("intro_")) {
                        data.npcKey_introKey.Add(npcKey + dialogueType.Replace("intro_", ""), dialogueKey);
                        dialogue.type = Dialogue.TYPE.INTRO;
                    } else if (dialogueType.StartsWith("scenario_")) {
                        data.npcKey_scenarioKey.Add(npcKey + dialogueType.Replace("scenario_", ""), dialogueKey);
                        dialogue.type = Dialogue.TYPE.START;
                    } else {
                        switch (dialogueType) {
                            case "player_response":
                                dialogue.type = Dialogue.TYPE.PLAYER_RESPONSE;
                                break;
                            case "npc_says":
                                dialogue.type = Dialogue.TYPE.NPC_SAYS;
                                break;
                            case "stop":
                                dialogue.type = Dialogue.TYPE.STOP;
                                break;
                            case "dialogue_text":
                                dialogue.type = Dialogue.TYPE.CLICKABLE_TEXT;
                                break;
                            case "dialogue_default":
                                dialogue.type = Dialogue.TYPE.NPC_SAYS;
                                data.npcKey_defaultDialogueKey.Add(npcKey, dialogueKey);
                                break;
                            case "card":
                                dialogue.type = Dialogue.TYPE.CARD;
                                break;
                            default:
                                print("ERROR: No such dialogue type exists: " + dialogueType);
                                break;
                        }
                    }
                }
            }
        }
        
        // parse dialogue edges
        foreach (XElement xe in xmlDoc.Descendants(yedBullcrap + "edge")) {
            // parse source and target
            string source = "";
            string target = "";
            foreach (XAttribute xa in xe.Attributes()) {
                if (xa.ToString().StartsWith("source=")) {
                    source = xa.ToString().Replace("source=", "");
                } else if (xa.ToString().StartsWith("target=")) {
                    target = xa.ToString().Replace("target=", "");
                } else {
                    //ignore id
                }
            }

            // save target in source, depending on target's type
            Dialogue sourceDialogue = data.key_dialoguesNEW[source];
            Dialogue targetDialogue = data.key_dialoguesNEW[target];
            switch (targetDialogue.type) {
                case Dialogue.TYPE.NPC_SAYS:
                    sourceDialogue.nextDialogueKey = target;
                    break;
                case Dialogue.TYPE.PLAYER_RESPONSE:
                    sourceDialogue.playerResponseKeys.Add(target);
                    break;
                case Dialogue.TYPE.STOP:
                    sourceDialogue.isEndOfConversation = true;
                    break;
                case Dialogue.TYPE.CLICKABLE_TEXT:
                    sourceDialogue.clickableDialogueKeys.Add(target);
                    break;
                case Dialogue.TYPE.CARD:
                    sourceDialogue.unlockCardKeys.Add(targetDialogue.text);
                    break;
            }
        }






        // OLD PARSER
        //TextAsset file = (TextAsset)Resources.Load("Dialogue");

        //string[] lines = file.text.Split("\n"[0]);

        //bool startParse = false;
        //for (int i = 0; i < lines.Length; i++) {
        //    if (startParse && lines[i].Length > 0) {
        //        ParseLine(lines[i]);
        //    // don't start parsing until we've reached the NPC table
        //    } else if (lines[i].StartsWith("NPC,Q")) {
        //        startParse = true;
        //    }
        //}
    }

    //private void ParseLine(string line) {
    //    string[] data = line.Split(',');
        
    //    string npcID = data[0];
    //    bool isQuestion = int.Parse(data[1]) == 0 ? false : true;
    //    string dialogueID = data[2];

    //    string dialogue = "";
    //    for (int i = 3; i < data.Length; i++) {
    //        dialogue += data[i] + ",";
    //    }
    //    dialogue = dialogue.Replace("\"", "");
    //    dialogue = dialogue.Substring(0, dialogue.Length - ",".Length);

    //    //Debug.LogFormat("NPC={0}, Q={1}, ID={2}, \"{3}\"", npcID, isQuestion, dialogueID, dialogue);
    //    if (isQuestion == true) {
    //        SaveQuestion(npcID, dialogueID, dialogue);
    //    } else {
    //        SaveDialogue(npcID, dialogueID, dialogue);
    //    }
    //}

    //private void SaveQuestion(string npcID, string prereqID, string text) {
    //    // get the npc's list of questions
    //    List<Question> questions = GetList(npcID, data.npc_questions);

    //    Question question = new Question();
    //    question.key = prereqID;
    //    question.text = text;
    //    question.isAskedByPlayer = false;

    //    questions.Add(question);

    //    if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_PARSER) {
    //        print(string.Format("Parsing question: NPC:{0}, prereqID:{1}, text:{2}", npcID, question.key, question.text));
    //    }
    //}

    //private void SaveDialogue(string npcID, string dialogueID, string text) {
    //    // get the npc's list of dialogues
    //    Dictionary<string, string> dialogues = GetList(npcID, data.npc_dialogues);
    //    dialogues.Add(dialogueID, text);

    //    if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_PARSER) {
    //        print(string.Format("Parsing dialogue: NPC:{0}, dialogueID:{1}, text:{2}", npcID, dialogueID, text));
    //    }
    //}

    //private Dictionary<string, string> GetList(string id, Dictionary<string, Dictionary<string, string>> listOfLists) {
    //    Dictionary<string, string> list;
    //    if (listOfLists.ContainsKey(id) == true) {
    //        // retrieve the existing list
    //        list = listOfLists[id];
    //    } else {
    //        // create a new list
    //        list = new Dictionary<string, string>();
    //        // add it to the list of lists
    //        listOfLists.Add(id, list);
    //    }
    //    return list;
    //}

    //private Dictionary<string, V> GetList<V>(string id, Dictionary<string, Dictionary<string, V>> listOfLists) {
    //    Dictionary<string, V> list;
    //    if (listOfLists.ContainsKey(id) == true) {
    //        // retrieve the existing list
    //        list = listOfLists[id];
    //    } else {
    //        // create a new list
    //        list = new Dictionary<string, V>();
    //        // add it to the list of lists
    //        listOfLists.Add(id, list);
    //    }
    //    return list;
    //}

    //private List<V> GetList<V>(string id, Dictionary<string, List<V>> listOfLists) {
    //    List<V> list;
    //    if (listOfLists.ContainsKey(id) == true) {
    //        // retrieve the existing list
    //        list = listOfLists[id];
    //    } else {
    //        // create a new list
    //        list = new List<V>();
    //        // add it to the list of lists
    //        listOfLists.Add(id, list);
    //    }
    //    return list;
    //}
}
