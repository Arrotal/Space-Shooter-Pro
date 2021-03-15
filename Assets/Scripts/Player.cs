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
    [SerializeField] private GameObject[] _fires;
    private int _lives = 3, _shieldHits= 2;
    Coroutine firingCoroutine;
    private bool _alternativeFire;
    [SerializeField] private AudioClip _laser;
    private AudioSource _audioSource;

    //powerup mechanics
    [SerializeField]private bool _tripleShotEnabled = false, _speedBoost = false, _shield = false;
    public float _tripleShotDuration = 0f, speedBoostAmount = 0f;
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


        _score = 0;
        _playerShield.SetActive(false);
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _UIManager = GameObject.Find("UIManagerCanvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _UIManager.CurrentLives(_lives);
        _UIManager.AddScore(_score);

        foreach (GameObject fire in _fires)
        {
            fire.SetActive(false);
        }
        CheckForNulls();
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
    }
    void Update()
    {
        MovementController();
        WeaponTyping();
        Fire();

    }
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
                yield return new WaitForSeconds(0.1f);
            }
            else if (_alternativeFire)
            {
                Instantiate(_projectile, transform.position + new Vector3(-0.1f, 0.7f, 0), Quaternion.identity);
                _alternativeFire = false;
                _audioSource.Play();
                yield return new WaitForSeconds(0.05f);
               
            }
            else if (!_alternativeFire)
            {
                Instantiate(_projectile, transform.position + new Vector3(0.1f, 0.7f, 0), Quaternion.identity);
                _alternativeFire = true;
                _audioSource.Play();
                yield return new WaitForSeconds(0.05f);
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
    public void TakeLives()
    {

        if (_shield)
        {
            _shieldHits--;
            _playerShield.GetComponent<SpriteRenderer>().color = Color.red;
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
            { _fires[1].SetActive(true); }
            else if (_fires[1].activeSelf)
            { _fires[0].SetActive(true); }
            else
            {
                _fires[UnityEngine.Random.Range(0, _fires.Length)].SetActive(true);
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
        _shieldHits = 2;
        _playerShield.GetComponent<SpriteRenderer>().color = Color.white ;
        _playerShield.SetActive(true);
        _shield = true;
    }

    public void AddScore(int scorePoints)
    {
        _score += scorePoints;
        _UIManager.AddScore(_score);
    }

    
   


}
