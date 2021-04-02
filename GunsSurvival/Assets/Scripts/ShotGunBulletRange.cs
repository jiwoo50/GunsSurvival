using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBulletRange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
