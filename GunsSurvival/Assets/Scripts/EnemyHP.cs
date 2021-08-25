using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public GameObject flamePrefab;
    public GameObject boomPrefab;
    public GameObject RushEnemy;

    public GameObject[] items;
    public float[] percentage;

    float splashRadius = 1.0f;
    float HP;

    bool splash = false;

    void Start()
    {
        if (this.gameObject.CompareTag("Rush")) HP = GameController.Instance.currTriangleHP; //monsters get more HP over time
        if (this.gameObject.CompareTag("Tracking")) HP = GameController.Instance.currRectangleHP;
        if (this.gameObject.CompareTag("Divisive")) HP = GameController.Instance.currPentagonHP;
    }

    void Update()
    {
        if (HP <= 0)
        {
            if (this.gameObject.CompareTag("Rush")) PlayerController.exp += 2.0f;
            if (this.gameObject.CompareTag("Tracking")) PlayerController.exp += 5.0f;
            if (this.gameObject.CompareTag("Divisive"))
            {
                PlayerController.exp += 10.0f;
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(RushEnemy, GetRandomPos(), Quaternion.identity);
                }
            }

            Destroy(this.gameObject);
            GameObject boom = Instantiate(boomPrefab, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            Destroy(boom, 1.0f);

            int idx = (int)ChooseItem();
            if (idx < items.Length) Instantiate(items[idx], this.gameObject.transform.position, Quaternion.identity);
        }

        if (splash)
        {
            Collider2D[] hitObj = Physics2D.OverlapCircleAll(transform.position, splashRadius);
            foreach (Collider2D hit in hitObj)
            {
                if (hit.gameObject.CompareTag("Rush") || hit.gameObject.CompareTag("Tracking"))
                {
                    hit.gameObject.GetComponent<EnemyHP>().Damage(PlayerController.currSplashDmg);
                    if (hit.gameObject.GetComponent<EnemyHP>().HP > 0)
                    {
                        GameObject flame = Instantiate(flamePrefab, hit.transform.position, Quaternion.identity) as GameObject;
                        Destroy(flame, 0.8f);
                    }
                    splash = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("machineGunBullet")) Damage(PlayerController.currMachineDmg);

        else if (collision.CompareTag("bazookaBomb"))
        {
            Damage(PlayerController.currBazookaDmg);
            splash = true;
        }

        else if (collision.CompareTag("shotGunBullet")) Damage(PlayerController.currShotgunDmg);

        else return;
        Destroy(collision.gameObject);

        if (HP > 0)
        {
            GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(flame, 0.8f);
        }
    }

    void Damage(float amount)
    {
        HP -= amount;
    }

    float ChooseItem()
    {
        float total = 0;
        foreach (float elem in percentage)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;
        for (int i = 0; i < percentage.Length; i++)
        {
            if (randomPoint < percentage[i])
            {
                return i;
            }
            else
            {
                randomPoint -= percentage[i];
            }
        }
        return percentage.Length - 1;
    }

    Vector3 GetRandomPos()
    {
        float radius = 0.5f;
        Vector3 pos = this.transform.position;

        float a = pos.x;
        float b = pos.y;

        float x = Random.Range(-radius + a, radius + a);
        float y_val = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x - a, 2));
        y_val *= Random.Range(0, 2) == 0 ? -1 : 1; //양수, 음수 결정
        float y = y_val + b;

        Vector3 randomPos = new Vector3(x, y, 0);

        return randomPos;
    }
}