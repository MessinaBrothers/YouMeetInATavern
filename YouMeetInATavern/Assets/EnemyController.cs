using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : DataUser {

    private AttackPlayer attackPlayer;

    private NavMeshAgent agent;
    private new Collider collider;
    private Rigidbody rb;

    private int id;

    void Start() {
        attackPlayer = gameObject.AddComponent<AttackPlayer>();

        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        id = GetComponentInParent<EntityID>().id;

        agent.speed = data.enemyMoveSpeed;
        agent.stoppingDistance = data.enemyReachDistance;
    }

    void Update() {

    }

    private void Die(int id) {
        if (this.id == id) {
            attackPlayer.enabled = false;
            collider.enabled = false;
            rb.isKinematic = true;
            agent.enabled = false;
        }
    }

    void OnEnable() {
        Health.deathEventHandler += Die;
    }

    void OnDisable() {
        Health.deathEventHandler -= Die;
    }
}
