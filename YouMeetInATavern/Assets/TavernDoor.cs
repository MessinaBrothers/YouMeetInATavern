using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernDoor : MonoBehaviour {

    public static event LeaveBarEventHandler leaveBarEventHandler;
    public delegate void LeaveBarEventHandler();

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (player == other.gameObject) {
            leaveBarEventHandler.Invoke();
        }
    }
}
