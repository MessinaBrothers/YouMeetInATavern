using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private AttackPlayer attackPlayer;

    private new Collider collider;
    private Rigidbody rb;

    private int id;

    void Start() {
        attackPlayer = gameObject.AddComponent<AttackPlayer>();

        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        id = GetComponentInParent<EntityID>().id;
    }

    void Update() {

    }

    private void Die(int id) {
        if (this.id == id) {
            attackPlayer.enabled = false;
            collider.enabled = false;
            rb.isKinematic = true;
        }
    }

    void OnEnable() {
        Health.deathEventHandler += Die;
    }

    void OnDisable() {
        Health.deathEventHandler -= Die;
    }
}
