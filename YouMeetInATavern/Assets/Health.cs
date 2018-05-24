using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int current, max;

    private Text text;

    void Start() {
        current = max;

        text = GetComponentInChildren<Text>();
    }

    void Update() {
        // always orient up
        // http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.up, Camera.main.transform.rotation * Vector3.up);

    }

    private void LateUpdate() {
        text.text = string.Format("{0:0.}", current);
    }
}
