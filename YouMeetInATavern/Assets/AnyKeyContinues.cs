﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyContinues : MonoBehaviour {

    void Start() {

    }

    void Update() {
        if (Input.anyKey) {
            SceneManager.LoadScene("Main");
        }
    }
}
