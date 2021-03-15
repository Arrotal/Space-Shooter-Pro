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

    [SerializeField] private AudioClip _explosion;
    private void Start()
    {
        SetupEnemy();
    }

    private void SetupEnemy()
    {
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

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.5f&& !death)
        {
            transform.position = new Vector3(Random.Range(-9,9), 9, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.TakeLives();
               
            }
            _audioSource.Play();
            _animator.SetTrigger("OnDeath");
            Destroy(this.gameObject, 2.29f);
            _collider.enabled = false;
            death = true;
        }
        if (other.tag == "Projectile")
        {
            _animator.SetTrigger("OnDeath");
            _player.AddScore(_scoreValue);
            Destroy(other.gameObject);
            _audioSource.Play();
            Destroy(this.gameObject,2.29f);
            _collider.enabled = false;
            death = true;
        }

        if (other.tag == "Enemy" && transform.position.y>7.5)
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
    }

}
