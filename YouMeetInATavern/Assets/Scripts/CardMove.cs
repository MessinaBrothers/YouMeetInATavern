﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMove : MonoBehaviour {

    // moves a card from one transform to another in x time
    // upon completion, calls a generic delegate

    private System.Action<GameObject> reachedPosition;
    
    private Transform startTransform, endTransform;

    private float moveTime, moveTimer;

    void Awake() {
        GameObject go = new GameObject("CardMove_TempTransform_" + gameObject.name);
        startTransform = go.transform;
        startTransform.parent = GameObject.Find("MoveTransforms").transform;
    }

    void Update() {
        if (moveTimer > 0) {
            moveTimer -= Time.deltaTime;
            //Debug.LogFormat("{0:0.000} / {1:0.000} = {2:0.000}", moveTimer, moveTime, moveTimer / moveTime);
            transform.position = Vector3.Slerp(endTransform.position, startTransform.position, moveTimer / moveTime);
            transform.rotation = Quaternion.Slerp(endTransform.rotation, startTransform.rotation, moveTimer / moveTime);

            if (transform.position == endTransform.position) {
                //print("Reached position!");
                reachedPosition(gameObject);
            }
        }
    }

    public void Set(Transform startTransform, Transform endTransform, float time, System.Action<GameObject> reachedPosition) {
        this.startTransform.position = startTransform.position;
        this.startTransform.rotation = startTransform.rotation;

        this.endTransform = endTransform;
        moveTime = time;
        this.reachedPosition = reachedPosition;
        moveTimer = moveTime;
    }
}
