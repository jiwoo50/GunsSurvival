using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    //shotgun bullet
    public static int shotDamage = 5;
    public GameObject bulletSpawn;
    public GameObject bullet;
    public GameObject player;

    public float bulletSpeed;
    public float shootDelay = 1.0f;

    public int numOfBullets;

    float flyingTime = 5.0f;
    float nextFire;

    Vector3 bulletSpawn_ShiftToAngle = Vector3.zero;

    void Update()
    {
        FireShotgun();
    }

    void FireShotgun()
    {
        if (GameController.Instance.canShoot && Input.GetMouseButton(0))
        {
            if (WeaponController.shotGun)
            {
                float cos = Mathf.Cos(player.transform.rotation.eulerAngles.z + 90);
                float sin = Mathf.Sin(player.transform.rotation.eulerAngles.z + 90);
                ShotGun();
                bulletSpawn_ShiftToAngle.x = bulletSpawn.transform.position.x * cos - bulletSpawn.transform.position.y * sin;
                bulletSpawn_ShiftToAngle.y = bulletSpawn.transform.position.x * sin + bulletSpawn.transform.position.y * cos;
            }
        }
    }
    void Shot()
    {
        for (int i = 0; i < numOfBullets; i++)
        {
            GameObject tempBullet = (GameObject)Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            Rigidbody2D tempBulletRB = tempBullet.GetComponent<Rigidbody2D>();

            float spreadAngle = -10 + 20 * i / numOfBullets;
            float x = bulletSpawn.transform.position.x - player.transform.position.x;
            float y = bulletSpawn.transform.position.y - player.transform.position.y;
            float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + player.transform.rotation.z + 30.0f;

            Vector2 MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
            tempBulletRB.AddForce(MovementDirection * bulletSpeed, ForceMode2D.Impulse);

            Destroy(tempBullet, flyingTime);
        }
    }

    void ShotGun()
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + shootDelay;
            Shot();
        }
    }
}