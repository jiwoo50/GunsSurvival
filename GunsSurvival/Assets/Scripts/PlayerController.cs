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
    public static bool choosingUpgrade = false;
    public static bool completeUpgrade = false;
    public static bool isInvincible = false;
    public static int currLevel = 0;
    public static int currHealth;
    public static int maxHealth = 30;
  
    public Boundary boundary;
    public GameObject explosionPrefab;
    public Image expBar;

    public float moveSpeed = 5.0f;
    public float timeInvincible = 2.0f;

    float machineDmg = 7.0f;
    float machineDelay = 0.75f;
    float shotDmg = 4.0f;
    float shotDelay = 1.2f;
    float bazookaDmg = 15.0f;
    float splashDmg = 4.0f;
    float bazookaDelay = 1.5f;

    int maxLevel = 10;
    int machineGunUpgradeCnt = 0;
    int shotGunUpgradeCnt = 0;
    int bazookaUpgradeCnt = 0; //max : 4

    Rigidbody2D rb2d;
    Renderer playerRenderer;
    Renderer weaponRenderer;
    GameObject activeWeapon;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerRenderer = this.gameObject.GetComponent<Renderer>();
        Initialize();

        GameController.Instance.ResetTimer();
        GameController.Instance.SetInactiveMaxLevelText();
    }

    void Update()
    {
        activeWeapon = this.transform.GetChild(0).GetChild(WeaponController.activeWeaponIdx).gameObject;
        if (currLevel == maxLevel)
        {
            expBar.fillAmount = 1.0f;
            achieveMaxLevel = true;
        }
        if (!achieveMaxLevel && !choosingUpgrade) GetEXP();
    }

    void Initialize()
    {
        exp = 0;
        currLevel = 0;
        currHealth = maxHealth;
        currShotgunDmg = shotDmg;
        currSplashDmg = splashDmg;
        currMachineDmg = machineDmg;
        currBazookaDmg = bazookaDmg;
        currShotgunDelay = shotDelay;
        currMachineDelay = machineDelay;
        currBazookaDelay = bazookaDelay;
    }

    void FinishUpgrade()
    {
        completeUpgrade = true;
        choosingUpgrade = false;
        exp = 0;
        ++currLevel;
    }

    public void PlayerMove(Vector3 pos, float distance)
    {
        Vector3 position = rb2d.position;
        position.x += pos.x * moveSpeed * distance * Time.deltaTime;
        position.y += pos.y * moveSpeed * distance * Time.deltaTime;
        position.z = -1.0f;

        position.x = Mathf.Clamp(position.x, boundary.xMin, boundary.xMax);
        position.y = Mathf.Clamp(position.y, boundary.yMin, boundary.yMax);

        rb2d.MovePosition(position);
    }

    public void PlayerRotate(Vector2 pos)
    {
        float theta = Mathf.Atan2(pos.x, pos.y) * Mathf.Rad2Deg;
        this.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, -theta);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rush")) ChangeHealth(-GameController.currRushDmg);       
        if (collision.gameObject.CompareTag("Tracking")) ChangeHealth(-GameController.currTrackingDmg);
        if (collision.gameObject.CompareTag("Divisive")) ChangeHealth(-GameController.currDivisiveDmg);
        else return;
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
                            if (MachinegunBullet.machineGunGaugeVal <= 15.0f) MachinegunBullet.machineGunGaugeVal = 0.0f;
                            else MachinegunBullet.machineGunGaugeVal -= 15.0f;
                            break;
                        case 1: //shot gun
                            if (ShotgunBullet.shotGunGaugeVal <= 15.0f) ShotgunBullet.shotGunGaugeVal = 0.0f;
                            else ShotgunBullet.shotGunGaugeVal -= 15.0f;
                            break;
                        case 2: //bazooka
                            if (BazookaBomb.bazookaGaugeVal <= 15.0f) BazookaBomb.bazookaGaugeVal = 0.0f;
                            else BazookaBomb.bazookaGaugeVal -= 15.0f;
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
        if (exp >= 100 && currLevel < maxLevel)
        {
            completeUpgrade = false;
            choosingUpgrade = true;
            GameController.Instance.PlayerLevel();
        }
        expBar.fillAmount = exp / 100;
    }

    public void UpgradeMachineGun()
    {
        if (machineGunUpgradeCnt >= 4)
        {
            GameController.Instance.StartCoroutine(GameController.Instance.CannotUpgrade());
            return;
        }
        currMachineDmg += 1.0f;
        currMachineDelay -= 0.05f;
        ++machineGunUpgradeCnt;
        SoundController.Instance.PlayUpgradeSound();
        FinishUpgrade();
    }

    public void UpgradeShotGun()
    {
        if (shotGunUpgradeCnt >= 4)
        {
            GameController.Instance.StartCoroutine(GameController.Instance.CannotUpgrade());
            return;
        }
        currShotgunDmg += 1.25f;
        currShotgunDelay -= 0.08f;
        ++shotGunUpgradeCnt;
        SoundController.Instance.PlayUpgradeSound();
        FinishUpgrade();
    }

    public void UpgradeBazooka()
    {
        if (bazookaUpgradeCnt >= 4)
        {
            GameController.Instance.StartCoroutine(GameController.Instance.CannotUpgrade());
            return;
        }
        currBazookaDmg += 1.5f;
        currSplashDmg += 0.5f;
        currBazookaDelay -= 0.1f;
        ++bazookaUpgradeCnt;
        SoundController.Instance.PlayUpgradeSound();
        FinishUpgrade();
    }

    IEnumerator OnDamage()
    {
        int cnt = 0;
        isInvincible = true;
        weaponRenderer = activeWeapon.GetComponent<Renderer>();
        while (cnt < 4)
        {
            playerRenderer.material.color = new Color(1, 1, 1, 0);
            weaponRenderer.material.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.25f);
            playerRenderer.material.color = new Color(1, 1, 1, 1);
            weaponRenderer.material.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.25f);
            ++cnt;
        }
        isInvincible = false;
    }
}