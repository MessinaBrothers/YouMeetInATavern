using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
    
    public static string DIALOGUE_INTRO = "intro";
    public static string DIALOGUE_DEFAULT = "default";
    public static string DIALOGUE_INVALID = "";
    public static char PARSER_DELIMITER = '-';

    public GameObject selectedCard;

    public List<GameObject> npcs;
    public uint dayCount;
    public Dictionary<uint, Scenario> scenarios;
    public Scenario scenario;
    public Dictionary<string, NPCData> npcData;
    public Dictionary<string, ItemData> itemData;
    public Dictionary<uint, List<ScenarioResult>> scenarioResultsData;

    [Header("Dialogue")]
    public Dictionary<string, Dictionary<string, string>> npc_dialogues;
    public Dictionary<string, Dictionary<string, string>> npc_questions;
    public List<string> unlockedDialogueKeys;

    public enum GameMode {
        INTRODUCE, CONVERSE, TAVERN, CONCLUDE
    }
    public GameMode gameMode;

    public enum Location {
        NONE, ROAD, MOUNTAIN, TOWN, FOREST
    }
    [Header("Conclusion")]
    public Location chosenLocation;
    public string resultsDialogue;

    void Start() {
        gameMode = GameMode.INTRODUCE;

        scenarios = new Dictionary<uint, Scenario>();
        npcData = new Dictionary<string, NPCData>();
        itemData = new Dictionary<string, ItemData>();
        scenarioResultsData = new Dictionary<uint, List<ScenarioResult>>();

        npc_dialogues = new Dictionary<string, Dictionary<string, string>>();
        npc_questions = new Dictionary<string, Dictionary<string, string>>();
        unlockedDialogueKeys = new List<string>();

        // DEBUG
        unlockedDialogueKeys.Add(GameData.DIALOGUE_DEFAULT);
    }
}
