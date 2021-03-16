using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPowerUp : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, -1f, Space.Self);
    }
}
