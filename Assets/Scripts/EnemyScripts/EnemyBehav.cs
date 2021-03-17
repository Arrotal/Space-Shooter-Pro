using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehav : MonoBehaviour
{
    private Player _player;
    [SerializeField] private int _scoreValue;
    private Animator _animator;
    private Collider2D _collider;
    private float _speed;
    private bool death = false;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _laser,_laserUP,_shield;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private int _health;
    [SerializeField] private int _enemyID;

    private void Start()
    {
        SetupEnemy();
        StartCoroutine(FiringLaser());
    }

    private void SetupEnemy()
    {
        if (_enemyID ==1)
        { _shield.SetActive(true); }
        _speed = Random.Range(2, 5);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        if (_animator == null)
        {
            Debug.Log("Animator not found.");
        }
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
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
        _audioSource.clip = _explosion;
    }
    public int ReturnID()
    {
        return _enemyID;
    }
    void Update()
    {

        if (transform.position.y < -5.5f && !death)
        {
            if (_enemyID == 2)
            {
                Destroy(this.gameObject);
            }
            else

            {
                transform.position = new Vector3(Random.Range(-9, 9), 9, 0);
            }
        }
       
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile" && _enemyID >0 && _health>0)
        { 
            _shield.SetActive(false);
            _health--;
        }
        else if (_health <= 1 ||other.tag == "Player"||other.tag =="BossLaser")
        {
            if (other.tag == "Player")
            {
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeLives();

                }
                if (_shield != null)
                { _shield.SetActive(false); }
                _audioSource.Play();
                _animator.SetTrigger("OnDeath");
                Destroy(this.gameObject, 2.29f);
                _collider.enabled = false;
                transform.tag = "Untagged";
                death = true;
            }
            if (other.tag == "Projectile")
            {
                
                _animator.SetTrigger("OnDeath");
                _player.AddScore(_scoreValue);
                Destroy(other.gameObject);
                _audioSource.Play();
                Destroy(this.gameObject, 2.29f);
                _collider.enabled = false;
                transform.tag = "Untagged";
                death = true;
            }

            if (other.tag == "Enemy" && transform.position.y > 7.5)
            {
                Destroy(this.gameObject);
                _spawnManager.SpawnExtraEnemy();
            }

            if (other.tag == "PowerUp" && transform.position.y > 7.5)

            {
                Destroy(this.gameObject);
                _spawnManager.SpawnExtraEnemy();
                _spawnManager.SpawnExtraPowerUp();
            }
            if (other.tag == "BossLaser")
            {
                _animator.SetTrigger("OnDeath");
                _audioSource.Play();
                Destroy(this.gameObject, 2.29f);
                _collider.enabled = false;
                transform.tag = "Untagged";
                death = true;
            }
        }
    }
    IEnumerator FiringLaser()
    {
        while (true)
        {
           float playerYPos = transform.position.x- _player.transform.position.x;
    if (Mathf.Abs(playerYPos) < 1&& transform.position.y <_player.transform.position.y)
            {
                Instantiate(_laserUP, transform.position, Quaternion.identity);
            }
            else
            {


                Instantiate(_laser, transform.position, Quaternion.identity);
                }
            yield return new WaitForSeconds(Random.Range(2,7));

        }

    }
}
