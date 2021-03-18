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
    private bool _isDead, _attemptedDodge;
    private float randomXFloat=0;
    private void Start()
    {
        _attemptedDodge = false;
        _enemyBehav = GetComponent<EnemyBehav>();

        Player.playerShoots += AttemptToAvoid;
            _player = GameObject.Find("Player").GetComponent<Player>();
        
        if (_waveManagement&&!_isDead)
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
    private void AttemptToAvoid(float PlayerPOS)
    {
        if (Mathf.Abs(PlayerPOS - transform.position.x) >= 0.5f && !_attemptedDodge && transform.position.y<=2)
        {
            randomXFloat = Random.Range(-3, 4);
        }
        else if (Mathf.Abs(PlayerPOS - transform.position.x) <= 0.5f)
        {
            randomXFloat = 0;
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
        _isDead = _enemyBehav.isDead();
    }

    public void SetWaveManager(WaveManager _waveManag)
    {
        this._waveManagement = _waveManag;
    }
    private void OnDisable()
    {
        Player.playerShoots -= AttemptToAvoid;
    }
    private void Move()
    {
        if (_enemyBehav.ReturnID() == 2 && transform.position.y > _player.transform.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, Time.deltaTime *3);
        }
        else if (_waypoints == null)
        {
            transform.Translate(new Vector3(randomXFloat,-1,0) * _speed * Time.deltaTime);

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

