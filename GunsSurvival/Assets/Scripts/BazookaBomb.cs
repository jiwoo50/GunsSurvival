using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBomb : MonoBehaviour
{
    //Bazooka bomb
    public float speed;

    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 position = rb2d.position;
        position.y += speed * Time.deltaTime;
        rb2d.MovePosition(position);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
