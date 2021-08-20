using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BazookaBomb : MonoBehaviour
{
    public static float bombDamage = 15.0f;
    public static float splashDamage = 4.0f;
    public static float bazookaShotDelay = 1.5f;
    public static float bazookaVal = 0.0f;

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

    void Update()
    {
        FireBazooka();
        BazookaeGaugeProgress();
    }
    
    void FireBazooka()
    {
        if (GameController.Instance.canShoot && Input.GetMouseButton(0) && !PauseMenu.gamePaused)
        {
            if (WeaponController.bazooka)
            {
                StopAllCoroutines();
                if (Time.time > nextFire && !bazookaOverheat)
                {
                    nextFire = Time.time + bazookaShotDelay;
                    Shot();
                    isBazookaeGauge = true;
                    if (bazookaVal < 100)
                    {
                        bazookaVal += 10.0f;
                        bazookaOverheat = false;
                    }
                }
            }
        }
        else isBazookaeGauge = false;

        if (WeaponController.bazooka && Input.GetMouseButtonUp(0)) StartCoroutine(BazookaGaugeDecrease());
    }

    void Shot()
    {
        GameObject bazookaBomb = Instantiate(bomb, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        Rigidbody2D rb2d = bazookaBomb.GetComponent<Rigidbody2D>();
        rb2d.AddForce(this.gameObject.transform.up * bombSpeed, ForceMode2D.Impulse);

        Destroy(bazookaBomb, flyingTime);
    }

    void BazookaeGaugeProgress()
    {
        if (GameController.Instance.canShoot && isBazookaeGauge)
        {
            if (bazookaVal >= 91) bazookaOverheat = true;
        }
        else
        {
            if (bazookaVal > 0 && bazookaGaugeDecrease)
            {
                bazookaVal -= WeaponController.gaugeSpeed * Time.deltaTime;
                bazookaOverheat = false;
            }
        }
        bazookaGauge.fillAmount = bazookaVal / 100;
    }

    IEnumerator BazookaGaugeDecrease() //delay occurs when stop shooting
    {
        bazookaGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f); //delay : 2 seconds
        bazookaGaugeDecrease = true;
    }
}
