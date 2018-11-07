using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcludeScenarioController : MonoBehaviour {
    
    private Transform cardsParent;
    public Transform handPos, unusedDeckPos, discardDeckPos;

    public Transform[] handPositions, selectedPositions;

    private GameData data;
    private AudioSource audioSource;

    private Dictionary<string, GameObject> key_card;
    private GameObject[] cardsInHand, cardsInHandHighlightPositions, cardsInSelection;
    private Queue<GameObject> unusedDeck, discardDeck;

    void Start() {
        data = FindObjectOfType<GameData>();
        audioSource = GetComponent<AudioSource>();

        key_card = new Dictionary<string, GameObject>();
        cardsInHand = new GameObject[handPositions.Length];
        cardsInHandHighlightPositions = new GameObject[handPositions.Length];
        cardsInSelection = new GameObject[selectedPositions.Length];
        unusedDeck = new Queue<GameObject>();
        discardDeck = new Queue<GameObject>();

        GameObject go = new GameObject("Cards");
        cardsParent = go.transform;
        cardsParent.parent = gameObject.transform;

        go = new GameObject("Transforms");
        go.transform.parent = gameObject.transform;

        // set the hand position ids
        for (int i = 0; i < handPositions.Length; i++) {
            handPositions[i].GetComponent<CardHandCollider>().id = i;
            handPositions[i].gameObject.SetActive(false);

            GameObject highlightPos = new GameObject("ConcludeScenarioController_Transform" + i);
            highlightPos.transform.parent = go.transform;
            Vector3 position = handPositions[i].position;
            position.y += 0.50f;
            highlightPos.transform.position = position;
            highlightPos.transform.rotation = handPositions[i].rotation;
            // set the highlight position
            cardsInHandHighlightPositions[i] = highlightPos;
        }

        // set the selected position ids
        for (int i = 0; i < selectedPositions.Length; i++) {
            selectedPositions[i].GetComponent<CardSelectedCollider>().id = i;
        }
    }

    void OnEnable() {
        InputController.gameflowEndInitialize += CreateDeck;
        InputController.gameflowStartBeginConclusion += Load;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.confirmScenarioChoicesEventHandler += ResetTavern;
        InputController.hoverOverCardHandEventHandler += HighlightCard;
        InputController.hoverExitCardHandEventHandler += UnhighlightCard;
        InputController.cardHandClickedEventHandler += SelectCard;
        InputController.hoverOverCardSelectedEventHandler += Dummy;
        InputController.hoverExitCardSelectedEventHandler += Dummy;
        InputController.cardSelectedClickedEventHandler += UnselectCard;
        InputController.unusedDeckClickedEventHandler += DrawFromUnused;
        InputController.discardDeckClickedEventHandler += DrawFromDiscard;
    }

    void OnDisable() {
        InputController.gameflowEndInitialize -= CreateDeck;
        InputController.gameflowStartBeginConclusion -= Load;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.confirmScenarioChoicesEventHandler -= ResetTavern;
        InputController.hoverOverCardHandEventHandler -= HighlightCard;
        InputController.hoverExitCardHandEventHandler -= UnhighlightCard;
        InputController.cardHandClickedEventHandler -= SelectCard;
        InputController.hoverOverCardSelectedEventHandler -= Dummy;
        InputController.hoverExitCardSelectedEventHandler -= Dummy;
        InputController.cardSelectedClickedEventHandler -= UnselectCard;
        InputController.unusedDeckClickedEventHandler -= DrawFromUnused;
        InputController.discardDeckClickedEventHandler -= DrawFromDiscard;
    }

    private void Dummy(int id) { }

    private void CreateDeck() {
        // create a card for each card data
        foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
            // create the card
            GameObject card = CardFactory.CreateCard(kvp.Key);
            // set the parent
            card.transform.parent = cardsParent;
            // move the card offscreen
            card.transform.position = new Vector3(0, -10, 0);
            // add the card to the list
            key_card.Add(kvp.Key, card);
            // disable the collider
            card.GetComponent<Collider>().enabled = false;
        }
    }

    private void HighlightCard(int id) {
        GameObject card = cardsInHand[id];
        card.GetComponent<CardMove>().Set(handPositions[id], cardsInHandHighlightPositions[id].transform, data.cardHoverSpeed, Wait);
    }

    private void UnhighlightCard(int id) {
        GameObject card = cardsInHand[id];
        card.GetComponent<CardMove>().Set(card.transform, handPositions[id], data.cardHoverExitSpeed, Wait);
    }

    private void SelectCard(Vector3 position, int id) {
        bool isChosen = false;

        for (int i = 0; i < cardsInSelection.Length; i++) {
            GameObject selectedCard = cardsInSelection[i];
            if (selectedCard == null) {
                isChosen = true;

                GameObject card = cardsInHand[id];
                card.GetComponent<CardSFX>().PlayGreeting();
                card.GetComponent<CardMove>().Set(card.transform, selectedPositions[i].transform, data.cardSelectedSpeed, Wait);
                RemoveCardFromHand(id);
                cardsInSelection[i] = card;

                data.chosenAnswerKeys.Add(card.GetComponent<Key>().key);

                if (unusedDeck.Count > 0) {
                    AddCardToHand(unusedDeck.Dequeue(), true);
                } else if (discardDeck.Count > 0) {
                    AddCardToHand(discardDeck.Dequeue(), false);
                }

                audioSource.Play();
                return;
            }
        }

        if (isChosen == false) {
            InputController.ClickConclusionBackground(position);
        }
    }

    private void UnselectCard(Vector3 position, int id) {
        if (data.gameMode == GameData.GameMode.CONCLUDE) {
            GameObject card = cardsInSelection[id];
            if (card != null) {
                if (IsHandFull() == true) {
                    Discard(card);
                } else {
                    AddCardToHand(card, true);
                }
                cardsInSelection[id] = null;
                audioSource.Play();

                data.chosenAnswerKeys.Remove(card.GetComponent<Key>().key);
            } else {
                InputController.ClickConclusionBackground(position);
            }
        }
    }

    private void DrawFromUnused(Vector3 position) {
        if (unusedDeck.Count > 0) {
            if (IsHandFull() == true) {
                Discard(cardsInHand[0]);
                RemoveCardFromHand(0);
            }
            AddCardToHand(unusedDeck.Dequeue(), true);
            audioSource.Play();
        } else {
            InputController.ClickConclusionBackground(position);
        }
    }

    private void DrawFromDiscard(Vector3 position) {
        if (discardDeck.Count > 0) {
            if (IsHandFull() == true) {
                Return(cardsInHand[cardsInHand.Length - 1]);
                RemoveCardFromHand(cardsInHand.Length - 1);
            }
            AddCardToHand(discardDeck.Dequeue(), false);
            audioSource.Play();
        } else {
            InputController.ClickConclusionBackground(position);
        }
    }

    private bool IsHandFull() {
        return cardsInHand[cardsInHand.Length - 1] != null;
    }

    private void Discard(GameObject card) {
        card.GetComponent<CardMove>().Set(card.transform, discardDeckPos, data.cardHoverSpeed, Wait);
        discardDeck.Enqueue(card);
    }

    private void Return(GameObject card) {
        card.GetComponent<CardMove>().Set(card.transform, unusedDeckPos, data.cardHoverSpeed, Wait);
        unusedDeck.Enqueue(card);
    }

    private static void Wait(GameObject card) {

    }

    private void RemoveCardFromHand(int id) {
        for (int i = 0; i < cardsInHand.Length; i++) {
            GameObject card = cardsInHand[i];
            if (card != null) {
                if (i < id) {
                    cardsInHand[i] = null;
                    handPositions[i].gameObject.SetActive(false);
                    cardsInHand[i + 1] = card;
                    handPositions[i + 1].gameObject.SetActive(true);
                    card.GetComponent<CardMove>().Set(card.transform, handPositions[i + 1], data.cardHoverSpeed, Wait);
                    // skip the next card
                    i += 1;
                } else if (i == id) {
                    cardsInHand[i] = null;
                    handPositions[i].gameObject.SetActive(false);
                } else {
                    cardsInHand[i] = null;
                    handPositions[i].gameObject.SetActive(false);
                    cardsInHand[i - 1] = card;
                    handPositions[i - 1].gameObject.SetActive(true);
                    card.GetComponent<CardMove>().Set(card.transform, handPositions[i - 1], data.cardHoverSpeed, Wait);
                }
            }
        }
    }

    private void AddCardToHand(GameObject card, bool isRight) {
        int lastID = -1;

        int moveCardsBy = isRight ? -1 : +1;

        // move all cards according to isRight
        int startIndex = isRight ? 0 : cardsInHand.Length - 1;
        int endIndex = isRight ? cardsInHand.Length : -1;
        for (int i = startIndex; i != endIndex; i = i - moveCardsBy) {
            GameObject existingCard = cardsInHand[i];
            if (existingCard != null) {
                cardsInHand[i] = null;
                handPositions[i].gameObject.SetActive(false);
                cardsInHand[i + moveCardsBy] = existingCard;
                handPositions[i + moveCardsBy].gameObject.SetActive(true);
                existingCard.GetComponent<CardMove>().Set(existingCard.transform, handPositions[i + moveCardsBy], data.cardSelectedSpeed, Wait);
                lastID = i;
            }
        }

        // if no cards moved
        if (lastID == -1) {
            // lastID is the middle, minus 1
            lastID = (handPositions.Length - 1) / 2 - 1;
        }
        // move the card to the last position, plus 1
        cardsInHand[lastID - moveCardsBy] = card;
        handPositions[lastID - moveCardsBy].gameObject.SetActive(true);
        card.GetComponent<CardMove>().Set(card.transform, handPositions[lastID - moveCardsBy], data.cardSelectedSpeed, Wait);
    }

    private int GetHandSize() {
        int handSize = 0;
        for (int i = 0; i < cardsInHand.Length; i++)
            if (cardsInHand[i] != null) handSize += 1;
        return handSize;
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.CONCLUDE) {
            if (card.GetComponent<NPC>() != null) {
                //npcSelection.Select(card);
            } else {
                //itemSelection.Select(card);
            }
        }

    }

    private void ResetTavern() {
        foreach (GameObject card in key_card.Values) {
            card.SetActive(false);
        }
    }

    private void Load() {
        InputController.ChangeMode(GameData.GameMode.CONCLUDE);
        
        ResetCards();

        // create a list of unlocked cards
        List<GameObject> cards = new List<GameObject>();
        foreach (string key in data.unlockedDialogueKeys) {
            if (key.StartsWith("ITEM_") || key.StartsWith("NPC_")) {
                GameObject card = key_card[key];
                card.SetActive(true);
                cards.Add(card);
            }
        }

        // create the hand
        int numCardsToShow = Mathf.Min((handPositions.Length + 1) / 2, cards.Count);
        int handPositionIndex = (handPositions.Length + 1) / 2 - numCardsToShow;

        for (int i = 0; i < numCardsToShow; i++) {
            GameObject card = cards[i];

            // activate the hand position
            Transform handPosition = handPositions[handPositionIndex];
            handPosition.gameObject.SetActive(true);
            SetPosition(card, handPosition);
            
            cardsInHand[handPositionIndex] = card;

            handPositionIndex += 2;
        }

        // for all other cards, create a deck of unused cards
        for (int i = numCardsToShow; i < cards.Count; i++) {
            GameObject card = cards[i];
            SetPosition(card, unusedDeckPos);
            unusedDeck.Enqueue(card);
        }
    }
    
    private void ResetCards() {
        unusedDeck.Clear();
        discardDeck.Clear();
        for (int i = 0; i < cardsInHand.Length; i++) {
            cardsInHand[i] = null;
        }
        for (int i = 0; i < cardsInSelection.Length; i++) {
            cardsInSelection[i] = null;
        }
        for (int i = 0; i < handPositions.Length; i++) {
            handPositions[i].gameObject.SetActive(false);
        }
        foreach (GameObject card in key_card.Values) {
            card.SetActive(false);
            card.GetComponent<CardMove>().Stop();
        }
    }

    private void SetPosition(GameObject card, Transform pos) {
        // set the card's rotation
        card.transform.localRotation = pos.rotation;
        // set the card's position
        card.transform.localPosition = pos.position;
    }
}