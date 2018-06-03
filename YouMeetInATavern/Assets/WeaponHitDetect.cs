using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitDetect : MonoBehaviour {

    public static event WeaponHitEventHandler weaponHitEventHandler;
    public delegate void WeaponHitEventHandler(int attackWeaponID, GameObject weaponObject, GameObject hitObject);

    private GameObject weapon;
    private new Collider collider;

    // allows one hit per object per enable
    // to allow new hits, clear the list (usually via Restart)
    private List<GameObject> hitObjects;

    private int id;

    void Awake() {
        EntityID entityID = GetComponentInParent<EntityID>();
        weapon = entityID.gameObject;
        id = entityID.id;

        collider = GetComponent<Collider>();
        hitObjects = new List<GameObject>();
    }

    void Update() {
        // allows collisions to trigger on enable
        collider.enabled = true;
    }

    public void Restart() {
        collider.enabled = false;
        enabled = true;
        hitObjects.Clear();
    }

    void OnTriggerEnter(Collider other) {
        if (enabled == true) {
            if (hitObjects.Contains(other.gameObject) == false) {
                hitObjects.Add(other.gameObject);
                weaponHitEventHandler.Invoke(id, weapon, other.gameObject);
            }
        }
    }

    void OnEnable() { }

    void OnDisable() {
        collider.enabled = false;
    }
}
