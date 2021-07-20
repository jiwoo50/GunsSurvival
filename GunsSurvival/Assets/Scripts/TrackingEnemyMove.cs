using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingEnemyMove : MonoBehaviour
{
    public static int Tracking_damage = 10;
    public float movePower = 5.0f;

    Rigidbody2D rb2d;
    Transform player;

    float searchTime = 0.7f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (searchTime <= 0.0f)
        {
            Rush();
            searchTime = 0.5f;
        }
        if (searchTime > 0.0f)
        {
            searchTime -= Time.deltaTime;
        }
        if (Mathf.Abs(transform.position.y) >= 4.89f || Mathf.Abs(transform.position.x) >= 2.95f)
        {
            Rush();
        }

    }

    void Rush()
    {
        if (!GameController.Instance.gameOver && player)
        {
            rb2d.velocity = Vector3.zero;
            Vector3 dir = player.position - transform.position;
            dir.Normalize();
            rb2d.AddForce(dir * movePower, ForceMode2D.Impulse);
        }
    }
}