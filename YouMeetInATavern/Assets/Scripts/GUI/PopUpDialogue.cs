using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDialogue : MonoBehaviour {

    public float lifetime;
    
    void Start() {

    }

    void Update() {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0 || Input.GetMouseButtonDown(0)) {
            Destroy(gameObject);
        }
    }

    public void SetText(string s) {
        GetComponentInChildren<Text>().text = s;
    }
}
