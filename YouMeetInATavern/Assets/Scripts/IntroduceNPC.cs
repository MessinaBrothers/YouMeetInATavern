﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceNPC : MonoBehaviour {

    public static event NPCIntrocedEventHandler npcIntroducedEventHandler;
    public delegate void NPCIntrocedEventHandler(GameObject card);

    public static event EnterTavernModeEventHandler enterTavernModeEventHandler;
    public delegate void EnterTavernModeEventHandler();

    public GameObject cardPrefab;

    public Transform offscreenPos, introPos, enterPos;

    public bool startIntro;

    public float moveIntroTime;
    private float moveIntroTimer;

    private GameObject card;

    private Transform startTransform, endTransform;

    void Start() {
        card = Instantiate(cardPrefab);

        Introduce(card);
    }

    void Update() {
        if (startIntro == true) {
            startIntro = false;

            moveIntroTimer = moveIntroTime;
        }

        if (moveIntroTimer > 0) {
            moveIntroTimer -= Time.deltaTime;
            card.transform.position = Vector3.Slerp(endTransform.position, startTransform.position, moveIntroTimer / moveIntroTime);
            card.transform.rotation = Quaternion.Slerp(endTransform.rotation, startTransform.rotation, moveIntroTimer / moveIntroTime);
        }
        
        if (card.transform.position == enterPos.position) {
            npcIntroducedEventHandler.Invoke(card);
            enabled = false;
        }
    }

    void OnEnable() {
        DialogueButton.dialogueEventHandler += HandleDialogue;
    }

    void OnDisable() {
        DialogueButton.dialogueEventHandler -= HandleDialogue;
    }

    public void Introduce(GameObject card) {
        this.card = card;

        card.transform.position = offscreenPos.position;
        card.transform.rotation = offscreenPos.rotation;

        startTransform = offscreenPos;
        endTransform = introPos;
    }

    private void HandleDialogue(int key) {
        if (key == 0) {
            moveIntroTimer = moveIntroTime;

            startTransform = introPos;
            endTransform = enterPos;

            enterTavernModeEventHandler.Invoke();
        }
    }
}
