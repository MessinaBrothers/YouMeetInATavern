using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    void Awake() {
        foreach (MyTest test in GetComponentsInChildren<MyTest>()) {
            print(test.StartTests());
        }

        DestroyImmediate(gameObject);
    }

    void Update() {

    }
}
