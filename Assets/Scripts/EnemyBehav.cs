using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehav : MonoBehaviour
{
    private Player _player;
    [SerializeField] private int _scoreValue;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * 4 * Time.deltaTime);
        if (transform.position.y < -5.5f)
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
            Destroy(this.gameObject);
            
        }
        if (other.tag == "Projectile")
        {

            _player.AddScore(_scoreValue);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
