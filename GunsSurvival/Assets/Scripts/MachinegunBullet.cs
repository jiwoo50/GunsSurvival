using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachinegunBullet : MonoBehaviour
{
    public static float machineGunGaugeVal = 0.0f;
    public static bool machineGaugeDecrease = true;

    public GameObject player;
    public GameObject bulletSpawn;
    public GameObject bullet;
    public Image machineGauge;

    public float bulletSpeed;

    float flyingTime = 0.35f;
    float nextFire;

    bool machineOverheat = false;
    bool isMachineGauge = false;
    
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
        if (GameController.Instance.canShoot && !PauseMenu.gamePaused && RotateJoystickController.canFire)
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
    }

    void Shot()
    {
        GameObject machineBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        SoundController.Instance.PlayMachineGunSound();
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
}