using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour {

    public static event EquipItemEventHandler equipItemEventHandler;
    public delegate void EquipItemEventHandler(int equipperID, int itemID);

    private GameObject heldItem;

    void Start() {

    }

    void Update() {

    }

    public void EquipItem(GameObject go) {
        heldItem = go;
        go.transform.parent = gameObject.transform;
    }
}
