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
    public Boundary boundary;
    public GameObject explosionPrefab;

    public float speed;
    public float timeInvincible = 2.0f;

    public int currentHealth;
    public int maxHealth = 30;

    float horizontal, vertical;
    float angle;

    bool isInvincible = false;

    Rigidbody2D rb2d;
    Renderer render;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        render = gameObject.GetComponent<Renderer>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - gameObject.transform.position.y, mouse.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
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

    void OnTriggerStay2D(Collider2D collision) // item -> call OnTriggerEnter2D 
    {
        if (collision.gameObject.CompareTag("Rush"))
        {
            ChangeHealth(-RushMove.Rush_damage);
        }
        if (collision.gameObject.CompareTag("Tracking"))
        {
            ChangeHealth(-TrackingEnemyMove.Tracking_damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("HealthPotion"))){
            ChangeHealth(1);//아이템 추가시 변경
        }
    }
    void ChangeHealth(int amount) 
    {
        if (amount < 0)
        {
            if (isInvincible) return;
            StartCoroutine(OnDamage());
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth <= 0)
        {
            GameController.Instance.PlayerDead();
            Destroy(gameObject);
        }
    }

    IEnumerator OnDamage()
    {
        int cnt = 0;
        isInvincible = true;
        while (cnt < 4)
        {
            render.material.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.25f);
            render.material.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.25f);
            ++cnt;
        }
        isInvincible = false;
    }
}