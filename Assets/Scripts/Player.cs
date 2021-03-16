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
    //Player Camera
    [SerializeField] private Camera _mainCamera;
    private bool _cameraShaking;

    //PlayerMovement Stuff
    private float _horizontalInput, _VerticalInput, _yMax = 2f, _yMin = -3f, _xMax = 11f, _xMin = -11f;
    private bool _speedBoosting;

    //fire Mechanics
    private WeaponStats blasters;

    [SerializeField] private float _speed = 10f;// _fireRate = 0.2f, _nextFire = -0.2f;
    [SerializeField] private GameObject _projectile, _tripleShotProjectile, _playerShield, _HomingShots;
    [SerializeField] private GameObject[] _fires;
    private int _lives = 3, _shieldHits= 3, _ammo= 15, _ammoMax = 50,_fireActive;
    private Coroutine _firingCoroutine;
    private bool _alternativeFire;
    [SerializeField] private AudioClip _laser;
    private AudioSource _audioSource;
    private bool[] _fireBool= new bool[2];

    //powerup mechanics
    [SerializeField] private bool _tripleShotEnabled = false, _speedBoost = false, _shield = false, _HomingShotsEnabled = false;
    public float _tripleShotDuration = 0f, speedBoostAmount = 0f, _HomingShotDuration = 0f, _speedBoostDuration = 0f, _speedBoostCoolDown = 0f;
    //SpawnManager mechanics
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    //Score Mechanics
    [SerializeField] private int _score;
    private UIManager _UIManager;

    [SerializeField] private GameObject _explosion;

    void Start()
    {

        SetDefaults();
        WeaponTyping();
    }

    private void SetDefaults()
    {
        _cameraShaking = false;
        _speedBoosting = false;
        _score = 0;
        _speedBoostDuration = 10f;
        _speedBoostCoolDown = 0f;
        _playerShield.SetActive(false);
        transform.position = new Vector3(0, 0, 0);
        for (int x=0; x<_fireBool.Length; x++)
        {
            _fireBool[x]= false;
        }
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _UIManager = GameObject.Find("UIManagerCanvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _UIManager.CurrentLives(_lives);
        _UIManager.AddScore(_score);
        _UIManager.SpeedBoostDuration(_speedBoostDuration);
        _UIManager.SpeedBoostOnCooldown(false);
        foreach (GameObject fire in _fires)
        {
            fire.SetActive(false);
        }
        CheckForNulls();

        _UIManager.AmmoCount(_ammo);
        _UIManager.AmmoMax(_ammoMax);
        _audioSource.clip = _laser;
    }

    private void CheckForNulls()
    {
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is missing");
        }
        if (_UIManager == null)
        {

            Debug.LogError("Score Board is missing");
        }
        if (_gameManager == null)
        {

            Debug.LogError("Game Manager is missing");
        }
        if (_audioSource == null)
        {

            Debug.LogError("Player Audio Source is missing");

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
        else if (_HomingShotsEnabled)
        {
            blasters = new WeaponStats("Homing Shots", 0.4f, _HomingShotDuration);
            _HomingShotDuration -= _HomingShotDuration;

        }
    }
    void Update()
    {
        MovementController();
        WeaponTyping();
        Fire();
        SpeedBoost();
    }

    private void SpeedBoost()
    {
        SpeedBoostCooldowns();
        SpeedBoostCheck();
        
    }

    private void SpeedBoostCooldowns()
    {
        if (_speedBoosting)
        {
            _speedBoostDuration -= Time.deltaTime;
            _UIManager.SpeedBoostDuration(_speedBoostDuration);
        }
        if (_speedBoostCoolDown > 0)
        {
            _speedBoostCoolDown -= Time.deltaTime;

            _speedBoostDuration += Time.deltaTime;
            _UIManager.SpeedBoostDuration(_speedBoostDuration);
            if (_speedBoostCoolDown < 0)
            {
                _speedBoostCoolDown = 0;
                _speedBoostDuration = 10f;
                _UIManager.SpeedBoostOnCooldown(false);
            }
        }
        if (!_speedBoosting && _speedBoostCoolDown == 0 && _speedBoostDuration<10f)
        {
            _speedBoostDuration += Time.deltaTime;

            if (_speedBoostDuration > 10f)
            {
                _speedBoostDuration = 10f;
            }
            _UIManager.SpeedBoostDuration(_speedBoostDuration);
        }

    }

    private void SpeedBoostCheck()
    {
        if (_speedBoostCoolDown ==0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) )
            {
                StartCoroutine("SpeedBoostCooldown");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            { _speed = 10f;
                StopCoroutine("SpeedBoostCooldown");
                _speedBoosting = false;
            }
        }
    }
    IEnumerator SpeedBoostCooldown()
    {
        _speedBoosting = true;
        _speed = 15f;
        
        yield return new WaitForSeconds(_speedBoostDuration);

        _UIManager.SpeedBoostOnCooldown(true);
        _speed = 10f;
        _speedBoostCoolDown = 10f;
        _speedBoosting = false;
    }
    
    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _firingCoroutine = StartCoroutine(FireContinously());

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(_firingCoroutine);
        }

    }
    IEnumerator FireContinously()
    {


        while (true)
        {
            if (_tripleShotEnabled)
            {
                Instantiate(_tripleShotProjectile, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                _audioSource.Play();
            }
            else if (_HomingShotsEnabled)
            {
                Instantiate(_HomingShots, transform.position  , Quaternion.identity);
                yield return new WaitForSeconds(0.3f);
                _audioSource.Play();
            }
            else if (_alternativeFire)
            {
                if (_ammo != 0)
                {
                    Instantiate(_projectile, transform.position + new Vector3(-0.1f, 0.7f, 0), Quaternion.identity);
                    _alternativeFire = false;
                    _audioSource.Play();
                    _ammo--;

                    _UIManager.AmmoCount(_ammo);
                }

                yield return new WaitForSeconds(0.5f);
            }
            else if (!_alternativeFire)
            {
                if (_ammo != 0)
                {
                    Instantiate(_projectile, transform.position + new Vector3(0.1f, 0.7f, 0), Quaternion.identity);
                    _alternativeFire = true;
                    _audioSource.Play();
                    _ammo--;

                    _UIManager.AmmoCount(_ammo);
                }

                yield return new WaitForSeconds(0.5f);
            }
            
        }
    }
    void MovementController()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _VerticalInput = Input.GetAxis("Vertical");
        if (_speedBoost)
        { transform.Translate(new Vector3(_horizontalInput, _VerticalInput, 0) * Time.deltaTime * (speedBoostAmount + _speed)); }
        
        else
        { transform.Translate(new Vector3(_horizontalInput, _VerticalInput, 0) * Time.deltaTime * _speed); }
        

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
    public void TakeLives()
    {
        //CameraShake
        _cameraShaking = true;
        StartCoroutine(CameraShake());
        if (_shield)
        {
            _shieldHits--;
            if (_shieldHits == 2)
            {
                _playerShield.GetComponent<SpriteRenderer>().color = Color.green;
            }
            if (_shieldHits == 1)
            {
                _playerShield.GetComponent<SpriteRenderer>().color = Color.red;
            }
                if (_shieldHits < 1)
            {
                _shield = false;
                _playerShield.SetActive(false);
            }
                return;
        }
        else
        {
            _lives--;
            _UIManager.CurrentLives(_lives);
            if (_fires[0].activeSelf)
            { _fires[1].SetActive(true); _fireBool[1] = true; }
            else if (_fires[1].activeSelf)
            { _fires[0].SetActive(true); _fireBool[0] = true; }
            else
            {
                _fireActive = UnityEngine.Random.Range(0, _fires.Length);
                _fireBool[_fireActive] = true ;
                _fires[_fireActive].SetActive(true);
            }
            if (_lives < 1)
            {

                Instantiate(_explosion, transform.position, Quaternion.identity);
                DestroySelf();
                _UIManager.GameOver();
                _gameManager.EndGame();
        }
        }

    }

    IEnumerator CameraShake()
    {
        while (_cameraShaking)
        {
            _mainCamera.transform.position = new Vector3(0.2f,1,-10);
            yield return new WaitForSeconds(0.5f);


            _mainCamera.transform.position = new Vector3(0, 1, -10);
            _cameraShaking = false;
        } 
    }
    private void DestroySelf()
    {

        _spawnManager.onPlayerDeath();
        Destroy(this.gameObject);
    }
    public void EnableTripleShot()
    {
        StopCoroutine("TripleShotPowerDown");
        _tripleShotEnabled = true;
        _tripleShotDuration += 10f;
        StartCoroutine("TripleShotPowerDown");
        
    }
    IEnumerator TripleShotPowerDown()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(_tripleShotDuration);
            _tripleShotEnabled = false;
        }
    }

    public void EnableSpeed()
    {
        StopCoroutine("SpeedBoosterPowerDown");
        _tripleShotEnabled = true;
        _tripleShotDuration += 10f;
        StartCoroutine("SpeedBoosterPowerDown");
    }

    IEnumerator SpeedBoosterPowerDown()
    {
        yield return new WaitForSeconds(20f);
        _speedBoost = false;
    }

    public void EnableShield()
    {
        _shieldHits = 3;
        _playerShield.GetComponent<SpriteRenderer>().color = Color.white ;
        _playerShield.SetActive(true);
        _shield = true;
    }

    public void AddScore(int scorePoints)
    {
        _score += scorePoints;
        _UIManager.AddScore(_score);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile")
        {
            TakeLives();
        }
    }
    public void AmmoRefill()
    {
        _ammo += 15;
        if (_ammo > _ammoMax)
        {
            _ammo = _ammoMax;
            _score += 100;
            _UIManager.AddScore(_score);
        }
        _UIManager.AmmoCount(_ammo);
    }

    public void HealthRefill()
    {
        if (_lives < 3)
        {
            _lives++;
            if (_fireBool[0] && _fireBool[1])
            { _fireActive = UnityEngine.Random.Range(0, 1);
                _fires[_fireActive].SetActive(false);
                _fireBool[_fireActive] = false;
            }
            else if (_fireBool[0])
            {
                _fires[0].SetActive(false);
                _fireBool[0] = false;
            }
            else if (_fireBool[1])
            {
                _fires[1].SetActive(false);
                _fireBool[1] = false;
            }

            _UIManager.CurrentLives(_lives);
        }
        else
        {
            AddScore(100);
        }
    }
    public void HomingShot()
    {
        StopCoroutine("HomingShotPowerDown");
        _HomingShotsEnabled = true;
        _HomingShotDuration += 5f;
        StartCoroutine("HomingShotPowerDown");

    }
    IEnumerator HomingShotPowerDown()
    {
        while (true)
        {

            yield return new WaitForSeconds(_HomingShotDuration);
            _HomingShotsEnabled = false;
        }
    }
}
