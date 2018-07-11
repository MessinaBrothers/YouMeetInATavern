using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveController : MonoBehaviour {

    public Transform offscreenPos, introPos;

    private CardMove move;
    private CardWander wander;

    void Start() {
        move = gameObject.AddComponent<CardMove>();
        move.enabled = false;

        wander = gameObject.AddComponent<CardWander>();
        wander.SetRange(-2, -2, 2, 2);
        wander.speed = 1;
        wander.enabled = false;
    }

    void Update() {

    }

    void OnEnable() {
        CardEnterTavern.npcEnteredTavernEventHandler += Move;
        DialogueButton.dialogueEventHandler += HandleDialogue;
        IntroduceNPC.npcIntroStartEventHandler += Introduce;
    }

    void OnDisable() {
        CardEnterTavern.npcEnteredTavernEventHandler -= Move;
        DialogueButton.dialogueEventHandler -= HandleDialogue;
        IntroduceNPC.npcIntroStartEventHandler -= Introduce;
    }

    private void HandleDialogue(int key) {
        if (key == GameData.DIALOGUE_DEFAULT) {
            
        }
    }

    private void Introduce(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(offscreenPos, introPos, 1, Wait);
    }

    private void Move(GameObject card) {
        card.GetComponent<CardWander>().enabled = true;
    }

    private void Wait(GameObject card) {

    }
}
