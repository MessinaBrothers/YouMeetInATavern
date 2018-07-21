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

    [Header("Dialogue")]
    public Dictionary<uint, Dictionary<string, string>> npc_dialogues;
    public Dictionary<uint, Dictionary<string, string>> npc_questions;
    public List<string> unlockedDialogueKeys;

    public enum GameMode {
        INTRODUCE, CONVERSE, TAVERN
    }
    public GameMode gameMode;

    void Start() {
        gameMode = GameMode.INTRODUCE;

        scenarios = new Dictionary<uint, Scenario>();
        npcData = new Dictionary<uint, NPCData>();

        npc_dialogues = new Dictionary<uint, Dictionary<string, string>>();
        npc_questions = new Dictionary<uint, Dictionary<string, string>>();
        unlockedDialogueKeys = new List<string>();

        // DEBUG
        unlockedDialogueKeys.Add(GameData.DIALOGUE_DEFAULT);
    }
}
