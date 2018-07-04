using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour {

    private int id;

    void Start() {
        id = GetComponentInParent<EntityID>().id;
    }

    private void Die(int id) {
        if (this.id == id) {
            GetComponentInChildren<Animator>().SetInteger("Health", 0);
            enabled = false;
        }
    }
    
    void OnEnable() {
        Health.deathEventHandler += Die;
    }

    void OnDisable() {
        Health.deathEventHandler -= Die;
    }
}
