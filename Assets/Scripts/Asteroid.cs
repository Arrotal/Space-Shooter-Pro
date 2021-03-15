using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float rotationSpeed =-.05f;
    [SerializeField]private GameObject _explosion;
    private Collider2D _collider;
    private SpawnManager _spawnManager;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is missing");
        }
    }
    void Update()
    {
        transform.Rotate(0,0,rotationSpeed, Space.Self);
    }
    
   

private void OnTriggerEnter2D(Collider2D other)
{
        {
            if (other.tag == "Projectile")
            {
                Destroy(other.gameObject);
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject,0.2f);
                _collider.enabled = false;
                _spawnManager.StartSpawning();
            }
        }
    }

}
