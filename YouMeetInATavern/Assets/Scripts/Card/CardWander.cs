﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWander : MonoBehaviour {

    public Transform[] quads;

    public float speed;

    private Vector3 wanderPosition;
    private Vector3 startPos, endPos;

    private float timer, maxTime;

    private float waitTime = 2f;

    private enum State {
        PICK_POSITION, MOVE_POSITION, WAIT, WANDER
    }
    private State state;

    void Start() {

    }

    void Update() {
        switch (state) {
            case State.PICK_POSITION:
                // randomly determing a point to wander around
                wanderPosition = GetRandomPosition(quads);

                // set start and end positions
                startPos = transform.position;
                endPos = wanderPosition;

                // calculate time based on speed
                maxTime = Vector3.Distance(startPos, endPos) / speed;
                timer = maxTime;

                state = State.MOVE_POSITION;
                break;
            case State.MOVE_POSITION:
                timer -= Time.deltaTime;
                // lerp from start position to end position
                transform.position = Vector3.Lerp(endPos, startPos, timer / maxTime);
                // keep y position constant
                transform.position = new Vector3(transform.position.x, endPos.y, transform.position.z);
                // if card is at end position
                if (timer <= 0) {
                    timer = waitTime;
                    state = State.WAIT;
                }
                break;
            case State.WAIT:
                timer -= Time.deltaTime;
                // if card is done waiting
                if (timer <= 0) {
                    state = State.PICK_POSITION;
                }
                break;
            case State.WANDER:
                //TODO implement random wandering X amount of times
                break;
        }
    }

    void OnEnable() {
        state = State.WAIT;
        timer = waitTime;
    }

    private Vector3 GetRandomPosition(Transform[] quads) {
        Transform quad = quads[Random.Range(0, quads.Length)];

        return new Vector3(
            quad.position.x + Random.Range(-quad.localScale.x / 2, quad.localScale.x / 2),
            transform.position.y,
            quad.position.z + Random.Range(-quad.localScale.y / 2, quad.localScale.y / 2)
        );
    }
}
