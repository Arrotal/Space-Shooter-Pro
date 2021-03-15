using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    //PowerUp IDs
    //0=TripleShot
    //1=Speed
    //2=Shields
    //3=AmmoRefill
    //4=HealthRefill
    //5=
    [SerializeField] private int powerUpID;
    [SerializeField]private AudioClip _audio;
    private void Update()
    {
        if (powerUpID < 3)
        {
            transform.Translate(Vector3.down * Time.deltaTime * _speed);
        }
        if (powerUpID >=3)
        {
            transform.Rotate(0, 0, 1, Space.World);
            transform.Translate(new Vector3(0,-1,0) * Time.deltaTime * _speed,Space.World) ;
        }
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);        
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                
                switch (powerUpID)
                { case 0:
                        {
                            player.EnableTripleShot();
                            break;
                        }
                    case 1:
                        {
                            player.EnableSpeed();
                            break;
                        }
                    case 2:
                        {
                            player.EnableShield();
                            break;
                        }
                    case 3:
                        {
                            player.AmmoRefill();
                            break;
                        }
                    case 4:
                        {
                            player.HealthRefill();
                            break;
                        }
                    case 5:
                        {
                            player.HomingShot();
                            break;
                        }

                    default:
                        {
                            Debug.Log("Outside PowerUpID parameters");
                                break;
                        }
                }
                AudioSource.PlayClipAtPoint(_audio, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
}
