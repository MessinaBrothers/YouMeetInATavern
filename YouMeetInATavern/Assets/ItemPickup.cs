using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public GameObject itemToEquipPrefab;
    public GameObject modelToRemove, glowToRemove;
    public Transform itemPosition;

    public bool hasItemToPickup;
    public float maxAngle;

    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hasItemToPickup = true;
    }

    void Update() {
        //print(Vector3.Angle(player.forward, itemPosition.transform.position - player.position));

        //if (Vector3.Angle(player.forward, itemPosition.transform.position - player.position) < maxAngle) {
        //    print("picking up item");
        //}
    }

    void OnEnable() {
        PlayerInput.playerInteractEventHandler += Pickup;
    }

    void OnDisable() {
        PlayerInput.playerInteractEventHandler -= Pickup;
    }

    private void Pickup(GameObject interactable) {
        if (hasItemToPickup == true && interactable == gameObject) {
            hasItemToPickup = false;
            print("interacting with me, " + gameObject.name);
            player.gameObject.GetComponentInChildren<ItemSlot>().EquipItem(Instantiate(itemToEquipPrefab));
            modelToRemove.SetActive(false);
            glowToRemove.SetActive(false);
        }
    }
}
