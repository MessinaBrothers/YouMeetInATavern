using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour {

    public GameObject defaultPos, zoomPos;

    private float timer, time;

    void Start() {

    }

    void Update() {

    }

    public void Zoom() {
        gameObject.transform.position = zoomPos.transform.position;
    }
    
    public void Unzoom() {
        gameObject.transform.position = defaultPos.transform.position;
    }
}
