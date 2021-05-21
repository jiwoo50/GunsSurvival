using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBulletRange : MonoBehaviour
{
    void Update()
    {
        if(GameController.Instance.gameOver) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
