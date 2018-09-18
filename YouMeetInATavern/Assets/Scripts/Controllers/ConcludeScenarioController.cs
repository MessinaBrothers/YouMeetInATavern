using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcludeScenarioController : MonoBehaviour {

    public Transform itemCenterPos, itemZoomPos, npcCenterPos, npcZoomPos;
    //private Transform itemsParent, npcsParent;

    private Transform cardsParent;
    public Transform handPos, unusedDeckPos, discardDeckPos;

    public Transform[] handPositions, selectedPositions;

    private GameData data;
    private AudioSource audioSource;

    private Dictionary<string, GameObject> key_card;
    private GameObject[] cardsInHand, cardsInHandHighlightPositions, cardsInSelection;
    private Queue<GameObject> unusedDeck, discardDeck;

    //private SelectCards itemSelection, npcSelection;

    void Start() {
        data = FindObjectOfType<GameData>();
        audioSource = GetComponent<AudioSource>();

        GameObject go = new GameObject("Cards");
        cardsParent = go.transform;
        cardsParent.parent = gameObject.transform;

        go = new GameObject("Transforms");
        go.transform.parent = gameObject.transform;

        key_card = new Dictionary<string, GameObject>();
        cardsInHand = new GameObject[handPositions.Length];
        cardsInHandHighlightPositions = new GameObject[handPositions.Length];
        cardsInSelection = new GameObject[selectedPositions.Length];
        unusedDeck = new Queue<GameObject>();
        discardDeck = new Queue<GameObject>();

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

        //GameObject go = new GameObject("Items");
        //itemsParent = go.transform;
        //itemsParent.parent = gameObject.transform;

        //GameObject go2 = new GameObject("NPCs");
        //npcsParent = go2.transform;
        //npcsParent.parent = gameObject.transform;

        //// card selections
        //itemSelection = gameObject.AddComponent<SelectCards>();
        //itemSelection.unselectClip = unselectClip;
        //npcSelection = gameObject.AddComponent<SelectCards>();
        //npcSelection.unselectClip = unselectClip;
    }

    void OnEnable() {
        InputController.gameInitializedEventHandler += CreateDeck;
        InputController.startConcludeScenarioEventHandler += Load;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.confirmScenarioChoicesEventHandler += ResetTavern;
        InputController.hoverOverCardHandEventHandler += highlightCard;
        InputController.hoverExitCardHandEventHandler += unhighlightCard;
        InputController.cardHandClickedEventHandler += SelectCard;
        InputController.hoverOverCardSelectedEventHandler += Dummy;
        InputController.hoverExitCardSelectedEventHandler += Dummy;
        InputController.cardSelectedClickedEventHandler += UnselectCard;
        InputController.unusedDeckClickedEventHandler += DrawFromUnused;
        InputController.discardDeckClickedEventHandler += DrawFromDiscard;
    }

    void OnDisable() {
        InputController.gameInitializedEventHandler -= CreateDeck;
        InputController.startConcludeScenarioEventHandler -= Load;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.confirmScenarioChoicesEventHandler -= ResetTavern;
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

    private void highlightCard(int id) {
        GameObject card = cardsInHand[id];
        card.GetComponent<CardMove>().Set(handPositions[id], cardsInHandHighlightPositions[id].transform, data.cardHoverSpeed, Wait);
    }

    private void unhighlightCard(int id) {
        GameObject card = cardsInHand[id];
        card.GetComponent<CardMove>().Set(card.transform, handPositions[id], data.cardHoverExitSpeed, Wait);
    }

    private void SelectCard(int id) {
        for (int i = 0; i < cardsInSelection.Length; i++) {
            GameObject selectedCard = cardsInSelection[i];
            if (selectedCard == null) {
                GameObject card = cardsInHand[id];
                card.GetComponent<CardSFX>().PlayGreeting();
                card.GetComponent<CardMove>().Set(card.transform, selectedPositions[i].transform, data.cardSelectedSpeed, Wait);
                RemoveCardFromHand(id);
                cardsInSelection[i] = card;

                if (unusedDeck.Count > 0) {
                    AddCardToHand(unusedDeck.Dequeue(), true);
                } else if (discardDeck.Count > 0) {
                    AddCardToHand(discardDeck.Dequeue(), false);
                }

                audioSource.Play();
                return;
            }
        }
    }

    private void UnselectCard(int id) {
        GameObject card = cardsInSelection[id];
        if (card != null) {
            if (IsHandFull() == true) {
                Discard(card);
            } else {
                AddCardToHand(card, true);
            }
            cardsInSelection[id] = null;
            audioSource.Play();
        }
    }

    private void DrawFromUnused() {
        if (unusedDeck.Count > 0) {
            if (IsHandFull() == true) {
                Discard(cardsInHand[0]);
                RemoveCardFromHand(0);
            }
            AddCardToHand(unusedDeck.Dequeue(), true);
            audioSource.Play();
        }
    }

    private void DrawFromDiscard() {
        if (discardDeck.Count > 0) {
            if (IsHandFull() == true) {
                Return(cardsInHand[cardsInHand.Length - 1]);
                RemoveCardFromHand(cardsInHand.Length - 1);
            }
            AddCardToHand(discardDeck.Dequeue(), false);
            audioSource.Play();
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
            print(i);
            GameObject existingCard = cardsInHand[i];
            if (existingCard != null) {
                cardsInHand[i] = null;
                handPositions[i].gameObject.SetActive(false);
                cardsInHand[i + moveCardsBy] = existingCard;
                handPositions[i + moveCardsBy].gameObject.SetActive(true);
                existingCard.GetComponent<CardMove>().Set(existingCard.transform, handPositions[i + moveCardsBy], data.cardHoverSpeed, Wait);
                lastID = i;
            }
        }

        // if no cards moved
        if (lastID == -1) {
            // lastID is the middle, minus 1
            lastID = (handPositions.Length - 1) / 2 - 1;
        }
        print(lastID);
        // move the card to the last position, plus 1
        cardsInHand[lastID - moveCardsBy] = card;
        handPositions[lastID - moveCardsBy].gameObject.SetActive(true);
        card.GetComponent<CardMove>().Set(card.transform, handPositions[lastID - moveCardsBy], data.cardHoverSpeed, Wait);
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
        foreach (Transform child in cardsParent.transform) {
            Destroy(child.gameObject);
        }
    }

    private void Load() {
        InputController.ChangeMode(GameData.GameMode.CONCLUDE);

        float currentXOffset = -(data.unlockedDialogueKeys.Count - 1) / 2;

        Vector3 cardPosition = handPos.position;

        // create the hand
        int numCardsToShow = Mathf.Min((handPositions.Length + 1) / 2, data.unlockedDialogueKeys.Count);
        int handPositionIndex = (handPositions.Length + 1) / 2 - numCardsToShow;

        for (int i = 0; i < numCardsToShow; i++) {
            GameObject card = key_card[data.unlockedDialogueKeys[i]];

            // activate the hand position
            Transform handPosition = handPositions[handPositionIndex];
            handPosition.gameObject.SetActive(true);
            SetPosition(card, handPosition);
            
            cardsInHand[handPositionIndex] = card;

            handPositionIndex += 2;
        }

        // for all other cards, create a deck of unused cards
        for (int i = numCardsToShow; i < data.unlockedDialogueKeys.Count; i++) {
            GameObject card = key_card[data.unlockedDialogueKeys[i]];
            SetPosition(card, unusedDeckPos);
            unusedDeck.Enqueue(card);
        }
        
        //float xOffset = 2.5f;
        //float x = 0;

        //// ITEMS
        //foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
        //    if (kvp.Key.StartsWith("ITEM_") && DeckController.Contains(kvp.Key)) {
        //        // create the card
        //        GameObject card = CardFactory.CreateCard(kvp.Key);
        //        // add a zoom script
        //        GameObject cardParent = AddZoom(card, x, itemCenterPos, itemZoomPos);
        //        // set the parent
        //        cardParent.transform.parent = itemsParent;
        //        // offset x for the next card
        //        x += xOffset;
        //    }
        //}

        //xOffset = 2f;
        //x = 0;

        //// NPCs
        //foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
        //    if (kvp.Key.StartsWith("NPC_") && DeckController.Contains(kvp.Key)) {
        //        // create the card
        //        GameObject card = CardFactory.CreateCard(kvp.Key);
        //        // add a zoom script
        //        GameObject cardParent = AddZoom(card, x, npcCenterPos, npcZoomPos);
        //        // set the parent
        //        cardParent.transform.parent = npcsParent;
        //        // offset x for the next card
        //        x += xOffset;
        //    }
        //}
    }

    private void SetPosition(GameObject card, Transform pos) {
        // set the card's rotation
        card.transform.localRotation = pos.rotation;
        // set the card's position
        card.transform.localPosition = pos.position;
    }

    //private GameObject AddZoom(GameObject card, float x, Transform centerTransform, Transform zoomTransform) {
    //    GameObject cardParent = new GameObject(card.name + " Parent");
    //    card.transform.parent = cardParent.transform;

    //    // create default position
    //    GameObject defaultPos = new GameObject("DefaultPos");
    //    defaultPos.transform.position = new Vector3(
    //        centerTransform.position.x + x,
    //        centerTransform.position.y,
    //        centerTransform.position.z
    //        );
    //    defaultPos.transform.rotation = centerTransform.rotation;

    //    // create zoom position
    //    GameObject zoomPos = new GameObject("ZoomPos");
    //    zoomPos.transform.position = new Vector3(
    //        centerTransform.position.x + x,
    //        zoomTransform.position.y,
    //        zoomTransform.position.z
    //        );
    //    zoomPos.transform.rotation = zoomTransform.rotation;

    //    // set the parents
    //    defaultPos.transform.parent = cardParent.transform;
    //    zoomPos.transform.parent = cardParent.transform;

    //    // add zoom script
    //    CardZoom zoom = card.AddComponent<CardZoom>();
    //    zoom.defaultPos = defaultPos;
    //    zoom.zoomPos = zoomPos;

    //    // start the position at its default
    //    card.transform.position = defaultPos.transform.position;
    //    card.transform.rotation = defaultPos.transform.rotation;

    //    return cardParent;
    //}
}