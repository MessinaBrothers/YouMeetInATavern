using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUGUnlockCardsGUI : MonoBehaviour {

    public GameObject itemsPanel, npcsPanel;
    public GameObject buttonPrefab;

    //private GameData data;

    internal void Load(GameData data) {
        //this.data = data;

        foreach (string key in data.cardData.Keys) {
            // create a button
            GameObject button = Instantiate(buttonPrefab);
            button.name = "BUTTON_" + key;
            button.GetComponentInChildren<Text>().text = key;

            // add a listener to unlock the key in GameData
            button.GetComponent<Button>().onClick.AddListener(delegate { Unlock(key); });

            // split buttons into items and npcs panels
            if (key.StartsWith("ITEM_")) {
                button.transform.SetParent(itemsPanel.transform);
            } else {
                button.transform.SetParent(npcsPanel.transform);
            }
        }

        Destroy(buttonPrefab);
    }

    private void Unlock(string key) {
        if (DeckController.Contains(key) == false) {
            DeckController.Add(key);
        }
    }
}
