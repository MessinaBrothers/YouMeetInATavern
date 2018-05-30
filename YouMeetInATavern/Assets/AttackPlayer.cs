using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : DataUser {

    private WeaponAttack weapon;

    private NavMeshAgent agent;

    private Transform player;

    void Start() {
        weapon = GetComponentInChildren<WeaponAttack>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    void Update() {
        if (Vector3.Distance(transform.position, player.position) < weapon.range) {
            weapon.Attack();
        }

        agent.destination = player.transform.position;

        //slowly look at player
        Quaternion lookQuat = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuat, data.enemyRotSpeed * Time.deltaTime);
    }
}
