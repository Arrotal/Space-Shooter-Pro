using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats

{
    public string name;
    public float fireRate, duration;

    public WeaponStats(string incomingName, float incomingFireRate)
    {
        name = incomingName;
        fireRate = incomingFireRate;
    }
    public WeaponStats(string incomingName, float incomingFireRate, float incomingDuration)
    {
        name = incomingName;
        fireRate = incomingFireRate;
        duration = incomingDuration;
    }

    public void BaseWeapon(string incomingName, float incomingFireRate)
    {
        name = incomingName;
        fireRate = incomingFireRate;
    }

    
}

public class Player : MonoBehaviour
{
    //PlayerMovement Stuff
    private float _horizontalInput, _VerticalInput, _yMax = 2f, _yMin = -3f, _xMax = 11f, _xMin = -11f;

    //fire Mechanics
    private WeaponStats blasters;

    [SerializeField] private float _speed = 10f;// _fireRate = 0.2f, _nextFire = -0.2f;
    [SerializeField] private GameObject _projectile, _tripleShotProjectile, _playerShield;
    private int _lives = 5;
    Coroutine firingCoroutine;
    private bool _alternativeFire;
    private bool _normalFire;
    //powerup mechanics
    private bool _tripleShotEnabled = false, _speedBoost = false, _shield = false;
    public float _tripleShotDuration = 5f, speedBoostAmount = 10f;
    //SpawnManager mechanics
    private SpawnManager _spawnManager;

    void Start()
    {
        _playerShield.SetActive(false);
        WeaponTyping();

        //Setting Player Pos to 0,0,0
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is missing");
        }
    }

    private void WeaponTyping()
    {
        if(blasters == null)
        {
            blasters = new WeaponStats("Blasters", 0.1f);

        }

        if (!_tripleShotEnabled)
        {
            blasters.BaseWeapon("Blasters", 0.1f);
        }
        else if (_tripleShotEnabled)
        {
            blasters = new WeaponStats("Triple Shot", 0.2f, _tripleShotDuration);

            _tripleShotDuration -= Time.deltaTime;
        }
    }


    void Update()
    {
        MovementController();
        WeaponTyping();
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
            if (_tripleShotEnabled)
            {
                Instantiate(_tripleShotProjectile, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
            else if (_alternativeFire)
            {
                Instantiate(_projectile, transform.position + new Vector3(-0.2f, 1.2f, 0), Quaternion.identity);
                _alternativeFire = false;
                yield return new WaitForSeconds(0.1f);
            }
            else if (!_alternativeFire)
            {
                Instantiate(_projectile, transform.position + new Vector3(0.2f, 1.2f, 0), Quaternion.identity);
                _alternativeFire = true;
                yield return new WaitForSeconds(0.1f);
            }
            
        }
    }
    void MovementController()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _VerticalInput = Input.GetAxis("Vertical");
        if (_speedBoost)
        { transform.Translate(new Vector3(_horizontalInput, _VerticalInput, 0) * Time.deltaTime * (speedBoostAmount + _speed)); }
        else { transform.Translate(new Vector3(_horizontalInput, _VerticalInput, 0) * Time.deltaTime * _speed); }
        

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
    {
        return _lives;
    }

    public void TakeLives()
    {

        if (_shield)
        {
            _shield = false;
            _playerShield.SetActive(false);
            return;
        }
        else
        {
            _lives--;
            Debug.Log("Lives Left: " + _lives);
            if (_lives < 1)
            {
                DestroySelf();
            }
        }

    }
    private void DestroySelf()
    {

        _spawnManager.onPlayerDeath();
        Destroy(this.gameObject);
    }

    public void EnableTripleShot()
    {
        _tripleShotEnabled = true;
        _tripleShotDuration = 5f;
        StartCoroutine(TripleShotPowerDown());
    }
    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotEnabled = false;
    }

    public void EnableSpeed()
    {
        _speedBoost = true;
        StartCoroutine(SpeedBoosterPowerDown());
    }

    IEnumerator SpeedBoosterPowerDown()
    {
        yield return new WaitForSeconds(10f);
        _speedBoost = false;
    }

    public void EnableShield()
    {

        _playerShield.SetActive(true);
        _shield = true;
    }

    




}
