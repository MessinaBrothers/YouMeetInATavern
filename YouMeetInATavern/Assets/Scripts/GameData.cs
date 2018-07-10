using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public enum GameMode {
        CONVERSE, TAVERN
    }
    public GameMode gameMode;

    void Start() {
        gameMode = GameMode.CONVERSE;
    }
}
