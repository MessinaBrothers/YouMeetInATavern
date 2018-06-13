﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : DataUser {

    public static event PlayerInteractEventHandler playerInteractEventHandler;
    public delegate void PlayerInteractEventHandler(GameObject interactable);

    public static event PlayerInspectEventHandler playerInspectEventHandler;
    public delegate void PlayerInspectEventHandler(GameObject inspectable);

    public static event TextContinueEventHandler textContinueEventHandler;
    public delegate void TextContinueEventHandler();

    public float interactDistance;

    private Transform player;
    private WeaponAttack weapon;

    private int playerID;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerID = player.GetComponent<EntityID>().id;
        gameObject.name = "asdf;";
    }

    void Update() {
        if (data.debug.IS_DEBUG) {
            Vector3 fwd = player.transform.TransformDirection(Vector3.forward);
            Vector3 pos = player.transform.position;
            pos.y = 1;
            Debug.DrawRay(pos, fwd * interactDistance, Color.green);
        }

        // if game is paused
        if (Time.timeScale == 0) {
            // inspect
            if (Input.GetButtonDown("Fire2")) {
                textContinueEventHandler.Invoke();
            }
            return;
        }

        // interact
        if (Input.GetButtonDown("Fire1")) {
            Vector3 fwd = player.transform.TransformDirection(Vector3.forward);
            Vector3 pos = player.transform.position;
            pos.y = 1;
            RaycastHit hit;
            if (Physics.Raycast(pos, fwd, out hit, interactDistance)) {
                print("Player wants to interact with " + hit.collider.name);
                playerInteractEventHandler.Invoke(hit.collider.gameObject);
            }
        }

        // inspect
        if (Input.GetButtonDown("Fire2")) {
            Vector3 fwd = player.transform.TransformDirection(Vector3.forward);
            Vector3 pos = player.transform.position;
            pos.y = 1;
            RaycastHit hit;
            if (Physics.Raycast(pos, fwd, out hit, interactDistance)) {
                print("Player wants to inspect " + hit.collider.name);
                playerInspectEventHandler.Invoke(hit.collider.gameObject);
            }
        }

        // attack
        if (Input.GetButtonDown("Fire3")) {
            weapon.Attack();
        }
    }

    void OnEnable() {
        ItemSlot.equipItemEventHandler += LoadWeapon;
    }

    void OnDisable() {
        ItemSlot.equipItemEventHandler -= LoadWeapon;
    }

    private void LoadWeapon(int equipperID, int itemID, GameObject go) {
        if (equipperID == playerID) {
            weapon = go.GetComponentInChildren<WeaponAttack>();
        }
    }
}
