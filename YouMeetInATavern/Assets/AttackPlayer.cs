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

        ////slowly look at player
        //// look at the nearest cardinal direction
        //float heading = Quaternion.LookRotation(transform.forward).eulerAngles.y;
        //Vector3 lookAt = transform.position;
        //if (heading >= 0 + 45 && heading < 90 + 45) {
        //    // right
        //    lookAt.x += 1;
        //} else if (heading >= 90 + 45 && heading < 180 + 45) {
        //    // down
        //    lookAt.z -= 1;
        //} else if (heading >= 180 + 45 && heading < 270 + 45) {
        //    // left
        //    lookAt.x -= 1;
        //} else {
        //    // up
        //    lookAt.z += 1;
        //}

        //transform.LookAt(lookAt);

        //Quaternion lookQuat = Quaternion.LookRotation(player.position - transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuat, data.enemyRotSpeed * Time.deltaTime);
    }

    void LateUpdate() {
        ////slowly look at player
        //// look at the nearest cardinal direction
        //float heading = Quaternion.LookRotation(player.position - transform.position).eulerAngles.y;
        //Vector3 lookAt = transform.position;
        //if (heading >= 0 + 45 && heading < 90 + 45) {
        //    // right
        //    lookAt.x += 1;
        //} else if (heading >= 90 + 45 && heading < 180 + 45) {
        //    // down
        //    lookAt.z -= 1;
        //} else if (heading >= 180 + 45 && heading < 270 + 45) {
        //    // left
        //    lookAt.x -= 1;
        //} else {
        //    // up
        //    lookAt.z += 1;
        //}

        //print(heading);

        ////transform.LookAt(lookAt);

        //Quaternion lookQuat = Quaternion.LookRotation(lookAt);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuat, data.enemyRotSpeed * Time.deltaTime);
    }
}
