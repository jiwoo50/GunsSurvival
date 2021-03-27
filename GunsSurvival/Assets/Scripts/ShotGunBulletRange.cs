using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBulletRange : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("bulletLimit"))
        {
            Destroy(gameObject);
        }
    }
}
