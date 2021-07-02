using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] points;

    public float spawnDelay = 1.5f;

    public int maxEnemy = 15;

    bool flag = true;

    void Start()
    {
        points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if(GameController.Instance.startSpawn && flag)
        {
            StartCoroutine(SpawnEnemy());
            flag = false;
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (!GameController.Instance.gameOver)
        {
            int enemyCnt = (int)GameObject.FindGameObjectsWithTag("Rush").Length + (int)GameObject.FindGameObjectsWithTag("Tracking").Length;
            if (enemyCnt <= maxEnemy)
            {
                yield return new WaitForSeconds(spawnDelay);
                int pos_idx = Random.Range(1, points.Length);
                int enemy_idx = Random.Range(0, enemies.Length);
                Instantiate(enemies[enemy_idx], points[pos_idx].position, Quaternion.identity);
            }
            else yield return null;
        }
    }
}
