using AuraAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcludeScenarioController : MonoBehaviour {

    public GameObject itemCardPrefab, tavern;

    public GameObject selectedItemFirst, selectedItemSecond;

    public Transform itemCenterPos, itemZoomPos, npcCenterPos, npcZoomPos;
    private Transform itemsParent;

    public AudioClip unselectClip;

    private GameData data;

    private AudioSource audioSource;

    //DEBUG
    public bool isLoad;

    void Start() {
        data = FindObjectOfType<GameData>();

        GameObject go = new GameObject("Items");
        itemsParent = go.transform;
        itemsParent.parent = gameObject.transform;

        audioSource = GetComponent<AudioSource>();
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

        CardZoom selectedZoom = card.GetComponent<CardZoom>();

        if (card.GetComponent<NPC>() != null) {
            print("User clicked on an NPC");
        } else {
            // check if card is already selected
            if (selectedItemFirst == card || selectedItemSecond == card) {
                // unzoom selected card
                selectedZoom.Unzoom();
                // play unselect sound
                audioSource.PlayOneShot(unselectClip);
                // highlight the card
                card.GetComponentInChildren<AuraVolume>().enabled = false;

                // move selected choices (first and second)
                if (selectedItemFirst == card) {
                    // second becomes first
                    selectedItemFirst = selectedItemSecond;
                }
                // clear second
                selectedItemSecond = null;
            } else {
                // bring card forward
                selectedZoom.Zoom();
                // play sound effect
                card.GetComponent<CardSFX>().PlayGreeting();
                // highlight the card
                card.GetComponentInChildren<AuraVolume>().enabled = true;

                if (selectedItemFirst == null) {
                    selectedItemFirst = card;
                } else if (selectedItemSecond == null) {
                    selectedItemSecond = card;
                } else {
                    // unzoom first card
                    selectedItemFirst.GetComponent<CardZoom>().Unzoom();
                    selectedItemFirst.GetComponentInChildren<AuraVolume>().enabled = false;
                    // set second card as first
                    selectedItemFirst = selectedItemSecond;
                    // set selected card as second
                    selectedItemSecond = card;
                }
            }
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
                AddZoom(card, x, itemCenterPos, itemZoomPos);
                // offset x for the next card
                x += xOffset;
            }
        }
        // NPCs
        //foreach (KeyValuePair<string, NPCData> kvp in data.npcData) {
        //    if (data.unlockedDialogueKeys.Contains(kvp.Key)) {
        //        // create the card
        //        GameObject card = CreateItemCard(kvp.Key);
        //        // add a zoom script
        //        AddZoom(card, x, itemCenterPos, itemZoomPos);
        //        // offset x for the next card
        //        x += xOffset;
        //    }
        //}
    }

    private GameObject CreateItemCard(string key) {
        GameObject card = CardFactory.CreateItemCard(key);
        card.transform.position = itemCenterPos.position;
        card.transform.rotation = itemCenterPos.rotation;

        card.transform.parent = itemsParent;

        return card;
    }

    private CardZoom AddZoom(GameObject card, float x, Transform centerTransform, Transform zoomTransform) {
        GameObject cardParent = new GameObject(card.name + " Parent");
        card.transform.parent = cardParent.transform;

        // create zoom positions
        GameObject defaultPos = new GameObject("DefaultPos");
        defaultPos.transform.position = new Vector3(
            centerTransform.position.x + x,
            centerTransform.position.y,
            centerTransform.position.z
            );
        GameObject zoomPos = new GameObject("ZoomPos");
        zoomPos.transform.position = new Vector3(
            centerTransform.position.x + x,
            zoomTransform.position.y,
            zoomTransform.position.z
            );

        // set the parents
        defaultPos.transform.parent = cardParent.transform;
        zoomPos.transform.parent = cardParent.transform;

        // add zoom script
        CardZoom zoom = card.AddComponent<CardZoom>();
        zoom.defaultPos = defaultPos;
        zoom.zoomPos = zoomPos;

        // start the position at its default
        card.transform.position = defaultPos.transform.position;

        return zoom;
    }
}