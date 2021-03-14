using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehav : MonoBehaviour
{

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
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
