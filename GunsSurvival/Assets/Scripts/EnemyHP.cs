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

    float splashRadius = 1.0f;

    bool splash = false;

    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            GameObject boom = Instantiate(boomPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(boom, 1.0f);
            //GameObject projectileObject = Instantiate(items[(int)ChooseItem()], this.gameObject.transform.position, Quaternion.identity);
        }
        if (splash)
        {
            Collider2D[] hitobj = Physics2D.OverlapCircleAll(transform.position, splashRadius);
            foreach (Collider2D hit in hitobj)
            {
                if (hit.gameObject.tag == "Rush" || hit.gameObject.tag == "Tracking")
                {
                    hit.gameObject.GetComponent<EnemyHP>().Damage(BazookaBomb.splashDamage);
                    splash = false;
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("machineGunBullet"))
        {
            Damage(MachinegunBullet.bulletDamage);
        }
        else if (collision.CompareTag("bazookaBomb"))
        {
            Damage(BazookaBomb.bombDamage);
            splash = true;
        }
        else if (collision.CompareTag("shotGunBullet"))
        {
            Damage(ShotgunBullet.shotDamage);
        }
        else return;
        Destroy(collision.gameObject);

        GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(flame, 0.8f);
    }

    void Damage(int amount)
    {
        HP -= amount;
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