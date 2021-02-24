using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      
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
