using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static int DIALOGUE_DEFAULT = -1; //TODO delete
    public static uint DIALOGUE_INTRO = 0;
    public static uint DIALOGUE_GENERIC = 1;
    public static uint DIALOGUE_INVALID = uint.MaxValue;

    public GameObject selectedCard;

    [Header("Dialogue")]
    public Dictionary<uint, Dictionary<uint, string>> npc_dialogues;
    public Dictionary<uint, Dictionary<uint, string>> npc_questions;
    public bool[] isDialogueIndexUnlocked;

    public enum GameMode {
        INTRODUCE, CONVERSE, TAVERN
    }
    public GameMode gameMode;

    void Start() {
        gameMode = GameMode.INTRODUCE;

        npc_dialogues = new Dictionary<uint, Dictionary<uint, string>>();
        npc_questions = new Dictionary<uint, Dictionary<uint, string>>();
        isDialogueIndexUnlocked = new bool[1024*2*2];

        // DEBUG
        isDialogueIndexUnlocked[1] = true;
    }
}
