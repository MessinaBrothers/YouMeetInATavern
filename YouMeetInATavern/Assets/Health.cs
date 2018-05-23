using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    void Start() {

    }

    void Update() {
        // always orient up
        // http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.up, Camera.main.transform.rotation * Vector3.up);
    }
}
