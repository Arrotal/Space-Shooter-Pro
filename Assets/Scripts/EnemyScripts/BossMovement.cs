using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    WaveManager _waveManagement;
    List<Transform> _waypoints;
    int _waypointIndex = 0, _speed;
    private bool _bossStop;
    private void Start()
    {        
            _speed = _waveManagement.GetmoveSpeed();
            _waypoints = _waveManagement.GetWaypoints();
               
    }

    private void Update()
    {
        Move();

    }

    public void SetWaveManager(WaveManager _waveManag)
    {
        this._waveManagement = _waveManag;
    }

    public void BossStop(bool isIt)
    {
        _bossStop = isIt;
    }
    private void Move()
    {
        if (!_bossStop)
        {
            if (_waypoints == null)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);

            }
            else
            {
                if (_waypointIndex <= _waypoints.Count - 1)
                {
                    var targetPosition = _waypoints[_waypointIndex].transform.position;
                    var movementThisFrame = _speed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                    if (transform.position == targetPosition)
                    {
                        _waypointIndex++;
                    }

                }
                else
                {
                    _waypointIndex = 0;

                }

            }
        }
    }
}
