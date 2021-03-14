using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushMove : MonoBehaviour
{
    public GameObject player;
    public float movePower = 5.0f;

    Rigidbody2D rigidbody2D;
    bool isRush = true;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Rush();
        if (Mathf.Abs(transform.position.y) >= 4.89f || Mathf.Abs(transform.position.x) >= 2.95f)
        {
            rigidbody2D.velocity = Vector3.zero;
            isRush = true;
            Rush();
        }
    }
    void Rush()
    {
        if (isRush)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            rigidbody2D.AddForce(dir*movePower, ForceMode2D.Impulse);
            isRush = false;
        }
    }
}
