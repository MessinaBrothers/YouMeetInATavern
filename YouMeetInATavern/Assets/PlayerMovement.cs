using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public RandomClip landClips, jumpClips;
    
    public float moveSpeed, rotSpeed, jumpSpeed, jumpThreshold;

    private Quaternion lookQuat;
    private Rigidbody rb;

    private bool isFalling, isJumping;

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
            if (Input.GetButtonDown("Jump")) {
                rb.AddForce(transform.up * jumpSpeed);
                jumpClips.PlaySound();
                isJumping = true;
            }
        }

        Fall();
    }

    private void Fall() {
        if (isJumping == false) return;

        float distance = 0;
        RaycastHit hit;

        Vector3 origin = transform.position;
        origin.y += jumpThreshold;

        Ray downRay = new Ray(origin, -Vector3.up);
        if (Physics.Raycast(downRay, out hit)) {
            distance = hit.distance;
            if (isFalling && distance <= jumpThreshold * 2) {
                //landClips.PlaySound();
                isFalling = false;
                isJumping = false;
            } else {
                if (distance > jumpThreshold * 2) {
                    isFalling = true;
                }
            }
        }
    }
}
