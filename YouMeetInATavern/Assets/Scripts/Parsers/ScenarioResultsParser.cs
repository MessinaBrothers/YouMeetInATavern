using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class ScenarioResultsParser : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        Parse();
    }

    private void Parse() {
        TextAsset xmlRawFile = (TextAsset)Resources.Load("results");
        XDocument xmlDoc = XDocument.Load(new StringReader(xmlRawFile.text));

        XElement root = xmlDoc.Element(GameData.YED_BULLCRAP + "graphml");
        XElement graph = root.Element(GameData.YED_BULLCRAP + "graph");

        foreach (XElement node in graph.Elements(GameData.YED_BULLCRAP + "node")) {
            bool isGraph = false;
            foreach (XAttribute xa in node.Attributes()) {
                if (xa.ToString().StartsWith("yfiles.foldertype=\"")) {
                    isGraph = true;
                }
            }

            if (isGraph) {
                ParseScenarioResults(node);
            } else {
                ParseDialogue(node);
            }
        }
        
        // parse dialogue edges
        foreach (XElement edge in xmlDoc.Descendants(GameData.YED_BULLCRAP + "edge")) {
            ParseEdge(edge);
        }
    }

    private void ParseScenarioResults(XElement node) {
        // get the scenario key
        string scenarioKey = "";
        foreach (XElement data in node.Elements(GameData.YED_BULLCRAP + "data")) {
            if (scenarioKey.Length == 0 && data.Value.StartsWith("nodetype=scenario_")) {
                scenarioKey = data.Value.Replace("nodetype=scenario_", "");
            }
        }

        XElement nodes = node.Element(GameData.YED_BULLCRAP + "graph");
        foreach (XElement nodeElement in nodes.Elements(GameData.YED_BULLCRAP + "node")) {
            Dialogue dialogue = ParseDialogue(nodeElement);

            if (dialogue.type == Dialogue.TYPE.START) {
                data.scenarioKey_resultsKey.Add(scenarioKey, dialogue.key);
            }
        }
    }

    private Dialogue ParseDialogue(XElement node) {
        // parse the ID
        string dialogueKey = "";
        dialogueKey = node.Attribute("id").ToString().Replace("id=", "");

        // parse the dialogue type and text
        string nodeText = "";
        string nodeType = "";
        foreach (XElement xe1 in node.Elements(GameData.YED_BULLCRAP + "data")) {
            if (xe1.Value.StartsWith("nodetype=")) {
                nodeType = xe1.Value.Replace("nodetype=", "");
            } else {
                nodeText = xe1.Value;
            }
        }

        // create the Dialogue
        Dialogue dialogue = CreateDialogue(dialogueKey, nodeText, nodeType);
        data.key_results.Add(dialogueKey, dialogue);

        return dialogue;
    }

    private Dialogue CreateDialogue(string key, string text, string nodeType) {
        Dialogue dialogue = new Dialogue(key, text);

        switch (nodeType) {
            case "start":
                dialogue.type = Dialogue.TYPE.START;
                break;
            case "result":
                dialogue.type = Dialogue.TYPE.INQUIRY;
                break;
            case "tag":
                dialogue.type = Dialogue.TYPE.TAG;
                break;
            case "card":
                dialogue.type = Dialogue.TYPE.CARD;
                break;
            default:
                print("PARSE ERROR: No such dialogue type exists: " + nodeType);
                break;
        }

        return dialogue;
    }

    private void ParseEdge(XElement edge) {
        // parse source and target
        string source = "";
        string target = "";
        foreach (XAttribute xa in edge.Attributes()) {
            if (xa.ToString().StartsWith("source=")) {
                source = xa.ToString().Replace("source=", "");
            } else if (xa.ToString().StartsWith("target=")) {
                target = xa.ToString().Replace("target=", "");
            } else {
                //ignore id
            }
        }

        // save target in source, depending on target's type
        Dialogue sourceDialogue = data.key_results[source];
        Dialogue targetDialogue = data.key_results[target];

        switch (sourceDialogue.type) {
            case Dialogue.TYPE.CARD:
                break;
            case Dialogue.TYPE.TAG:
                break;
            default:
                break;
        }

        if (sourceDialogue.type == Dialogue.TYPE.CARD) {
            targetDialogue.unlockCardKeys.Add(sourceDialogue.text);
        } else if (targetDialogue.type == Dialogue.TYPE.TAG) {
            print("Adding tag:" + targetDialogue.text);
            sourceDialogue.tags.Add(targetDialogue.text);
        } else if (targetDialogue.type == Dialogue.TYPE.INQUIRY) {
            sourceDialogue.nextDialogueKey = target;
        } else {
            print("PARSE ERROR: edge unaccounted for: " + sourceDialogue.text);
        }
    }
}
