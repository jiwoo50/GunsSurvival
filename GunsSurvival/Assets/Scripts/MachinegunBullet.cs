using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunBullet : MonoBehaviour
{
    //Machine Gun bullet
    public static int bulletDamage = 10;

    public float speed;

    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.AddForce(this.gameObject.transform.up * speed,ForceMode2D.Impulse);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("bulletLimit"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
