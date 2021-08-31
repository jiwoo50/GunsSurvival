using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachinegunBullet : MonoBehaviour
{
    public static float machineGunGaugeVal = 0.0f;

    public GameObject player;
    public GameObject bulletSpawn;
    public GameObject bullet;
    public Image machineGauge;

    public float bulletSpeed;

    float flyingTime = 0.35f;
    float nextFire;

    bool machineOverheat = false;
    bool isMachineGauge = false;
    bool machineGaugeDecrease = false;

    void Start()
    {
        machineGunGaugeVal = 0.0f;
    }

    void Update()
    {
        FireMachineGun();
        MachineGaugeProgress();
    }

    void FireMachineGun()
    {
        if (GameController.Instance.canShoot && JoystickController.fireBullets && !PauseMenu.gamePaused)
        {
            if (WeaponController.machineGun)
            {
                StopAllCoroutines();
                if (Time.time > nextFire && !machineOverheat)
                {
                    nextFire = Time.time + PlayerController.currMachineDelay;
                    Shot();
                    isMachineGauge = true;
                    if (machineGunGaugeVal < 100)
                    {
                        machineGunGaugeVal += 2.0f;
                        machineOverheat = false;
                    }
                }
            }
        }
        else isMachineGauge = false;

        if (WeaponController.machineGun && !JoystickController.fireBullets) StartCoroutine(MachineGaugeDecrease());
    }

    void Shot()
    {
        GameObject machineBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        Rigidbody2D rb2d = machineBullet.GetComponent<Rigidbody2D>();
        rb2d.AddForce(this.gameObject.transform.up * bulletSpeed, ForceMode2D.Impulse);

        Destroy(machineBullet, flyingTime);
    }

    void MachineGaugeProgress()
    {
        if (GameController.Instance.canShoot && isMachineGauge)
        {
            if (machineGunGaugeVal >= 100) machineOverheat = true;
        }
        else
        {
            if (machineGunGaugeVal > 0 && machineGaugeDecrease)
            {
                machineGunGaugeVal -= WeaponController.gaugeSpeed * Time.deltaTime;
                machineOverheat = false;
            }
        }
        machineGauge.fillAmount = machineGunGaugeVal / 100;
    }
    IEnumerator MachineGaugeDecrease() //delay occurs when stop shooting
    {
        machineGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f); //delay : 2 seconds
        machineGaugeDecrease = true;
    }
}