using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior : MonoBehaviour {

    public float moveSpeed;

    public Transform target;

    void Start() {

    }

    void Update() {
        if (target != null) {
            // move to target
            transform.Translate((target.position - transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}