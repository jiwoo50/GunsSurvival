using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBomb : MonoBehaviour
{
    //Bazooka bomb
    public float speed;

    public static int bombDamage = 20;
    public static int splashDamage = 5;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(this.gameObject.transform.up * speed,ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
