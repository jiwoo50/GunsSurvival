using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponController : MonoBehaviour
{
    enum kindOfWeapons { isMachine = 0, isShot, isBazooka }

    public GameObject[] guns; //Machine Gun, Shot Gun, Bazooka 
    public GameObject[] projectile; //Machine Gun bullet, Shot Gun bullet, Bazooka bomb

    float machineGunDelay = 0.5f;
    float shotGunDelay = 1.0f;
    float bazookaDelay = 1.5f;
    float changeDelay = 0.5f;

    int cnt = 0;

    bool[] shootingWeapon = { false, false, false };
    bool shootFlag = true;
    bool isSwitching = false;

    void Start()
    {
        InitializeWeapon();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isSwitching)
        {
            ++cnt;
            if (cnt >= guns.Length) cnt = 0;
            StopAllCoroutines();
            StartCoroutine(SwitchDelay(cnt));
            shootFlag = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            if (shootingWeapon[(int)kindOfWeapons.isMachine] && shootFlag)
            {
                StartCoroutine(MachineGun());
            }
            if (shootingWeapon[(int)kindOfWeapons.isShot] && shootFlag)
            {
                StartCoroutine(ShotGun());
            }
            if (shootingWeapon[(int)kindOfWeapons.isBazooka] && shootFlag)
            {
                StartCoroutine(Bazooka());
            }
            shootFlag = false;
        }
    }

    IEnumerator MachineGun()
    {
        while (true)
        {
            Instantiate(projectile[(int)kindOfWeapons.isMachine], new Vector2(transform.position.x, transform.position.y) + Vector2.up * 0.5f, Quaternion.identity);
            yield return new WaitForSeconds(machineGunDelay);
        }
    }

    IEnumerator ShotGun()
    {
        while (true)
        {
            Instantiate(projectile[(int)kindOfWeapons.isShot], new Vector2(transform.position.x, transform.position.y) + Vector2.up * 0.5f, Quaternion.identity);
            yield return new WaitForSeconds(shotGunDelay);
        } 
    }

    IEnumerator Bazooka()
    {
        while (true)
        {
            Instantiate(projectile[(int)kindOfWeapons.isBazooka], new Vector2(transform.position.x, transform.position.y) + Vector2.up * 0.5f, Quaternion.identity);
            yield return new WaitForSeconds(bazookaDelay);
        }
    }

    void InitializeWeapon()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
        guns[(int)kindOfWeapons.isMachine].SetActive(true);
        shootingWeapon[(int)kindOfWeapons.isMachine] = true;
    }

    IEnumerator SwitchDelay(int idx)
    {
        isSwitching = true;
        SwitchWeapon(idx);
        yield return new WaitForSeconds(changeDelay);
        isSwitching = false;
    }

    void SwitchWeapon(int idx)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
            shootingWeapon[i] = false;
        }
        guns[idx].SetActive(true);
        shootingWeapon[idx] = true;
    }
}