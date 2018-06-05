using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInput : MonoBehaviour {

    void Start() {

    }

    void Update() {
        if (Input.GetButtonDown("Start")) {
            SceneManager.LoadScene("Main");
        }
    }
}
