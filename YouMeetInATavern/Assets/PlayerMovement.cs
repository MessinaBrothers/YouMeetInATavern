using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : DataUser {

    public RandomClip landClips, jumpClips;
    
    public float moveSpeed, rotSpeed, jumpSpeed, jumpThreshold;
    public float moveVelocityMin, rotateMin, velocityReduction;

    private Quaternion lookQuat;
    private Rigidbody rb;

    private bool isFalling, isJumping;

    private Vector3 lastVelocity;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(float h, float v) {
        if (data.canMove == false) {
            lastVelocity = Vector3.zero;
            return;
        }

        h *= moveSpeed * Time.deltaTime;
        v *= moveSpeed * Time.deltaTime;
        Vector3 velocity = new Vector3(h, 0, v);
        // if the velocity is greater than the min speed needed to move
        if (velocity.sqrMagnitude > moveVelocityMin * moveVelocityMin) {
            // move the player by its speed
            transform.Translate(velocity, Space.World);
            // save the speed
            lastVelocity = velocity;
            // if not moving and residual speed remains
        } else if (lastVelocity.sqrMagnitude > 0.0001f) {
            // move the player the residual speed
            transform.Translate(lastVelocity, Space.World);
            // reduce the residual speed
            lastVelocity -= lastVelocity * velocityReduction * Time.deltaTime;
        }
        //if (velocity.magnitude > moveSpeed) {
        //    //velocity = velocity.normalized * moveSpeed;
        //}

        // slowly took at forward direction
        if (velocity.sqrMagnitude > rotateMin * rotateMin) {
            lookQuat = Quaternion.LookRotation(new Vector3(h, 0, v));
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuat, rotSpeed * Time.deltaTime);
    }

    public void Jump() {
        if (isFalling == false) {
            rb.AddForce(transform.up * jumpSpeed);
            jumpClips.PlaySound();
            isJumping = true;
        }
    }

    void Update() {
        if (Time.timeScale == 0) {
            return;
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
