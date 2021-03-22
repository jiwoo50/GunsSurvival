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
    public static int currentHealth;

    public float speed;
    public float timeInvincible = 2.0f;
   
    public Boundary boundary;
    public GameObject explosionPrefab;

    float horizontal, vertical;

    Rigidbody2D rb2d;
    //Vector2 mouse;
    float angle;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - gameObject.transform.position.y, mouse.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
    }

    void FixedUpdate()
    {
        Vector2 position = rb2d.position;
        Vector3 rotation = gameObject.transform.rotation.eulerAngles;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, boundary.xMin, boundary.xMax);
        position.y = Mathf.Clamp(position.y, boundary.yMin, boundary.yMax);

        rb2d.MovePosition(position);
    }
}
