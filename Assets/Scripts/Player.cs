using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //PlayerMovement Stuff
    private float _horizontalInput, _VerticalInput, _yMax = 2f, _yMin = -3f, _xMax = 11f, _xMin = -11f;

    //fire Mechanics
    [SerializeField] private float _speed = 10f;// _fireRate = 0.2f, _nextFire = -0.2f;
    [SerializeField] private GameObject _projectile;
    private int _lives = 5;
    Coroutine firingCoroutine;

    //SpawnManager info
    private SpawnManager _spawnManager;

    void Start()
    {
        //Setting Player Pos to 0,0,0
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is missing");
        }
    }


    void Update()
    {
        MovementController();
        Fire();
    }


    /*  private void Fire()
      {
          if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
          {
              _nextFire = Time.time + _fireRate;
              Instantiate(_projectile, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
          }

      }*/

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            firingCoroutine = StartCoroutine(FireContinously());

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(firingCoroutine);
        }

    }
    IEnumerator FireContinously()
    {
        while (true)
        {
            Instantiate(_projectile, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void MovementController()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _VerticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(_horizontalInput, _VerticalInput, 0) * Time.deltaTime * _speed);

        //Limit the Top and Bottom Movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yMin, _yMax));


        //Wrap on the x Axis
        if (transform.position.x > _xMax)
        {
            transform.position = new Vector3(_xMin, transform.position.y, transform.position.z);

        }
        else if (transform.position.x < _xMin)
        {
            transform.position = new Vector3(_xMax, transform.position.y, transform.position.z);

        }


    }

    public int GetLives()
    { return _lives; }

    public void TakeLives()
    {
       

            _lives--;
            Debug.Log("Lives Left: " + _lives);
            if (_lives < 1)
            {
                DestroySelf();
            }
        
        
    }
    private void DestroySelf()
    {
       
        _spawnManager.onPlayerDeath();
        Destroy(this.gameObject);
    }
}
