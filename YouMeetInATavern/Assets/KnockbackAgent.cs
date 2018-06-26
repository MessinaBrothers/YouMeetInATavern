using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackAgent : DataUser {

    public static event KockbackDoneEventHandler knockbackDoneEventHandler;
    public delegate void KockbackDoneEventHandler(KnockbackAgent knockback);

    public float time, timer;

    public Vector3 knockbackFromPosition;

    private NavMeshAgent agent;

    private Vector3 knockbackForce;

    private float originalAcceleration, originalSpeed;

    void Start() {
        time = data.enemyStunTime;
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= time) {
            knockbackDoneEventHandler.Invoke(this);
        }
    }

    void OnEnable() {
        // reset the stun timer
        timer = 0;

        agent = GetComponent<NavMeshAgent>();

        // save the original properties
        originalAcceleration = agent.acceleration;
        originalSpeed = agent.speed;

        // calculate the knockback force
        knockbackForce = gameObject.transform.position - knockbackFromPosition;
        knockbackForce = knockbackForce.normalized * data.enemyKnockbackDistance;
        knockbackForce.y = 0;

        // set the agent properties
        agent.destination = gameObject.transform.position + knockbackForce;
        agent.updateRotation = false;
        agent.acceleration = float.MaxValue;
        agent.speed = data.enemyKnockbackSpeed;
    }

    void OnDisable() {
        // set the agent original properties
        agent.enabled = true;
        agent.updateRotation = true;
        agent.acceleration = originalAcceleration;
        agent.speed = originalSpeed;
    }
}
