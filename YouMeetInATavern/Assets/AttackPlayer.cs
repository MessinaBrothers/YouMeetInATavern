﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : DataUser {

    private SteeringBehavior steering;

    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        steering = GetComponent<SteeringBehavior>();
    }

    void Update() {
        // if distance to player is greater that reach distance
        if ((transform.position - player.position).sqrMagnitude > data.enemyReachDistance * data.enemyReachDistance) {
            // set the steering behaviour target
            steering.target = player;
        } else {
            steering.target = null;
        }

        //slowly look at player
        Quaternion lookQuat = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuat, data.enemyRotSpeed * Time.deltaTime);
    }
}
