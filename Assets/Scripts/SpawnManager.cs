using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemy, _enemyContainer;
    Coroutine spawning;
    private bool keepSpawning = true;
    void Start()
    {
        spawning = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (keepSpawning)
        {
            
            GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-9,9),9,0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    public void onPlayerDeath()
    {
        keepSpawning = false;
    }

}
