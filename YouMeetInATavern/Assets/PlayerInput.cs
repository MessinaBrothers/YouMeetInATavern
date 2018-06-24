using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MyInput {

    public static event PlayerInteractEventHandler playerInteractEventHandler;
    public delegate void PlayerInteractEventHandler(GameObject interactable);

    public static event PlayerInspectEventHandler playerInspectEventHandler;
    public delegate void PlayerInspectEventHandler(GameObject inspectable);

    public static event TextContinueEventHandler textContinueEventHandler;
    public delegate void TextContinueEventHandler();

    public float interactDistance;

    private GameObject player;
    private WeaponAttack weapon;

    private int playerID;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerID = player.GetComponent<EntityID>().id;
    }

    void Update() {
        if (data.debug.IS_DEBUG) {
            Vector3 fwd = player.transform.TransformDirection(Vector3.forward);
            Vector3 pos = player.transform.position;
            pos.y = 1;
            Debug.DrawRay(pos, fwd * interactDistance, Color.green);
        }
        // save the attack data for player movement
        if (weapon != null) data.isAttacking = !weapon.canAttack;
    }

    void OnEnable() {
        ItemSlot.equipItemEventHandler += EquipItem;
    }

    void OnDisable() {
        ItemSlot.equipItemEventHandler -= EquipItem;
    }

    public override void Handle(string input) {
        // if game is paused
        if (Time.timeScale == 0) {
            // inspect
            if (input == "Fire2") {
                textContinueEventHandler.Invoke();
            }
            return;
        }

        // interact
        if (input == "Fire1") {
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
        if (input == "Fire2") {
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
        if (input == "Fire3") {
            weapon.Attack();
        }
    }

    private void EquipItem(int equipperID, int itemID, GameObject go) {
        print("Loading weapon");
        if (equipperID == playerID) {
            weapon = go.GetComponentInChildren<WeaponAttack>();
        }
    }
}
