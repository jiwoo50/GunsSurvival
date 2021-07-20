using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushMove : MonoBehaviour
{
    public static int Rush_damage = 5;

    public float movePower = 5.0f;

    Rigidbody2D rb2d;
    Transform player;

    bool isRush = true;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        Rush();
        if (Mathf.Abs(transform.position.y) >= 4.89f || Mathf.Abs(transform.position.x) >= 2.95f)
        {
            rb2d.velocity = Vector3.zero;
            isRush = true;
            Rush();
        }
    }
    void Rush()
    {
        if (isRush && !GameController.Instance.gameOver && player)
        {
            Vector3 dir = player.position - transform.position;
            dir.Normalize();
            rb2d.AddForce(dir * movePower, ForceMode2D.Impulse);
            isRush = false;
        }

    }
}
