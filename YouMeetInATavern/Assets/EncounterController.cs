﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterController : MonoBehaviour {

    public static event CreateBossEventHandler createBossEventHandler;
    public delegate void CreateBossEventHandler(int id);

    public float timeBetweenEnemies;

    public GameObject minionPrefab, lieutenantPrefab, bossPrefab;
    public Transform mainEntranceTransform;

    // "3.5M5L2B1" = after 3.5 seconds, spawn 5 minions, 2 lieutenants, and 1 boss
    public List<string> spawnTimes;

    void Start() {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies() {
        for (int i = 0; i < spawnTimes.Count; i++) {
            float spawnTime = float.Parse(Parse(spawnTimes[i], "", "M"));
            yield return new WaitForSeconds(spawnTime);
            int minionCount = int.Parse(Parse(spawnTimes[i], "M", "L"));
            int lieutenantCount = int.Parse(Parse(spawnTimes[i], "L", "B"));
            int bossCount = int.Parse(Parse(spawnTimes[i], "B", ""));
            Debug.LogFormat("Spawning {0} minions, {1} lieutenants, and {2} bosses", minionCount, lieutenantCount, bossCount);
            yield return CreateSpawner(minionPrefab, mainEntranceTransform, minionCount, timeBetweenEnemies, SpawnMinion);
            yield return CreateSpawner(lieutenantPrefab, mainEntranceTransform, lieutenantCount, timeBetweenEnemies, SpawnLieutenant);
            yield return CreateSpawner(bossPrefab, mainEntranceTransform, bossCount, timeBetweenEnemies, SpawnBoss);
        }
    }

    private string Parse(string s, string start, string end) {
        int startIndex = start == "" ? 0 : s.IndexOf(start) + start.Length;
        int endIndex = end == "" ? s.Length : s.IndexOf(end);
        return s.Substring(startIndex, endIndex - startIndex);
    }

    private IEnumerator CreateSpawner(GameObject prefab, Transform spawnTransform, int count, float timeBetween, Action<int> spawnEvent) {
        EnemySpawner spawner = gameObject.AddComponent<EnemySpawner>();
        spawner.enemyMinion = prefab;
        spawner.spawnTransform = spawnTransform;
        spawner.minionCount = count;
        spawner.timeBetweenSpawn = timeBetween;
        spawner.spawnEvent = spawnEvent;
        yield return new WaitForSeconds(count * timeBetween);
    }

    private void SpawnMinion(int id) {
        print("Creating a Minion with ID = " + id);
    }

    private void SpawnLieutenant(int id) {
        print("Creating a Lieutenant with ID = " + id);
    }

    private void SpawnBoss(int id) {
        createBossEventHandler.Invoke(id);
        print("Creating a Boss with ID = " + id);
    }
}
