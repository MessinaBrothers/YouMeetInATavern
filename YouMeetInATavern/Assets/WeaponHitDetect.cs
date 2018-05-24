using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitDetect : MonoBehaviour {

    private List<GameObject> hitObjects;

    void Start() {
        hitObjects = new List<GameObject>();
    }

    void Update() {

    }

    public void Restart() {
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
}
