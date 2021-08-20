using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachinegunBullet : MonoBehaviour
{
    public static float bulletDamage = 7.0f;
    public static float machineGunShotDelay = 0.75f;
    public static float machineVal = 0.0f;

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

    void Update()
    {
        FireMachineGun();
        MachineGaugeProgress();
    }

    void FireMachineGun()
    {
        if (GameController.Instance.canShoot && Input.GetMouseButton(0) && !PauseMenu.gamePaused)
        {
            if (WeaponController.machineGun)
            {
                StopAllCoroutines();
                if (Time.time > nextFire && !machineOverheat)
                {
                    nextFire = Time.time + machineGunShotDelay;
                    Shot();
                    isMachineGauge = true;
                    if (machineVal < 100)
                    {
                        machineVal += 2.0f;
                        machineOverheat = false;
                    }
                }
            }
        }
        else isMachineGauge = false;

        if (WeaponController.machineGun && Input.GetMouseButtonUp(0)) StartCoroutine(MachineGaugeDecrease());
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
            if (machineVal >= 100) machineOverheat = true;
        }
        else
        {
            if (machineVal > 0 && machineGaugeDecrease)
            {
                machineVal -= WeaponController.gaugeSpeed * Time.deltaTime;
                machineOverheat = false;
            }
        }
        machineGauge.fillAmount = machineVal / 100;
    }
    IEnumerator MachineGaugeDecrease() //delay occurs when stop shooting
    {
        machineGaugeDecrease = false;
        yield return new WaitForSeconds(2.0f); //delay : 2 seconds
        machineGaugeDecrease = true;
    }
}
