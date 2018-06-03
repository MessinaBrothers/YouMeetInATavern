using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour {

    private int playerID;
    private int bossID;

    void Start() {
        playerID = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityID>().id;
    }

    void Update() {

    }

    private void CheckDeath(int id) {
        if (id == playerID) {
            SceneManager.LoadScene("LoseScreen");
        } else if (id == bossID) {
            SceneManager.LoadScene("WinScreen");
        }
    }

    private void BossCreated(int id) {
        bossID = id;
    }

    void OnEnable() {
        Health.deathEventHandler += CheckDeath;
        EncounterController.createBossEventHandler += BossCreated;
    }

    void OnDisable() {
        Health.deathEventHandler -= CheckDeath;
        EncounterController.createBossEventHandler -= BossCreated;
    }
}
