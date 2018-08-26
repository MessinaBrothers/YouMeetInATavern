using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGProfiler : MonoBehaviour {

    private DateTime startTime;

    void Awake() {
        startTime = DateTime.Now;
    }

    void Start() {

    }

    void Update() {

    }

    public void EndLoadProfiler() {
        Debug.LogFormat("Game load time: {0:0.} ms", (DateTime.Now - startTime).TotalMilliseconds);
    }
}
