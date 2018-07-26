using AuraAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcludeScenarioController : MonoBehaviour {

    public GameObject itemCardPrefab, tavern;

    public Transform itemCenterPos, itemZoomPos, npcCenterPos, npcZoomPos;
    private Transform itemsParent, npcsParent;

    public AudioClip unselectClip;

    private GameData data;

    private SelectCards itemSelection, npcSelection;

    //DEBUG
    public bool isLoad;

    void Start() {
        data = FindObjectOfType<GameData>();

        GameObject go = new GameObject("Items");
        itemsParent = go.transform;
        itemsParent.parent = gameObject.transform;

        GameObject go2 = new GameObject("NPCs");
        npcsParent = go2.transform;
        npcsParent.parent = gameObject.transform;

        // card selections
        itemSelection = gameObject.AddComponent<SelectCards>();
        itemSelection.unselectClip = unselectClip;
        npcSelection = gameObject.AddComponent<SelectCards>();
        npcSelection.unselectClip = unselectClip;
    }

    void Update() {
        if (isLoad == true) {
            isLoad = false;
            InputController.ConcludeScenario();
        }
    }

    void OnEnable() {
        InputController.startConcludeScenarioEventHandler += Load;
        InputController.cardClickedEventHandler += HandleCardClick;
    }

    void OnDisable() {
        InputController.startConcludeScenarioEventHandler -= Load;
        InputController.cardClickedEventHandler -= HandleCardClick;
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode != GameData.GameMode.CONCLUDE) return;

        if (card.GetComponent<NPC>() != null) {
            npcSelection.Select(card);
        } else {
            itemSelection.Select(card);
        }
    }

    private void Load() {
        data.gameMode = GameData.GameMode.CONCLUDE;

        // hide the tavern
        tavern.gameObject.SetActive(false);

        // DEBUG
        foreach (KeyValuePair<string, ItemData> kvp in data.itemData) {
            data.unlockedDialogueKeys.Add(kvp.Key);
        }
        foreach (KeyValuePair<string, NPCData> kvp in data.npcData) {
            data.unlockedDialogueKeys.Add(kvp.Key);
        }
        //data.unlockedDialogueKeys.Add("ITEM_GOLD");
        //data.unlockedDialogueKeys.Add("ITEM_THIEFSKIT");

        float xOffset = 2.5f;
        float x = 0;

        // ITEMS
        foreach (KeyValuePair<string, ItemData> kvp in data.itemData) {
            if (data.unlockedDialogueKeys.Contains(kvp.Key)) {
                // create the card
                GameObject card = CreateItemCard(kvp.Key);
                // add a zoom script
                GameObject cardParent = AddZoom(card, x, itemCenterPos, itemZoomPos);
                // set the parent
                cardParent.transform.parent = itemsParent;
                // offset x for the next card
                x += xOffset;
            }
        }

        xOffset = 2f;
        x = 0;

        // NPCs
        foreach (KeyValuePair<string, NPCData> kvp in data.npcData) {
            if (data.unlockedDialogueKeys.Contains(kvp.Key)) {
                // create the card
                GameObject card = CreateNPCCard(kvp.Key);
                // add a zoom script
                GameObject cardParent = AddZoom(card, x, npcCenterPos, npcZoomPos);
                // set the parent
                cardParent.transform.parent = npcsParent;
                // offset x for the next card
                x += xOffset;
            }
        }
    }

    private GameObject CreateItemCard(string key) {
        GameObject card = CardFactory.CreateItemCard(key);
        return card;
    }

    private GameObject CreateNPCCard(string key) {
        GameObject card = CardFactory.CreateNPCCard(key);
        return card;
    }

    private GameObject AddZoom(GameObject card, float x, Transform centerTransform, Transform zoomTransform) {
        GameObject cardParent = new GameObject(card.name + " Parent");
        card.transform.parent = cardParent.transform;

        // create default position
        GameObject defaultPos = new GameObject("DefaultPos");
        defaultPos.transform.position = new Vector3(
            centerTransform.position.x + x,
            centerTransform.position.y,
            centerTransform.position.z
            );
        defaultPos.transform.rotation = centerTransform.rotation;

        // create zoom position
        GameObject zoomPos = new GameObject("ZoomPos");
        zoomPos.transform.position = new Vector3(
            centerTransform.position.x + x,
            zoomTransform.position.y,
            zoomTransform.position.z
            );
        zoomPos.transform.rotation = zoomTransform.rotation;

        // set the parents
        defaultPos.transform.parent = cardParent.transform;
        zoomPos.transform.parent = cardParent.transform;

        // add zoom script
        CardZoom zoom = card.AddComponent<CardZoom>();
        zoom.defaultPos = defaultPos;
        zoom.zoomPos = zoomPos;

        // start the position at its default
        card.transform.position = defaultPos.transform.position;
        card.transform.rotation = defaultPos.transform.rotation;

        return cardParent;
    }
}