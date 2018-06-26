using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKnockback : MyInput {
    
    public static event KnockbackDoneEventHandler knockbackDoneEventHandler;
    public delegate void KnockbackDoneEventHandler();

    private GameObject player;
    private Vector3 originalPlayerPos, destination;

    private float time, timer;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        time = data.playerStunTime;
        timer = time;
    }

    void Update() {
        if (timer < time) {
            timer += Time.deltaTime;

            // lerp between original position and destination
            player.transform.position = Vector3.Lerp(originalPlayerPos, destination, timer / time);

            if (timer >= time) {
                knockbackDoneEventHandler.Invoke();
            }
        }
    }

    public override void Handle(string input) {

    }

    public override void Handle(float h, float v) {
        
    }

    public void Knockback(Vector3 knockbackFromPosition) {
        timer = 0f;
        originalPlayerPos = player.transform.position;

        // calculate the knockback force
        Vector3 knockbackForce = player.transform.position - knockbackFromPosition;
        knockbackForce = knockbackForce.normalized * data.playerKnockbackDistance;
        knockbackForce.y = 0;

        destination = originalPlayerPos + knockbackForce;
    }

    void OnEnable() {

    }

    void OnDisable() {

    }
}
