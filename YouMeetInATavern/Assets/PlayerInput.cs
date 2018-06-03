using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private WeaponAttack weapon;

    private int playerID;

    void Start() {
        playerID = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityID>().id;
    }

    void Update() {
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
