using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
    
    public static string DIALOGUE_INTRO = "intro";
    public static string DIALOGUE_DEFAULT = "default";
    public static string DIALOGUE_INVALID = "";
    public static string DIALOGUE_SCENARIO_PREFIX = "SCENARIO_";
    public static string DIALOGUE_SETTING = "=";
    public static string SCENARIO_RESULT_DEFAULT = "default";
    public static char PARSER_DELIMITER = '-';
    public static string YED_BULLCRAP = "{http://graphml.graphdrawing.org/xmlns}";
    public static char GRAPH_INQUIRY_SPLIT = ':';
    public static char GRAPH_DELIMITER = ' ';

    [Header("DEBUG")]
    public float DEBUG_SPEED_EDITOR = 1f;
    public static float DEBUG_SPEED;
    public bool DEBUG_IS_PRINT;
    public bool DEBUG_IS_PRINT_PARSER;
    public bool DEBUG_IS_PRINT_DIALOGUE;

    public uint dayCount;

    [Header("Clock")]
    public int currentHour;
    public int tavernOpenHour;
    public int tavernCloseHour;
    public Dictionary<string, CardData> cardData;

    [Header("NPCs")]
    public GameObject selectedCard;
    public List<GameObject> npcs;
    public List<GameObject> npcsInTavern;
    public Queue<string> npcsToIntroduce;
    public Queue<GameObject> npcsToReintroduce;
    public float cardIntroductionSpeed = 1f;
    public float cardConverseSpeed = 1f;
    public float cardEnterTavernSpeed = 1f;
    public float cardLeaveTavernSpeed = 1f;
    public float cardPreviewSpeed = 0.75f;
    public float cardPreviewWaitTime = 0.5f;
    public float cardPreviewEnterDeckSpeed = 0.5f;
    public float cardHoverSpeed = 1f;
    public float cardHoverExitSpeed = 3f;
    public float cardSelectedSpeed = 2f;
    
    [Header("NPC Wander Areas")]
    public Transform[] wanderAreasBartender;
    public Transform[] wanderAreasTavern;
    public Transform[] wanderAreasExit;

    /// <summary>
    /// Scenarios indexed by order they appear in game.
    /// NOT by scenario index.
    /// </summary>
    public Scenario[] scenarios;
    public Scenario scenario;
    [Header("Scenario")]
    public uint nextScenarioIndex;

    [Header("Dialogue")]
    public string nextDialogueIntroKey;
    public HashSet<string> unlockedDialogueKeys;
    public bool isGoodbyeEnabled;
    
    public Dictionary<string, Dialogue> key_dialoguesNEW;
    public Dictionary<string, Dialogue> key_results;
    public Dictionary<string, string> npcKey_introKey;
    public Dictionary<string, string> npcKey_scenarioKey;
    public Dictionary<string, string> npcKey_resultsKey;
    public Dictionary<string, string> npcKey_defaultDialogueKey;
    public Dictionary<string, string> scenarioKey_resultsKey;

    public enum Location {
        NONE, LOCATION_ROAD, LOCATION_MOUNTAIN, LOCATION_TOWN, LOCATION_FOREST, LOCATION_DOCKS
    }

    public HashSet<string> chosenAnswerKeys;
    [Header("Conclusion")]
    public Location[] hexIndex_location;
    public Location chosenLocation;
    public string resultsDialogue;

    [Header("Misc")]
    public float fadeInTime;
    public float fadeOutTime;
    public float introPauseTime;
    public float fadeInTimeDefault;
    public float introPauseTimeDefault;
    public float popupGrowTime;
    public float popupShrinkTime;
    public float tutorialTextFadeTime;
    public bool isLeaveButtonEnabled;

    public Color buttonItemColor, buttonNPCColor, buttonLocationColor;

    public enum GameMode {
        INTRODUCE, CONVERSE, TAVERN, CONCLUDE, RESULTS
    }
    public GameMode gameMode;

    void Start() {
        DEBUG_SPEED = DEBUG_SPEED_EDITOR;

        gameMode = GameMode.INTRODUCE;
        
        cardData = new Dictionary<string, CardData>();

        npcs = new List<GameObject>();
        npcsToIntroduce = new Queue<string>();
        npcsToReintroduce = new Queue<GameObject>();
        
        key_dialoguesNEW = new Dictionary<string, Dialogue>();
        key_results = new Dictionary<string, Dialogue>();
        npcKey_introKey = new Dictionary<string, string>();
        npcKey_scenarioKey = new Dictionary<string, string>();
        npcKey_resultsKey = new Dictionary<string, string>();
        npcKey_defaultDialogueKey = new Dictionary<string, string>();
        scenarioKey_resultsKey = new Dictionary<string, string>();

        unlockedDialogueKeys = new HashSet<string>();
        chosenAnswerKeys = new HashSet<string>();
    }
}
