using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour {

    private int playerID;

    void Start() {
        playerID = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityID>().id;
    }

    void Update() {

    }

    private void CheckDeath(int id) {
        if (id == playerID) {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void OnEnable() {
        Health.deathEventHandler += CheckDeath;
    }

    void OnDisable() {
        Health.deathEventHandler -= CheckDeath;
    }
}
