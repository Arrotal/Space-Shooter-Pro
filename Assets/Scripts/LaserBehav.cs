using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehav : MonoBehaviour
{
   [SerializeField] private float speed = 15f;
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.position.y > 8)
        { 
        Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Laser Hit " + other.transform.name);
    }
}
