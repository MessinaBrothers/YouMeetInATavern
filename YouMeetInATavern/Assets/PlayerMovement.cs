using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed, rotSpeed;

    private Quaternion lookQuat;

    void Start() {

    }

    void Update() {
        // move player
        float h = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float v = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.Translate(h, 0, v, Space.World);

        // slowly took at forward direction
        if (h != 0 || v != 0) {
            lookQuat = Quaternion.LookRotation(new Vector3(h, 0, v));
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuat, rotSpeed * Time.deltaTime);
        
    }
}
