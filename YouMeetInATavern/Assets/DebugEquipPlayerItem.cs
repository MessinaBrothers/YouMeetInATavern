using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEquipPlayerItem : MonoBehaviour {

    public GameObject[] items;

    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            EquipSlot(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            EquipSlot(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            EquipSlot(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            EquipSlot(4);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            EquipSlot(5);
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            EquipSlot(6);
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            EquipSlot(7);
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            EquipSlot(8);
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            EquipSlot(9);
        } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            EquipSlot(10);
        }
    }

    private void EquipSlot(int slot) {
        int itemIndex = slot - 1;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<ItemSlot>().EquipItem(Instantiate(items[itemIndex]));
    }
}
