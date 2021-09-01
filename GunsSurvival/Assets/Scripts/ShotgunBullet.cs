using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunBullet : MonoBehaviour
{
    public static float shotGunGaugeVal = 0.0f;
    public static bool ShotgunGaugeDecrease = false;

    public GameObject bulletSpawn;
    public GameObject bullet;
    public GameObject player;
    public Image shotgunGauge;

    public float bulletSpeed;

    public int numOfBullets;

    float flyingTime = 0.7f;
    float nextFire;

    bool shotgunOverheat = false;
    bool isShotGauge = false;
    
    Vector3 bulletSpawn_ShiftToAngle = Vector3.zero;

    void Start()
    {
        shotGunGaugeVal = 0.0f;
    }

    void Update()
    {
        FireShotgun();
        ShotGunGaugeProgress();
    }

    void FireShotgun()
    {
        if (GameController.Instance.canShoot && !PauseMenu.gamePaused && RotateJoystickController.canFire)
        {
            if (WeaponController.shotGun)
            {
                StopAllCoroutines();
                if (Time.time > nextFire && !shotgunOverheat)
                {
                    nextFire = Time.time + PlayerController.currShotgunDelay;
                    Shot();
                    isShotGauge = true;
                    if (shotGunGaugeVal < 100)
                    {
                        shotGunGaugeVal += 8.0f;
                        shotgunOverheat = false;
                    }

                    float cos = Mathf.Cos(player.transform.rotation.eulerAngles.z + 90);
                    float sin = Mathf.Sin(player.transform.rotation.eulerAngles.z + 90);
                    bulletSpawn_ShiftToAngle.x = bulletSpawn.transform.position.x * cos - bulletSpawn.transform.position.y * sin;
                    bulletSpawn_ShiftToAngle.y = bulletSpawn.transform.position.x * sin + bulletSpawn.transform.position.y * cos;
                }
            }
        }
        else isShotGauge = false;
    }
    void Shot()
    {
        for (int i = 0; i < numOfBullets; i++)
        {
            GameObject tempBullet = (GameObject)Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            Rigidbody2D tempBulletRB = tempBullet.GetComponent<Rigidbody2D>();

            float spreadAngle = -16 + 40 * i / numOfBullets;
            float x = bulletSpawn.transform.position.x - player.transform.position.x;
            float y = bulletSpawn.transform.position.y - player.transform.position.y;
            float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + player.transform.rotation.z + 30.0f;

            Vector2 MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
            tempBulletRB.AddForce(MovementDirection * bulletSpeed, ForceMode2D.Impulse);

            Destroy(tempBullet, flyingTime);
        }
    }

    void ShotGunGaugeProgress()
    {
        if (GameController.Instance.canShoot && isShotGauge)
        {
            if (shotGunGaugeVal >= 93) shotgunOverheat = true;
        }
        else
        {
            if (shotGunGaugeVal > 0 && ShotgunGaugeDecrease)
            {
                shotGunGaugeVal -= WeaponController.gaugeSpeed * Time.deltaTime;
                shotgunOverheat = false;
            }
        }
        shotgunGauge.fillAmount = shotGunGaugeVal / 100;
    }
}