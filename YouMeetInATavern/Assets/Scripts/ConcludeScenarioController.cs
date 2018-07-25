using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcludeScenarioController : MonoBehaviour {

    public GameObject itemCardPrefab;

    public GameObject selectedItemFirst, selectedItemSecond;

    public Transform itemCenterPos, itemZoomPos;
    private Transform itemsParent;

    public AudioClip unselectClip;

    private GameData data;

    private AudioSource audioSource;

    //DEBUG
    public bool isLoad;

    void Start() {
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

                if (selectedItemFirst == null) {
                    selectedItemFirst = card;
                } else if (selectedItemSecond == null) {
                    selectedItemSecond = card;
                } else {
                    // unzoom first card
                    selectedItemFirst.GetComponent<CardZoom>().Unzoom();
                    // set second card as first
                    selectedItemFirst = selectedItemSecond;
                    // set selected card as second
                    selectedItemSecond = card;
                }
            }
        }

    }

    private void Load() {
        data = FindObjectOfType<GameData>();

        data.gameMode = GameData.GameMode.CONCLUDE;

        // DEBUG
        foreach (KeyValuePair<string, ItemData> kvp in data.itemData) {
            data.unlockedDialogueKeys.Add(kvp.Key);
        }
        //data.unlockedDialogueKeys.Add("ITEM_GOLD");
        //data.unlockedDialogueKeys.Add("ITEM_THIEFSKIT");

        float xOffset = 2.5f;
        float x = 0;

        foreach (KeyValuePair<string, ItemData> kvp in data.itemData) {
            if (data.unlockedDialogueKeys.Contains(kvp.Key)) {
                // create the card
                GameObject card = CardFactory.CreateItemCard(kvp.Key);
                card.transform.position = itemCenterPos.position;
                card.transform.rotation = itemCenterPos.rotation;

                GameObject cardParent = new GameObject(card.name + " Parent");
                card.transform.parent = cardParent.transform;

                // create zoom positions
                GameObject defaultPos = new GameObject("DefaultPos");
                defaultPos.transform.position = new Vector3(
                    itemCenterPos.position.x + x,
                    itemCenterPos.position.y,
                    itemCenterPos.position.z
                    );
                defaultPos.transform.parent = cardParent.transform;
                GameObject zoomPos = new GameObject("ZoomPos");
                zoomPos.transform.position = new Vector3(
                    itemCenterPos.position.x + x,
                    itemZoomPos.position.y,
                    itemZoomPos.position.z
                    );
                zoomPos.transform.parent = cardParent.transform;

                // add zoom script
                CardZoom zoom = card.AddComponent<CardZoom>();
                zoom.defaultPos = defaultPos;
                zoom.zoomPos = zoomPos;

                // start the position at its default
                card.transform.position = defaultPos.transform.position;

                x += xOffset;
            }
        }
    }
}