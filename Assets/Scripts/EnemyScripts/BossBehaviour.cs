using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private Player _player;
    [SerializeField] private int _scoreValue;
    private Animator _animator;
    private Collider2D _collider;
    private float _speed;
    private bool _death = false, _firingBigLaser;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _laser,_shield,_bigLaser, _thruster;
    [SerializeField] private AudioClip _explosion;
    private UIManager _UIManager;
    private BossMovement _bossMovement;
    [SerializeField] private GameObject _waypoints;
    private int _waypointIndex = 0;
    [SerializeField] private int _health;
    private void Start()
    {
        SetupEnemy();
        StartCoroutine(FiringLaser());
        _waypointIndex = 0;
    }

    private void SetupEnemy()
    {
        _firingBigLaser = false;
        _health = 100;
        _speed = 2;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _bossMovement = GetComponent<BossMovement>();
        if (_bossMovement == null)
        {
            Debug.LogError("Boss Movement not found");
        }
        if (_animator == null)
        {
            Debug.Log("Animator not found.");
        }
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("UIManagerCanvas").GetComponent<UIManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is missing");
        }
        if (_explosion == null)
        {
            Debug.LogError("Explosion Audio is missing");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is missing");
        }

        if (_UIManager == null)
        {
            Debug.Log("UI Manager is Missing");
        }
        _audioSource.clip = _explosion;

        _UIManager.BossStatus(true);
        _UIManager.BossHealth(_health);
    }


    private void FiringBigLaser()
    {
        StartCoroutine(FiringBigLaserStopMoving());
        //coroutine to keep the boss stationary while firing big laser
        
        
    }

    IEnumerator FiringBigLaserStopMoving()
    {

        StopCoroutine("FiringLaser");
        _bossMovement.BossStop(true);
     
        yield return new WaitForSeconds(1f);

        Instantiate(_bigLaser, transform.position +new Vector3(0f,-1.2f,0), Quaternion.identity);
        yield return new WaitForSeconds(3f);
        _firingBigLaser = false;
        _bossMovement.BossStop(false);
        StartCoroutine("FiringLaser");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Projectile")
        {

            Destroy(other.gameObject);
            TakeDamage();
            
        }
    }

    private void TakeDamage()
    {
        _health -= 1;
        _UIManager.BossHealth(_health);
        if (_health < 1)
        {
                _animator.SetTrigger("OnDeath");
                _player.AddScore(_scoreValue);
                _audioSource.Play();
                Destroy(this.gameObject, 2.29f);
                _collider.enabled = false;
                transform.tag = "Untagged";
            _shield.SetActive(false);
            _thruster.SetActive(false) ;
            _spawnManager.StartSpawning();
                _death = true;
            _UIManager.BossStatus(false);
            StopAllCoroutines();
        }
    }
    IEnumerator FiringLaser()
    {
        while (!_firingBigLaser)
        {
            Instantiate(_laser, transform.position + new Vector3(0.2f,-2.5f,0), Quaternion.identity);
            Instantiate(_laser, transform.position + new Vector3(-0.2f, -2.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(1f);
            if (Random.Range(0, 5) == 0)
            {
                FiringBigLaser();
                _firingBigLaser = true;
            }
        }
    }
    private void Update()
    {
        if (_UIManager.IsGameOver())
        {
            Destroy(this.gameObject);
        }

    }

}
