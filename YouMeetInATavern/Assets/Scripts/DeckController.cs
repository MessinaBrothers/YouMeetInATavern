using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {

    public GameObject cardbackPrefab, cardPrefab;

    public Transform[] deckCardTransforms, handTransforms;

    private GameObject[] deckCards;

    public float spreadTime;
    private float spreadTimer;
    
    private int cardCount;

    private bool isSpreading;

    private List<string> alreadyUnlockedKeywords;
    
    [Header("DEBUG")]
    public bool addCard;
    public int startWithAddedCards;

    void Start() {
        cardCount = 0;
        spreadTimer = 0;

        isSpreading = false;

        alreadyUnlockedKeywords = new List<string>();

        // create deck of cards
        deckCards = new GameObject[deckCardTransforms.Length];
        for (int i = 0; i < deckCards.Length; i++) {
            GameObject card = Instantiate(cardbackPrefab, deckCardTransforms[i]);
            card.transform.parent = gameObject.transform;
            // deactivate card
            card.SetActive(false);
            // save card
            deckCards[i] = card;
        }

        for (int i = 0; i < startWithAddedCards; i++) AddCard(); //DEBUG
    }

    void Update() {
        if (addCard) { addCard = false; AddCard(); } //DEBUG

        if (isSpreading == true) {
            if (spreadTimer < spreadTime) spreadTimer += Time.deltaTime;
        } else {
            if (spreadTimer > 0) spreadTimer -= Time.deltaTime;
        }

        for (int i = 0; i < deckCards.Length; i++) {
            deckCards[i].transform.position = Vector3.Lerp(deckCardTransforms[i].position, handTransforms[i].position, spreadTimer / spreadTime);
            deckCards[i].transform.rotation = Quaternion.Lerp(deckCardTransforms[i].rotation, handTransforms[i].rotation, spreadTimer / spreadTime);
        }
    }

    void OnEnable() {
        InputController.dialogueEventHandler += HandleDialogue;
        InputController.cardEnteredDeckEventHandler += EnterDeck;
        InputController.deckHoverEventHandler += ToggleSpreading;
        InputController.deckClickedEventHander += DisplayDeck;
    }

    void OnDisable() {
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.cardEnteredDeckEventHandler -= EnterDeck;
        InputController.deckHoverEventHandler -= ToggleSpreading;
        InputController.deckClickedEventHander -= DisplayDeck;
    }

    private void HandleDialogue(string unlockKey) {
        if (alreadyUnlockedKeywords.Contains(unlockKey) == false && (unlockKey.StartsWith("NPC_") || unlockKey.StartsWith("ITEM_"))) {
            alreadyUnlockedKeywords.Add(unlockKey);

            GameObject card = CardFactory.CreateCard(unlockKey);

            // move offscreen
            card.transform.position = new Vector3(0, 10, 0);

            // apply Deck layer to card
            MoveToLayer(card.transform, LayerMask.NameToLayer("Deck"));

            InputController.DialogueCardCreated(card);
        }
    }

    //recursive
    private void MoveToLayer(Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

    private void EnterDeck(GameObject card) {
        AddCard();
        Destroy(card);
    }

    private void AddCard() {
        if (cardCount < deckCards.Length) {
            // activate the next card
            deckCards[cardCount].SetActive(true);
            cardCount += 1;
        }
    }

    private void ToggleSpreading(bool isHover) {
        isSpreading = isHover;
    }

    private void DisplayDeck() {

    }
}
