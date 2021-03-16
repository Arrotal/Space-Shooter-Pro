using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Wave Manager")]
public class WaveManager: ScriptableObject
{
    [SerializeField] private GameObject _enemy, _path;
    [SerializeField] private int _numberOfEnemies, _moveSpeed;

    public GameObject GetEnemy() { return _enemy; }

    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in _path.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public int GetmoveSpeed() {
        return _moveSpeed;
    }
    public int GetNumberOfEnemies() {
        return _numberOfEnemies;
    }

}
