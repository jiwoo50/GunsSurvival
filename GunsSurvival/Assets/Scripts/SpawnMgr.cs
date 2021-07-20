using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] points;

    public float spawnDelay = 1.5f;
    public float startWait = 3.0f;

    public int maxEnemy = 15;

    void Start()
    {
        points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {
        if (GameController.Instance.gameOver)
        {
            StopCoroutine(SpawnEnemy());
            Destroy(GameObject.FindWithTag("Rush"));
            Destroy(GameObject.FindWithTag("Tracking"));
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            if (GameController.Instance.gameOver) break;
            int enemyCnt = (int)GameObject.FindGameObjectsWithTag("Rush").Length + (int)GameObject.FindGameObjectsWithTag("Tracking").Length;
            if (enemyCnt <= maxEnemy)
            {
                int pos_idx = Random.Range(1, points.Length);
                int enemy_idx = Random.Range(0, enemies.Length);
                Instantiate(enemies[enemy_idx], points[pos_idx].position, Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
            else yield return null;
        }
    }
}
