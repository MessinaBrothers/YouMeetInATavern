using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityID : MonoBehaviour {

    private static int NEXT_ID = 0;

    public int id;

    void Awake() {
        id = NEXT_ID;
        NEXT_ID += 1;
    }

    void Update() {

    }
}
