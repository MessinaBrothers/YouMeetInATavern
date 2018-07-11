using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static int DIALOGUE_DEFAULT;

    public GameObject selectedCard;

    public enum GameMode {
        INTRODUCE, CONVERSE, TAVERN
    }
    public GameMode gameMode;

    void Start() {
        gameMode = GameMode.INTRODUCE;
    }
}
