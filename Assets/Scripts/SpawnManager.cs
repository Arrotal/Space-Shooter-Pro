using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemy, spawningContainer;
    [SerializeField] private GameObject[] powerups;
    Coroutine spawningEnemy, spawningPowerUps;
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
            GameObject newEnemy3 = Instantiate(_enemy, new Vector3(Random.Range(-4,7),13,0), Quaternion.identity);
            newEnemy.transform.parent = spawningContainer.transform;
            newEnemy2.transform.parent = spawningContainer.transform;
            newEnemy3.transform.parent = spawningContainer.transform;
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator spawnPowerUps()
    {
        yield return new WaitForSeconds(2F);
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

    public void StartSpawning()
    {

        keepSpawning = true;
        spawningEnemy = StartCoroutine(SpawnRoutineEnemy());

        spawningPowerUps = StartCoroutine(spawnPowerUps());
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
