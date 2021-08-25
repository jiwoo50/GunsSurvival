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
    public static float currMachineDmg = 7.0f;
    public static float currMachineDelay = 0.75f;
    public static float currShotgunDmg = 4.0f;
    public static float currShotgunDelay = 1.2f;
    public static float currBazookaDmg = 15.0f;
    public static float currSplashDmg = 4.0f;
    public static float currBazookaDelay = 1.5f;
    public static bool achieveMaxLevel = false;
    public static bool completeUpgrade = false;
    public static int currLevel = 0;
    public static int currHealth;
    public static int maxHealth = 30;
  
    public Boundary boundary;
    public GameObject explosionPrefab;
    public Image expBar;

    public float speed;
    public float timeInvincible = 2.0f;

    float horizontal, vertical;
    float angle;
    float machineDmg = 7.0f;
    float machineDelay = 0.75f;
    float shotDmg = 4.0f;
    float shotDelay = 1.2f;
    float bazookaDmg = 15.0f;
    float splashDmg = 4.0f;
    float bazookaDelay = 1.5f;

    bool isInvincible = false;
    bool choosingUpgrade = false;

    int maxLevel = 10;

    Rigidbody2D rb2d;
    Renderer render;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        render = gameObject.GetComponent<Renderer>();
        Initialize();

        GameController.Instance.ResetScore();
        GameController.Instance.InitializeGame();
    }

    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(mouse.y - gameObject.transform.position.y, mouse.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }

        if (choosingUpgrade)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UpgradeMachineGun();
                completeUpgrade = true;
                choosingUpgrade = false;
                exp = 0;
                ++currLevel;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                UpgradeShotGun();
                completeUpgrade = true;
                choosingUpgrade = false;
                exp = 0;
                ++currLevel;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                UpgradeBazooka();
                completeUpgrade = true;
                choosingUpgrade = false; 
                exp = 0;
                ++currLevel;
            }
        }

        if (currLevel == maxLevel)
        {
            expBar.fillAmount = 1.0f;
            achieveMaxLevel = true;
        }

        if (!achieveMaxLevel && !choosingUpgrade) GetEXP();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Initialize()
    {
        currHealth = maxHealth;
        currMachineDmg = machineDmg;
        currMachineDelay = machineDelay;
        currShotgunDmg = shotDmg;
        currShotgunDelay = shotDelay;
        currBazookaDmg = bazookaDmg;
        currBazookaDelay = bazookaDelay;
        currSplashDmg = splashDmg;
    }

    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

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
            ChangeHealth(-GameController.Instance.currRushDmg);
        }
        if (collision.gameObject.CompareTag("Tracking"))
        {
            ChangeHealth(-GameController.Instance.currTrackingDmg);
        }
        if (collision.gameObject.CompareTag("Divisive"))
        {
            ChangeHealth(-GameController.Instance.currDivisiveDmg);
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
            for (int i = 0; i < WeaponController.shootingWeapon.Length; i++)
            {
                if (WeaponController.shootingWeapon[i])
                {
                    switch (i)
                    {
                        case 0: //machine gun
                            if (MachinegunBullet.machineVal <= 15.0f) MachinegunBullet.machineVal = 0.0f;
                            else MachinegunBullet.machineVal -= 15.0f;
                            break;
                        case 1: //shot gun
                            if (ShotgunBullet.shotVal <= 15.0f) ShotgunBullet.shotVal = 0.0f;
                            else ShotgunBullet.shotVal -= 15.0f;
                            break;
                        case 2: //bazooka
                            if (BazookaBomb.bazookaVal <= 15.0f) BazookaBomb.bazookaVal = 0.0f;
                            else BazookaBomb.bazookaVal -= 15.0f;
                            break;
                    }
                }
            }
        }

        else return;
        SoundController.Instance.PlayGetItemSound();
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
        if (exp >= 5 && currLevel < maxLevel)
        {
            completeUpgrade = false;
            choosingUpgrade = true;
            GameController.Instance.ShowUpgradeText();
            GameController.Instance.PlayerLevel();
        }
        expBar.fillAmount = exp / 100;
    }

    void UpgradeMachineGun()
    {
        currMachineDmg += 0.5f;
        currMachineDelay -= 0.025f;
    }

    void UpgradeShotGun()
    {
        currShotgunDmg += 0.5f;
        currShotgunDelay -= 0.05f;
    }

    void UpgradeBazooka()
    {
        currBazookaDmg += 0.75f;
        currSplashDmg += 0.3f;
        currBazookaDelay -= 0.05f;
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