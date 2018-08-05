using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {

    public GameObject cardbackPrefab, cardPrefab;

    public bool addCard;

    public float cardDistance;
    public float cardRotationMax;

    public int cardCountMax;
    private int cardCount;

    void Start() {
        cardCount = 0;
    }

    void Update() {
        if (addCard) {
            addCard = false;
            AddCard();
        }
    }

    void OnEnable() {
        InputController.dialogueEventHandler += HandleDialogue;
        InputController.cardEnteredDeckEventHandler += EnterDeck;
    }

    void OnDisable() {
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.cardEnteredDeckEventHandler -= EnterDeck;
    }

    private void HandleDialogue(string unlockKey) {
        if (unlockKey.StartsWith("NPC_") || unlockKey.StartsWith("ITEM_")) {
            GameObject card = CardFactory.CreateCard(unlockKey);

            // move offscreen
            card.transform.position = new Vector3(0, 10, 0);

            // apply Deck layer to card
            MoveToLayer(card.transform, LayerMask.NameToLayer("Deck"));

            InputController.DialogueCardCreated(card);
        }
    }

    //recursive
    void MoveToLayer(Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

    private void EnterDeck(GameObject card) {
        AddCard();
        Destroy(card);
    }

    private void AddCard() {
        if (cardCount < cardCountMax) {
            GameObject card = Instantiate(cardbackPrefab);
            card.transform.parent = gameObject.transform;
            card.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + cardCount * cardDistance,
                transform.position.z
                );
            card.transform.rotation = transform.rotation;
            card.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(-cardRotationMax, cardRotationMax));
            cardCount += 1;
        }
    }
}
