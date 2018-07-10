using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverseNPC : MonoBehaviour {

    public Transform conversePos, enterPos;

    private GameData data;
    
    void Start() {
        data = GameObject.FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        CardClickable.cardClickedEventHandler += HandleInput;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleInput;
    }

    private void HandleInput(GameObject card) {
        // only start conversation with card if in tavern mode
        if (data.gameMode == GameData.GameMode.TAVERN) {
            card.transform.position = conversePos.position;
            // set the game mode to converse
            data.gameMode = GameData.GameMode.CONVERSE;
        }
    }
}
