using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour {

    public static event EquipItemEventHandler equipItemEventHandler;
    public delegate void EquipItemEventHandler(int equipperID, int itemID, GameObject item);

    private GameObject heldItem;

    void Start() {

    }

    void Update() {

    }

    public void EquipItem(GameObject go) {
        // for now, destroy the currently-held item
        if (heldItem != null) {
            Destroy(heldItem);
        }

        heldItem = go;
        go.transform.parent = gameObject.transform;
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        equipItemEventHandler.Invoke(GetComponentInParent<EntityID>().id, go.GetComponentInChildren<EntityID>().id, go);
    }
}
