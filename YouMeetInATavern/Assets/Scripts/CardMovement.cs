using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour {

    private CardWander wander;

    void Start() {
        wander = gameObject.AddComponent<CardWander>();
        wander.SetRange(-2, -2, 2, 2);
        wander.speed = 1;
        wander.enabled = false;
    }

    void Update() {
    }

    void OnEnable() {
        IntroduceNPC.npcIntroducedEventHandler += Move;
    }

    void OnDisable() {
        IntroduceNPC.npcIntroducedEventHandler -= Move;
    }

    private void Move(GameObject card) {
        if (card == gameObject) {
            wander.enabled = true;
        }
    }
}
