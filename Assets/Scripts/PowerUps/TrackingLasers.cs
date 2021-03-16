using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingLasers : MonoBehaviour
{
    [SerializeField]private GameObject[] _targets;
    private GameObject _target;
    void Start()
    {
        GetTarget();
        Destroy(this.gameObject, 5f);
    }

    private void GetTarget()
    {
        if (!GameObject.FindGameObjectWithTag("Enemy"))
        { transform.Translate(Vector3.up * Time.deltaTime);
        }
        else
        {
            _targets = GameObject.FindGameObjectsWithTag("Enemy");

            int randomEnemy = Random.Range(0, _targets.Length);
            _target = _targets[randomEnemy];
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        if (_target == null || _target.tag !="Enemy" )
        {
            GetTarget();
            if (_target == null)
            {
                transform.Translate(Vector3.up * Time.deltaTime);
            }
        }
        else if (_target.transform.position.y < -5.4f)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.up = _target.transform.position - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, 2 * Time.deltaTime);

        }
    }
}
