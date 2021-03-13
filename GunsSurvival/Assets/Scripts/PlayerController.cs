using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed; 
    public Rigidbody2D rb2d;
    public Boundary boundary;

    float horizontal, vertical;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 position = rb2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, boundary.xMin, boundary.xMax);
        position.y = Mathf.Clamp(position.y, boundary.yMin, boundary.yMax);

        rb2d.MovePosition(position);
    }
}
