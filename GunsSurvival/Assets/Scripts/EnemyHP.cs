using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int HP;
    public GameObject splashPrefab;
    public GameObject flamePrefab;
    public GameObject boomPrefab;

    GameObject splash;
    
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            GameObject boom = Instantiate(boomPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(boom, 1.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("machineGunBullet"))
        {
            HP -= MachinegunBullet.bulletDamage;
        }
        else if (collision.CompareTag("bazookaBomb"))
        {
            HP -= BazookaBomb.bombDamage;
            splash = Instantiate(splashPrefab, transform.position, Quaternion.identity) as GameObject;
        }
        else if (collision.CompareTag("Splash"))
        {
            HP -= BazookaBomb.splashDamage;
        }
        else return;

        if (splash) Destroy(splash);

        GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(flame, 0.8f);
        Destroy(collision.gameObject);
    }
}
