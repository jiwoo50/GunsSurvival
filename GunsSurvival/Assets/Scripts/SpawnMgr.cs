using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    public GameObject[] Enemies;
    public GameObject[] SpawnAreas;

    BoxCollider2D boxCollider;

    public float spawnDelay = 1.5f;
    public float startWait = 3.0f;

    int numberOfEnemies = 0;
    int numberOfRush = 0;
    int numberOfTracking = 0;
    int numberOfDivisive = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {
        if (GameController.Instance.gameOver)
        {
            StopCoroutine(SpawnEnemy());
            Destroy(GameObject.FindWithTag("Rush"));
            Destroy(GameObject.FindWithTag("Tracking"));
            Destroy(GameObject.FindWithTag("Divisive"));
        }
        numberOfRush = (int)GameObject.FindGameObjectsWithTag("Rush").Length;
        numberOfTracking = (int)GameObject.FindGameObjectsWithTag("Tracking").Length;
        numberOfDivisive = (int)GameObject.FindGameObjectsWithTag("Divisive").Length;
        numberOfEnemies = numberOfRush + numberOfTracking + numberOfDivisive;
    }

    Vector3 GetRandomPos(int idx)
    {
        Vector3 originPos = SpawnAreas[idx].transform.position;
        boxCollider = SpawnAreas[idx].GetComponent<BoxCollider2D>();

        float range_x = boxCollider.bounds.size.x;
        float range_y = boxCollider.bounds.size.y;
        range_x = Random.Range((range_x / 2) * (-1), range_x / 2);
        range_y = Random.Range((range_y / 2) * (-1), range_y / 2);
        Vector3 randomPos = new Vector3(range_x, range_y, 0.0f);
        Vector3 spawnPos = originPos + randomPos;
        spawnPos.z = -1.0f;

        return spawnPos;
    }
    
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            if (GameController.Instance.gameOver) break;
            if (numberOfEnemies < GameController.Instance.maxEnemy)
            {
                int val = Random.Range(0, SpawnAreas.Length);
                int enemy_idx = Random.Range(0, Enemies.Length);
                Instantiate(Enemies[enemy_idx], GetRandomPos(val), Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
            else yield return null;
        }
    } 
}
