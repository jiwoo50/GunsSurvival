﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponController : MonoBehaviour
{
    enum kindOfWeapons { isMachine = 0, isShot, isBazooka }

    public GameObject[] guns; //Machine Gun, Shot Gun, Bazooka 
    public GameObject[] projectile; //Machine Gun bullet, Shot Gun bullet, Bazooka bomb
    public GameObject bulletSpawn;

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
                InvokeRepeating("MachineGun", 0, machineGunDelay);
            }
            if (shootingWeapon[(int)kindOfWeapons.isShot] && shootFlag)
            {
                InvokeRepeating("ShotGun", 0, shotGunDelay);
            }
            if (shootingWeapon[(int)kindOfWeapons.isBazooka] && shootFlag)
            {
                InvokeRepeating("Bazooka", 0, bazookaDelay);
            }
            shootFlag = false;
        }
        else
        {
            CancelInvoke("MachineGun");
            CancelInvoke("ShotGun");
            CancelInvoke("Bazooka");
            shootFlag = true;
        }
        
    } 

    void MachineGun()
    {
        Instantiate(projectile[(int)kindOfWeapons.isMachine], bulletSpawn.transform.position, Quaternion.identity);
    }
    void ShotGun()
    {
        Instantiate(projectile[(int)kindOfWeapons.isShot], bulletSpawn.transform.position, Quaternion.identity);
    }
    void Bazooka()
    {
        Instantiate(projectile[(int)kindOfWeapons.isBazooka], bulletSpawn.transform.position, Quaternion.identity);
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