using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehav : MonoBehaviour
{
   [SerializeField] private float speed = 17f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.position.y > 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        Destroy(this.gameObject);
        }
    }

    
}
