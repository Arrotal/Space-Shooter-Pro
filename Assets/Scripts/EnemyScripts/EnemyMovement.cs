using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private WaveManager _waveManagement;
    private List<Transform> _waypoints;
    private int waypointIndex = 0, _speed;
    private EnemyBehav _enemyBehav;
    private Player _player;
    private bool isDead;
    private void Start()
    {
        
        _enemyBehav = GetComponent<EnemyBehav>();
        
       
            _player = GameObject.Find("Player").GetComponent<Player>();
        
        if (_waveManagement&&!isDead)
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
        AmIDead();
        if (_player == null)
        {
            Destroy(this.gameObject);

        }
        Move();
        
    }
    private void AmIDead()
    {
        isDead = _enemyBehav.isDead();
    }

    public void SetWaveManager(WaveManager _waveManag)
    {
        this._waveManagement = _waveManag;
    }

    private void Move()
    {
        if (_enemyBehav.ReturnID() == 2 && transform.position.y > _player.transform.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, Time.deltaTime *3);
        }
        else if (_waypoints == null)
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
            

        }
    }
}

