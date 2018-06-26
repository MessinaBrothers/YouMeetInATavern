using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : DataUser {

    private AttackPlayer attackPlayer;
    private KnockbackAgent knockback;

    private NavMeshAgent agent;
    private new Collider collider;
    private Rigidbody rb;

    private int id;
    private bool isDead;

    void Start() {
        attackPlayer = gameObject.AddComponent<AttackPlayer>();
        knockback = gameObject.AddComponent<KnockbackAgent>();
        knockback.enabled = false;

        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        id = GetComponentInParent<EntityID>().id;
        isDead = false;

        // set the navmesh agent properties from game data
        agent.speed = data.enemyMoveSpeed;
        agent.stoppingDistance = data.enemyReachDistance;
    }

    void Update() {

    }

    void OnEnable() {
        Health.deathEventHandler += Die;
        WeaponHitDetect.weaponHitEventHandler += GetHit;
        KnockbackAgent.knockbackDoneEventHandler += FinishKnockback;
    }

    void OnDisable() {
        Health.deathEventHandler -= Die;
        WeaponHitDetect.weaponHitEventHandler -= GetHit;
        KnockbackAgent.knockbackDoneEventHandler -= FinishKnockback;
    }

    private void FinishKnockback(KnockbackAgent knockback) {
        if (this.knockback == knockback) {
            attackPlayer.enabled = true;
            knockback.enabled = false;
        }
    }

    private void Die(int id) {
        if (this.id == id) {
            attackPlayer.enabled = false;
            knockback.enabled = false;
            collider.enabled = false;
            rb.isKinematic = true;
            agent.enabled = false;

            isDead = true;
        }
    }

    private void GetHit(int attackWeaponID, GameObject weaponObject, GameObject hitObject) {
        if (hitObject == gameObject && isDead == false) {
            knockback.knockbackFromPosition = weaponObject.transform.position;
            attackPlayer.enabled = false;
            knockback.enabled = true;
        }
    }
}
