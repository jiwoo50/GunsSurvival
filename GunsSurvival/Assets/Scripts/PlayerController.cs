using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{ 
    public static float exp = 0.0f;
    public static bool achieveMaxLevel = false;
    public static bool completeUpgrade = false;
    public static int currLevel = 0;
    public static int currHealth;
    public static int maxHealth = 30;
    public static int rushDamage = 5;
    public static int trackingDamage = 10;
    public static int divisiveEnemyDamage = 15;

    public Boundary boundary;
    public GameObject explosionPrefab;
    public Image expBar;

    public float speed;
    public float timeInvincible = 2.0f;

    float horizontal, vertical;
    float angle;

    bool isInvincible = false;
    bool chooseUpgrade = false;
    
    int maxLevel = 10;

    Rigidbody2D rb2d;
    Renderer render;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        render = gameObject.GetComponent<Renderer>();
        currHealth = maxHealth;

        GameController.Instance.ScoreReset();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (!PauseMenu.gamePaused)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(mouse.y - gameObject.transform.position.y, mouse.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }

        if (chooseUpgrade)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UpgradeMachineGun();
                completeUpgrade = true;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                UpgradeShotGun();
                completeUpgrade = true;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                UpgradeBazooka();
                completeUpgrade = true;
            }
        }

        if(currLevel == maxLevel)
        {
            expBar.fillAmount = 1.0f;
            achieveMaxLevel = true;
            chooseUpgrade = false;
            completeUpgrade = true;
        }

        if (!achieveMaxLevel) GetEXP();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 position = rb2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, boundary.xMin, boundary.xMax);
        position.y = Mathf.Clamp(position.y, boundary.yMin, boundary.yMax);

        rb2d.MovePosition(position);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rush"))
        {
            ChangeHealth(-rushDamage);
        }
        if (collision.gameObject.CompareTag("Tracking"))
        {
            ChangeHealth(-trackingDamage);
        }
        if (collision.gameObject.CompareTag("Divisive"))
        {
            ChangeHealth(-divisiveEnemyDamage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HP05")) //Health Potion
        {
            if (currHealth >= maxHealth) return;
            ChangeHealth(5);
        }

        else if (collision.gameObject.CompareTag("HP10")) //Health Potion
        {
            if (currHealth >= maxHealth) return;
            ChangeHealth(10);
        }

        else if (collision.gameObject.CompareTag("Gauge15")) //Recover Gauge
        {
            for(int i = 0; i < WeaponController.shootingWeapon.Length; i++) //0:machine, 1:shot, 3:bazooka
            {
                if (WeaponController.shootingWeapon[i])
                {
                    switch (i)
                    {
                        case 0:
                            if (WeaponController.machineVal <= 15.0f) WeaponController.machineVal = 0.0f;
                            else WeaponController.machineVal -= 15.0f;
                            break;
                        case 1:
                            if (ShotgunBullet.shotVal <= 15.0f) ShotgunBullet.shotVal = 0.0f;
                            else ShotgunBullet.shotVal -= 15.0f;
                            break;
                        case 2:
                            if (WeaponController.bazookaVal <= 15.0f) WeaponController.bazookaVal = 0.0f;
                            else WeaponController.bazookaVal -= 15.0f;
                            break;
                    }
                }
            }
        }

        else return;
        Destroy(collision.gameObject);
    }

    void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;
            StartCoroutine(OnDamage());
        }

        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
        GameController.Instance.PlayerHealth();

        if (currHealth <= 0)
        {
            GameController.Instance.PlayerDead();
            Destroy(gameObject);
        }
    }

    void GetEXP()
    {
        if (exp >= 100 && currLevel < maxLevel)
        {
            completeUpgrade = false;
            GameController.Instance.ShowUpgradeText();
            chooseUpgrade = true;
            exp = 0;
            ++currLevel;
            GameController.Instance.PlayerLevel();
        }
        expBar.fillAmount = exp / 100;
    }

    void UpgradeMachineGun()
    {
        MachinegunBullet.bulletDamage += 0.5f;
        WeaponController.machineGunShotDelay -= 0.025f;
    }

    void UpgradeShotGun()
    {
        ShotgunBullet.shotDamage += 0.5f;
        ShotgunBullet.shotDelay -= 0.05f;
    }

    void UpgradeBazooka()
    {
        BazookaBomb.bombDamage += 0.75f;
        BazookaBomb.splashDamage += 0.3f;
        WeaponController.bazookaShotDelay -= 0.05f;
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