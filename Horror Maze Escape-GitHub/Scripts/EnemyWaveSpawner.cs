using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public Transform[] patrolWaypoints;
    public int startEnemies = 1;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool waveActive = false;

    private List<WaveTrigger> registeredTriggers = new List<WaveTrigger>();

    public void RegisterTrigger(WaveTrigger trigger)
    {
        if (!registeredTriggers.Contains(trigger))
        {
            registeredTriggers.Add(trigger);
        }
    }

    public void StartWave()
    {
        if (!waveActive)
        {
            StartCoroutine(SpawnWave());

            // Destroy all connected triggers so they don't re-trigger
            foreach (WaveTrigger trigger in registeredTriggers)
            {
                if (trigger != null)
                    trigger.DestroyTrigger();
            }

            registeredTriggers.Clear(); // Optional cleanup
        }
    }

    IEnumerator SpawnWave()
    {
        waveActive = true;
        currentWave++;
        int enemiesToSpawn = startEnemies;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null && patrolWaypoints.Length > 0)
        {
            enemyController.waypoints = patrolWaypoints;
        }

        enemy.GetComponent<EnemyHealth>().OnDeath += EnemyDefeated;
        enemiesAlive++;
    }

    void EnemyDefeated()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            waveActive = false;
        }
    }
}
