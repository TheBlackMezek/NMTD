using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public GameObject enemySpawner;
    public GameObject enemyPrefab;
    public GameObject enemyTarget;

    public float spawnInterval = 1.0f;
    public int startingWaveEnemies = 5;

    private int enemiesInWave;
    private float timer = 0;


    private void Start()
    {
        enemiesInWave = startingWaveEnemies;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= spawnInterval)
        {
            timer = 0;

            if(enemiesInWave > 0)
            {
                GameObject e = Instantiate(enemyPrefab);
                e.transform.position = enemySpawner.transform.position;
                e.GetComponent<EnemyController>().target = enemyTarget;
                NetworkServer.Spawn(e);
                --enemiesInWave;
            }
        }
    }

}
