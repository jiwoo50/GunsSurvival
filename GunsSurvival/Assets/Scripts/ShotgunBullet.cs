using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    //shotgun bullet
    public GameObject bulletSpawn;
    public GameObject bullet;
    public GameObject player;

    public float bulletSpeed;
    public int numOfBullets;

    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Shot();
        }
    }
    void Shot()
    {
        for (int i = 0; i < numOfBullets; i++)
        {
            //총알 토큰 생성
            GameObject tempBullet = (GameObject)Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            //총알에 물리 부여
            Rigidbody2D tempBulletRB = tempBullet.GetComponent<Rigidbody2D>();
            float spreadAngle = -10+20*i/numOfBullets;//총알의 각도
            float x = bulletSpawn.transform.position.x - player.transform.position.x;
            float y = bulletSpawn.transform.position.y - player.transform.position.y;
            float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg)*3/2;//발사되는 각 계산
            Vector2 MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
            tempBulletRB.AddForce(MovementDirection * bulletSpeed, ForceMode2D.Impulse);
            Destroy(tempBullet, 5.0f);
        }
    }
}
