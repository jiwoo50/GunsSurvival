using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public GameObject splashPrefab;
    public GameObject flamePrefab;
    public GameObject boomPrefab;
    //public GameObject[] items;
    //public float[] percentage;

    public int HP;

    GameObject splash;

    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            GameObject boom = Instantiate(boomPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(boom, 1.0f);
            //GameObject projectileObject = Instantiate(items[(int)ChooseItem()], this.gameObject.transform.position, Quaternion.identity);
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
        else if (collision.CompareTag("shotGunBullet"))
        {
            HP -= ShotgunBullet.shotDamage;
        }
        else return;
        Destroy(collision.gameObject);

        if (splash) Destroy(splash);

        GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(flame, 0.8f);

    }

    // float ChooseItem()
    // {
    //     float total = 0;
    //     foreach (float elem in percentage)
    //     {
    //         total += elem;
    //     }
    //     float randomPoint = Random.value * total;
    //     for (int i = 0; i < percentage.Length; i++)
    //     {
    //         if (randomPoint < percentage[i])
    //         {
    //             return i;
    //         }
    //         else
    //         {
    //             randomPoint -= percentage[i];
    //         }
    //     }
    //     return percentage.Length - 1;
    // }
}
