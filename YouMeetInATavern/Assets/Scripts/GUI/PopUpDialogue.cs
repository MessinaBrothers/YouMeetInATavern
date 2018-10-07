using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDialogue : MonoBehaviour {

    public float lifetime;
    public float minSize, maxSize;

    private GameData data;

    private float timer;

    void Start() {
        data = FindObjectOfType<GameData>();

        timer = 0;

        SetSize(minSize);
    }
    
    public void SetText(string s) {
        GetComponentInChildren<Text>().text = s;
    }

    void Update() {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) {
            Destroy(gameObject);
        } else if (timer < data.growTime) {
            SetSize(Mathf.Lerp(minSize, maxSize, timer / data.growTime));
        } else if (timer > lifetime + data.shrinkTime) {
            Destroy(gameObject);
        } else if (timer > lifetime) {
            SetSize(Mathf.Lerp(maxSize, minSize, (timer - lifetime) / data.shrinkTime));
        } else {
            SetSize(maxSize);
        }
    }

    private void SetSize(float v) {
        v = Mathf.Clamp(v, minSize, maxSize);
        transform.localScale = new Vector3(v, v, v);
    }

}
