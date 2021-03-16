﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private List<WaveManager> _waveManagement;
    [SerializeField] private GameObject _enemy, spawningContainer, _enemyLeft, _enemyRight;
    [SerializeField] private GameObject[] powerups;
    Coroutine spawningEnemy, spawningPowerUps, spawningPathed, spawningRight;
    private bool keepSpawning = false;
    void Start()
    {
    }

    IEnumerator SpawnRoutineEnemy()
    {
        yield return new WaitForSeconds(2F);
        while (keepSpawning)
        {
            
            GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-7, 4), 9, 0), Quaternion.identity);
            GameObject newEnemy2 = Instantiate(_enemy, new Vector3(Random.Range(-9, 0), 8, 0), Quaternion.identity);
            newEnemy.transform.parent = spawningContainer.transform;
            newEnemy2.transform.parent = spawningContainer.transform;
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator spawnPowerUps()
    {
        yield return new WaitForSeconds(2F);
        while (keepSpawning)
        {
            if (Random.Range(0, 20) == 20)
            {
                GameObject powerUp = Instantiate(powerups[5], new Vector3(Random.Range(-9, 9), 9, 0), Quaternion.identity);
            }
            else
            {
                GameObject powerUp = Instantiate(powerups[Random.Range(0, 5)], new Vector3(Random.Range(-9, 9), 9, 0), Quaternion.identity);
                powerUp.transform.parent = spawningContainer.transform;
                yield return new WaitForSeconds(Random.Range(3, 7));
            }
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveManager waveManage)
    {
        for (int loop = 0; loop < waveManage.GetNumberOfEnemies(); loop++)
        {
            GameObject newEnemy = Instantiate(waveManage.GetEnemy(), waveManage.GetWaypoints()[0].position, Quaternion.identity);
            newEnemy.GetComponent<EnemyMovement>().SetWaveManager(waveManage);
            newEnemy.transform.parent = spawningContainer.transform;
            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator SpawningPathedEnemies()
    {
        for (int waveIndex = 0; waveIndex < _waveManagement.Count; waveIndex++)
        {
            var currentWave = _waveManagement[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }


        
    }

    
    public void onPlayerDeath()
    {
        keepSpawning = false;

        StopAllCoroutines();
        Destroy(spawningContainer);
    }

    public void StartSpawning()
    {

        keepSpawning = true;
        spawningEnemy = StartCoroutine(SpawnRoutineEnemy());

        spawningPowerUps = StartCoroutine(spawnPowerUps());

        spawningPathed = StartCoroutine(SpawningPathedEnemies());
    }
    public void SpawnExtraEnemy()
    {
        GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-7, 4), 9, 0), Quaternion.identity);
        newEnemy.transform.parent = spawningContainer.transform;
    }

    public void SpawnExtraPowerUp()
    {
        GameObject powerUp = Instantiate(powerups[Random.Range(0, 3)], new Vector3(Random.Range(-9, 9), 9, 0), Quaternion.identity);
        powerUp.transform.parent = spawningContainer.transform;

    }
}
