using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingEnemyMove : MonoBehaviour
{
    public static int Tracking_damage = 10;
    public GameObject player;
    public float movePower = 5.0f;

    Rigidbody2D rb2d;
    float searchTime = 0.7f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
        if (!PlayerController.isdead)
        {
            rb2d.velocity = Vector3.zero;
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            rb2d.AddForce(dir * movePower, ForceMode2D.Impulse);
        }
    }
}
