using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyMinion;

    public Transform spawnTransform;

    public int minionCount; // how many to spawn
    public float timeBetweenSpawn; // how long to wait between spawns

    private Transform enemyParent; // parent node for organization

    void Start() {
        // organize all enemies as children under a new GameObject
        GameObject go = new GameObject("Enemies");
        go.transform.parent = gameObject.transform;
        enemyParent = go.transform;

        // start spawning
        StartCoroutine(SpawnAll());
    }

    // void Update() { }

    // spawn all the enemies
    private IEnumerator SpawnAll() {
        for (int i = 0; i < minionCount; i++) {
            Spawn();
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    // spawn one enemy
    private void Spawn() {
        GameObject go = Instantiate(enemyMinion, spawnTransform.position, Quaternion.identity);
        go.transform.parent = enemyParent;
    }
}
