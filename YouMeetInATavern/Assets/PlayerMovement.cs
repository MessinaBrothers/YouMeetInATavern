using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public RandomClip jumpClips;
    
    public float moveSpeed, rotSpeed, jumpSpeed, jumpThreshold;

    private Quaternion lookQuat;
    private Rigidbody rb;

    private bool isFalling;

    void Start() {
        rb = GetComponent<Rigidbody>();
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
        
        // jump
        if (isFalling == false) {
            if (Input.GetButtonDown("Fire1")) {
                rb.AddForce(transform.up * jumpSpeed);
            }
        }

        Fall();
    }

    private void Fall() {
        float distance = 0;
        RaycastHit hit;

        Ray downRay = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(downRay, out hit)) {
            distance = hit.distance;

            if (isFalling && distance <= jumpThreshold) {
                print("Landed!");
                jumpClips.PlaySound();
                isFalling = false;
            } else {
                if (distance > jumpThreshold) {
                    isFalling = true;
                }
            }
        }
    }
}
