using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
    
    public static string DIALOGUE_INTRO = "intro";
    public static string DIALOGUE_DEFAULT = "default";
    public static string DIALOGUE_INVALID = "";

    public GameObject selectedCard;

    public List<GameObject> npcs;
    public uint dayCount;
    public Dictionary<uint, Scenario> scenarios;
    public Scenario scenario;
    public Dictionary<uint, NPCData> npcData;
    public Dictionary<string, ItemData> itemData;

    [Header("Dialogue")]
    public Dictionary<uint, Dictionary<string, string>> npc_dialogues;
    public Dictionary<uint, Dictionary<string, string>> npc_questions;
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

    void Start() {
        gameMode = GameMode.INTRODUCE;

        scenarios = new Dictionary<uint, Scenario>();
        npcData = new Dictionary<uint, NPCData>();
        itemData = new Dictionary<string, ItemData>();

        npc_dialogues = new Dictionary<uint, Dictionary<string, string>>();
        npc_questions = new Dictionary<uint, Dictionary<string, string>>();
        unlockedDialogueKeys = new List<string>();

        // DEBUG
        unlockedDialogueKeys.Add(GameData.DIALOGUE_DEFAULT);
    }
}
