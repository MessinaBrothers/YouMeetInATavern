using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior : DataUser {

    public Transform target;

    void Start() {

    }

    void Update() {
        if (target != null) {
            // move to target
            transform.Translate((target.position - transform.position).normalized * data.enemyMoveSpeed * Time.deltaTime, Space.World);
        }
    }
}