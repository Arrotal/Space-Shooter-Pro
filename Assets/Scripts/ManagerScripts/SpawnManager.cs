using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private List<WaveManager> _waveManagement;
    [SerializeField] private WaveManager _Boss;
    [SerializeField] private GameObject _enemy, spawningContainer, _enemyLeft, _enemyRight;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private int randomChance;
    Coroutine spawningEnemy, spawningPowerUps, spawningPathed, spawningRight;
    private bool keepSpawning = false, _bossDead;
    void Start()
    {
        _bossDead = false;
    }

    IEnumerator TimeUntilBoss()
    {
        while (!_bossDead)
        {
            yield return new WaitForSeconds(1f);

            StartCoroutine(BossTime(_Boss));
        }

    }
    IEnumerator BossTime(WaveManager boss)
    {

        keepSpawning = false;
        StopAllCoroutines();

        spawningPowerUps = StartCoroutine(spawnPowerUps());
        GameObject bossEnemy = Instantiate(boss.GetEnemy(), new Vector3(0,9,0), Quaternion.identity);
        bossEnemy.GetComponent<BossMovement>().SetWaveManager(boss);

        yield return new WaitUntil(() => _bossDead );
        StartSpawning();

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
        while (true)
        {
            randomChance = Random.Range(0, 21);
            if(randomChance ==20)
            { 
                GameObject powerUp = Instantiate(powerups[6], new Vector3(Random.Range(-9, 9), 9, 0), Quaternion.identity);
            }
            else if (randomChance >=15)
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
        StartCoroutine(Wait3Seconds());
        
    }
    IEnumerator Wait3Seconds()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(TimeUntilBoss());
        _bossDead = false;
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
