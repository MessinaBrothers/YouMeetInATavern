﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveController : MonoBehaviour {

    public static event NPCInConversePosEventHandler npcInConversePosEventHandler;
    public delegate void NPCInConversePosEventHandler(GameObject card);

    public Transform offscreenPos, introPos, conversePos, enterTavernPos;
    private Transform dumbTransform;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();

        // we need a new transform to copy the card's initial transform into our start transform
        GameObject go = new GameObject("CardMoveController_DumbTransform");
        go.transform.parent = transform;
        dumbTransform = go.transform;
    }

    void Update() {

    }

    void OnEnable() {
        IntroduceNPC.npcIntroStartEventHandler += Introduce;
        IntroduceNPC.npcIntroEndEventHandler += Converse;
        ConverseNPC.npcStartConverseEventHandler += Converse;
        InputController.stopConverseEventHandler += Stop;
    }

    void OnDisable() {
        IntroduceNPC.npcIntroStartEventHandler -= Introduce;
        IntroduceNPC.npcIntroEndEventHandler -= Converse;
        ConverseNPC.npcStartConverseEventHandler -= Converse;
        InputController.stopConverseEventHandler -= Stop;
    }

    private void Introduce(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(offscreenPos, introPos, 1, Wait);
    }

    private void Converse(GameObject card) {
        CardMove move = card.GetComponent<CardMove>();
        move.enabled = true;

        // copy the card's position and rotation into our startTransform
        dumbTransform.position = card.transform.position;
        dumbTransform.rotation = card.transform.rotation;

        move.Set(dumbTransform, conversePos, 1, StartDialogue);
        
        // disable wandering
        card.GetComponent<CardWander>().enabled = false;
    }

    private void Stop() {
        CardMove move = data.selectedCard.GetComponent<CardMove>();
        move.enabled = true;
        move.Set(conversePos, enterTavernPos, 1, Wander);
    }

    private void Wander(GameObject card) {
        card.GetComponent<CardMove>().enabled = false;
        card.GetComponent<CardWander>().enabled = true;
    }

    private void Wait(GameObject card) {

    }

    private void StartDialogue(GameObject card) {
        npcInConversePosEventHandler.Invoke(card);
    }
}