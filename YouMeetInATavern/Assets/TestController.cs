using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    void Awake() {
        string results = "\n";

        foreach (MyTest test in GetComponentsInChildren<MyTest>()) {
            results += test.StartTests() + "\n";
        }

        Debug.LogFormat("Test Results: {0}\n{1}", results.Contains("fail") ? "FAILED" : "OK", results);

        DestroyImmediate(gameObject);
    }

    void Update() {

    }
}
