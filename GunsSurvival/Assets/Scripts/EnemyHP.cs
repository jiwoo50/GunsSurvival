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
        if (this.gameObject.CompareTag("Rush")) HP = GameController.currTriangleHP; //monsters get more HP over time
        if (this.gameObject.CompareTag("Tracking")) HP = GameController.currRectangleHP;
        if (this.gameObject.CompareTag("Divisive")) HP = GameController.currPentagonHP;
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
                for (int i = 0; i < 3; i++) Instantiate(RushEnemy, GetRandomPos(), Quaternion.identity); //spawn 3 triangle enemies
            }

            Destroy(this.gameObject);
            GameObject boom = Instantiate(boomPrefab, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            Destroy(boom, 1.0f);

            int item_idx = (int)ChooseItem();
            if (item_idx < items.Length) Instantiate(items[item_idx], this.gameObject.transform.position, Quaternion.identity); //spawn items randomly
        }

        if (splash)
        {
            Collider2D[] hitObj = Physics2D.OverlapCircleAll(transform.position, splashRadius);
            foreach (Collider2D hit in hitObj)
            {
                if (hit.gameObject.CompareTag("Rush") || hit.gameObject.CompareTag("Tracking") || hit.gameObject.CompareTag("Divisive"))
                {
                    hit.gameObject.GetComponent<EnemyHP>().Damage(PlayerController.currSplashDmg);
                    splash = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("machineGunBullet")) Damage(PlayerController.currMachineDmg);

        else if (collision.gameObject.CompareTag("bazookaBomb"))
        {
            Damage(PlayerController.currBazookaDmg);
            splash = true;
        }

        else if (collision.gameObject.CompareTag("shotGunBullet")) Damage(PlayerController.currShotgunDmg);

        else return;
        Destroy(collision.gameObject);
    }

    void Damage(float amount)
    {
        HP -= amount;
        if(HP > 0)
        {
            GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(flame, 0.8f);
        }
    }

    float ChooseItem()
    {
        float total = 0;
        foreach (float elem in percentage) total += elem;
        
        float randomPoint = Random.value * total; //0~100
        for (int i = 0; i < percentage.Length; i++)
        {
            if (randomPoint < percentage[i]) return i;
            else randomPoint -= percentage[i];
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
        y_val *= Random.Range(0, 2) == 0 ? -1 : 1; //negative or positive
        float y = y_val + b;

        Vector3 randomPos = new Vector3(x, y, -1);

        return randomPos;
    }
}