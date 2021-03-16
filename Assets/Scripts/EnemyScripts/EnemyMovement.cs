using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    WaveManager _waveManagement;
    List<Transform> _waypoints;
    int waypointIndex = 0, _speed;

    private void Start()
    {
        
        if (_waveManagement)
        {
            _speed = _waveManagement.GetmoveSpeed();
            _waypoints = _waveManagement.GetWaypoints();
            transform.position = _waypoints[waypointIndex].transform.position;
        }
        else
        {
            _speed = Random.Range(2, 5);
        }
    }

    private void Update()
    {
        Move();

    }

    public void SetWaveManager(WaveManager _waveManag)
    {
        this._waveManagement = _waveManag;
    }

    private void Move()
    {
        if (_waypoints == null)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            
        }
        else
        {
            if (waypointIndex <= _waypoints.Count - 1)
            {
                var targetPosition = _waypoints[waypointIndex].transform.position;
                var movementThisFrame = _speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    waypointIndex++;
                }

            }
            else
            {
                Destroy(gameObject);

            }

        }
    }
}

