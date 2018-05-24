using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitDetect : MonoBehaviour {

    private new Collider collider;

    private List<GameObject> hitObjects;

    void Awake() {
        collider = GetComponent<Collider>();
        hitObjects = new List<GameObject>();
    }

    void Update() {
        // allows collisions to trigger when on enable
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
                print(other.name);
            }
        }
    }

    void OnEnable() { }

    void OnDisable() {
        collider.enabled = false;
    }
}
