using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBomb : MonoBehaviour
{
    public static float bombDamage = 15.0f;
    public static float splashDamage = 4.0f;

    public float speed = 7.0f;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(this.gameObject.transform.up * speed, ForceMode2D.Impulse);
    }
    void Update()
    {
        if (GameController.Instance.gameOver) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
