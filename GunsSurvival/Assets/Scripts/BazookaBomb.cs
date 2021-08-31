using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BazookaBomb : MonoBehaviour
{
    public static float bazookaGaugeVal = 0.0f;

    public GameObject player;
    public GameObject bulletSpawn;
    public GameObject bomb;
    public Image bazookaGauge;

    public float bombSpeed;

    float flyingTime = 1.0f;
    float nextFire;

    bool bazookaOverheat = false;
    bool isBazookaeGauge = false;
    bool bazookaGaugeDecrease = false;

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
        if (GameController.Instance.canShoot && JoystickController.fireBullets && !PauseMenu.gamePaused)
        {
            if (WeaponController.bazooka)
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

        if (WeaponController.bazooka && !JoystickController.fireBullets) StartCoroutine(BazookaGaugeDecrease());
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

    IEnumerator BazookaGaugeDecrease() //delay occurs when stop shooting
    {
        bazookaGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f); //delay : 2 seconds
        bazookaGaugeDecrease = true;
    }
}
