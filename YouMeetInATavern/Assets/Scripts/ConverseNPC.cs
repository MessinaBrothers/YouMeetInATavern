using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverseNPC : MonoBehaviour {

    public Transform conversePos, enterPos;
    
    void Start() {

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
        card.transform.position = conversePos.position;
    }
}
