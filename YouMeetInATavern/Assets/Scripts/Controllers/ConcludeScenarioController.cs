using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcludeScenarioController : MonoBehaviour {

    public Transform itemCenterPos, itemZoomPos, npcCenterPos, npcZoomPos;
    //private Transform itemsParent, npcsParent;

    private Transform cardsParent;
    public Transform handPos;

    public AudioClip unselectClip;

    public Transform[] handPositions;

    private GameData data;

    private Dictionary<string, GameObject> key_card;
    private GameObject[] cardsInHand, cardsInHandHighlightPositions;

    //private SelectCards itemSelection, npcSelection;

    void Start() {
        data = FindObjectOfType<GameData>();

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

        GameObject go = new GameObject("Cards");
        cardsParent = go.transform;
        cardsParent.parent = gameObject.transform;

        go = new GameObject("Transforms");
        go.transform.parent = gameObject.transform;

        key_card = new Dictionary<string, GameObject>();
        cardsInHand = new GameObject[handPositions.Length];
        cardsInHandHighlightPositions = new GameObject[handPositions.Length];

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
    }

    void Update() {

    }

    void OnEnable() {
        InputController.gameInitializedEventHandler += CreateDeck;
        InputController.startConcludeScenarioEventHandler += Load;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.confirmScenarioChoicesEventHandler += ResetTavern;
        InputController.hoverOverCardHandEventHandler += highlightCard;
        InputController.hoverExitCardHandEventHandler += unhighlightCard;
        InputController.cardHandClickedEventHandler += SelectCard;
    }

    void OnDisable() {
        InputController.gameInitializedEventHandler -= CreateDeck;
        InputController.startConcludeScenarioEventHandler -= Load;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.confirmScenarioChoicesEventHandler -= ResetTavern;
        InputController.hoverOverCardHandEventHandler -= highlightCard;
        InputController.hoverExitCardHandEventHandler -= unhighlightCard;
        InputController.cardHandClickedEventHandler -= SelectCard;
    }

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
        
    }

    private static void Wait(GameObject card) {

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

        int numCardsToShow = Mathf.Min((handPositions.Length + 1) / 2, data.unlockedDialogueKeys.Count);
        int handPositionIndex = (handPositions.Length + 1) / 2 - numCardsToShow;

        for (int i = 0; i < numCardsToShow; i++) {
            // get the card
            GameObject card = key_card[data.unlockedDialogueKeys[i]];
            // get the transform
            Transform handPosition = handPositions[handPositionIndex];
            // activate the card hand
            handPosition.gameObject.SetActive(true);
            // set the card's rotation
            card.transform.localRotation = handPosition.rotation;
            // set the card's position
            card.transform.localPosition = handPosition.position;
            // add card to list
            cardsInHand[handPositionIndex] = card;
            // increment the hand position index
            handPositionIndex += 2;
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