using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingEnemyMove : MonoBehaviour
{
    public GameObject player;
    public float movePower = 5.0f;

    Rigidbody2D rigidbody2D;
    float searchTime = 0.7f;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (searchTime <= 0.0f)
        {
            Rush();
            searchTime = 0.7f;
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
        rigidbody2D.velocity = Vector3.zero;
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        rigidbody2D.AddForce(dir * movePower, ForceMode2D.Impulse);
    }
}
