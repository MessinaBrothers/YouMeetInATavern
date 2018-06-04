using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public Transform itemPosition;

    public float maxAngle;

    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        //print(Vector3.Angle(player.forward, itemPosition.transform.position - player.position));

        //if (Vector3.Angle(player.forward, itemPosition.transform.position - player.position) < maxAngle) {
        //    print("picking up item");
        //}
    }
}
