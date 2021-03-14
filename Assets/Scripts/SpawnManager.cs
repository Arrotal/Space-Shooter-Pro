using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemy, spawningContainer;
    [SerializeField] private GameObject[] powerups;
    Coroutine spawningEnemy, spawningPowerUps;
    private bool keepSpawning = true;
    void Start()
    {
        spawningEnemy = StartCoroutine(SpawnRoutineEnemy());
        spawningPowerUps = StartCoroutine(spawnPowerUps());
    }

    IEnumerator SpawnRoutineEnemy()
    {
        while (keepSpawning)
        {
            
            GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-9,9),9,0), Quaternion.identity);
            newEnemy.transform.parent = spawningContainer.transform;
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator spawnPowerUps()
    {

        while (keepSpawning)
        {

            GameObject powerUp = Instantiate(powerups[Random.Range(0,3)], new Vector3(Random.Range(-9, 9), 9, 0), Quaternion.identity);
            powerUp.transform.parent = spawningContainer.transform;
            yield return new WaitForSeconds(Random.Range(3,7));
        }
    }

    public void onPlayerDeath()
    {
        keepSpawning = false;
        Destroy(spawningContainer);
    }

}
