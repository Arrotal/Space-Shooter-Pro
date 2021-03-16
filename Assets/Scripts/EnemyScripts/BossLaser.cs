using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Destroy(this.gameObject,1f); 
    }
    private void Update()
    {

        transform.Translate(Vector3.down * 0.04f);
        transform.localScale += new Vector3(0, 5f* Time.deltaTime, 0);
    }
}
