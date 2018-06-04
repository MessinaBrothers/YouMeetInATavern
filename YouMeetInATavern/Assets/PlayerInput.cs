using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : DataUser {

    public static event PlayerInteractEventHandler playerInteractEventHandler;
    public delegate void PlayerInteractEventHandler(GameObject interactable);

    public float interactDistance;

    private WeaponAttack weapon;

    private int playerID;

    void Start() {
        playerID = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityID>().id;
    }

    void Update() {
        if (data.debug.IS_DEBUG) {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Vector3 pos = transform.position;
            pos.y = 1;
            Debug.DrawRay(pos, fwd * interactDistance, Color.green);
        }

        if (Input.GetButtonDown("Fire1")) {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Vector3 pos = transform.position;
            pos.y = 1;
            RaycastHit hit;
            if (Physics.Raycast(pos, fwd, out hit, interactDistance)) {
                print("Player wants to interact with " + hit.collider.name);
                playerInteractEventHandler.Invoke(hit.collider.gameObject);
            }
        }
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
