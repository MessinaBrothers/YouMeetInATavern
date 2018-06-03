using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemIcon : MonoBehaviour {

    private Image image;

    private int playerID;

    void Start() {
        image = GetComponent<Image>();
        playerID = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityID>().id;
    }

    void OnEnable() {
        ItemSlot.equipItemEventHandler += LoadIcon;
    }

    void OnDisable() {
        ItemSlot.equipItemEventHandler -= LoadIcon;
    }

    private void LoadIcon(int equipperID, int itemID, GameObject go) {
        if (equipperID == playerID) {
            if (image == null) {
                image = GetComponent<Image>();
            }
            image.sprite = go.GetComponentInChildren<Icon>().icon;
        }
    }
}
