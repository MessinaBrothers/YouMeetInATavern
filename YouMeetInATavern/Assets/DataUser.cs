using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUser : MonoBehaviour {

    protected GameData data;

    void Awake() {
        data = GameObject.FindObjectOfType<GameData>();
    }

}
