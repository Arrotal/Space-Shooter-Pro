using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserBehav : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            
            Destroy(this.gameObject);
        }
    }


}
