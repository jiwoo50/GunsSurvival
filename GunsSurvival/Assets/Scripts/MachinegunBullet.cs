using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunBullet : MonoBehaviour
{
    public static float bulletDamage = 7.0f;
    public float flyingTime = 0.35f;

    public GameObject player;

    public float speed;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(this.gameObject.transform.up * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        if(GameController.Instance.gameOver) Destroy(gameObject);
        if(gameObject) Destroy(this.gameObject, flyingTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
