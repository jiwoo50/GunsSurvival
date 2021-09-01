using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BazookaBomb : MonoBehaviour
{
    public static float bazookaGaugeVal = 0.0f;
    public static bool bazookaGaugeDecrease = false;

    public GameObject player;
    public GameObject bulletSpawn;
    public GameObject bomb;
    public Image bazookaGauge;

    public float bombSpeed;

    float flyingTime = 1.0f;
    float nextFire;

    bool bazookaOverheat = false;
    bool isBazookaeGauge = false;

    void Start()
    {
        bazookaGaugeVal = 0.0f;
    }

    void Update()
    {
        FireBazooka();
        BazookaeGaugeProgress();
    }
    
    void FireBazooka()
    {
        if (GameController.Instance.canShoot && !PauseMenu.gamePaused && RotateJoystickController.canFire)
        {
            if (WeaponController.bazooka && !WeaponController.isSwitching)
            {
                StopAllCoroutines();
                if (Time.time > nextFire && !bazookaOverheat)
                {
                    nextFire = Time.time + PlayerController.currBazookaDelay;
                    Shot();
                    isBazookaeGauge = true;
                    if (bazookaGaugeVal < 100)
                    {
                        bazookaGaugeVal += 10.0f;
                        bazookaOverheat = false;
                    }
                }
            }
        }
        else isBazookaeGauge = false;
    }

    void Shot()
    {
        GameObject bazookaBomb = Instantiate(bomb, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        SoundController.Instance.PlayBazookaSound();
        Rigidbody2D rb2d = bazookaBomb.GetComponent<Rigidbody2D>();
        rb2d.AddForce(this.gameObject.transform.up * bombSpeed, ForceMode2D.Impulse);

        Destroy(bazookaBomb, flyingTime);
    }

    void BazookaeGaugeProgress()
    {
        if (GameController.Instance.canShoot && isBazookaeGauge)
        {
            if (bazookaGaugeVal >= 91) bazookaOverheat = true;
        }
        else
        {
            if (bazookaGaugeVal > 0 && bazookaGaugeDecrease)
            {
                bazookaGaugeVal -= WeaponController.gaugeSpeed * Time.deltaTime;
                bazookaOverheat = false;
            }
        }
        bazookaGauge.fillAmount = bazookaGaugeVal / 100;
    }
}
