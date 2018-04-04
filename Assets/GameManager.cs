using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public GameObject enemySpawner;
    public GameObject enemyPrefab;
    public GameObject enemyTarget;

    public float spawnInterval = 1.0f;
    public float spawnSpeedup = 0.9f;
    public int startingWaveEnemies = 5;
    public float waveEnemyIncrease = 1.1f;
    public float timeBetweenWaves = 5.0f;

    private int lastWaveEnemies;
    private int enemiesInWave;
    private float timer = 0;
    private int wave = 0;


    private void Start()
    {
        enemiesInWave = startingWaveEnemies;
        lastWaveEnemies = enemiesInWave;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if(enemiesInWave <= 0 && timer >= timeBetweenWaves)
        {
            timer = 0;
            lastWaveEnemies = (int)(lastWaveEnemies * waveEnemyIncrease);
            enemiesInWave = lastWaveEnemies;
            spawnInterval *= spawnSpeedup;
        }
        else if(enemiesInWave > 0 && timer >= spawnInterval)
        {
            timer = 0;
            
            GameObject e = Instantiate(enemyPrefab);
            e.transform.position = enemySpawner.transform.position;
            e.GetComponent<EnemyController>().target = enemyTarget;
            NetworkServer.Spawn(e);
            --enemiesInWave;
        }
    }

}
