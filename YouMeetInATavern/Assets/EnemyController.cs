using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private AttackPlayer attackPlayer;

    private int id;

    void Start() {
        attackPlayer = gameObject.AddComponent<AttackPlayer>();

        id = GetComponentInParent<EntityID>().id;
    }

    void Update() {

    }

    private void Die(int id) {
        if (this.id == id) {
            attackPlayer.enabled = false;
        }
    }

    void OnEnable() {
        Health.deathEventHandler += Die;
    }

    void OnDisable() {
        Health.deathEventHandler -= Die;
    }
}
