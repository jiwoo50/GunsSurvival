using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponController : MonoBehaviour
{
    enum kindOfWeapons { isMachine = 0, isShot, isBazooka }

    public static bool shotGun = false;
    public static bool shotgunFire = false;

    public GameObject[] guns; //Machine Gun, Shot Gun, Bazooka 
    public GameObject[] projectile; //Machine Gun bullet, Shot Gun bullet, Bazooka bomb
    public GameObject bulletSpawn;

    float machineGunDelay = 0.5f;
    bool machineFire = false;
    float bazookaDelay = 1.5f;
    bool bazookaFire = false;
    float changeDelay = 0.5f;

    int cnt = 0;

    bool[] shootingWeapon = { false, false, false };
    bool isSwitching = false;

    void Start()
    {
        InitializeWeapon();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSwitching)
        {
            ++cnt;
            if (cnt >= guns.Length) cnt = 0;
            StopAllCoroutines();
            StartCoroutine(SwitchDelay(cnt));
            machineFire = false;
            bazookaFire = false;
            shotgunFire = false;
        }
        shotGun = shootingWeapon[(int)kindOfWeapons.isShot];
        Fire();
    }
    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if (shootingWeapon[(int)kindOfWeapons.isMachine]) StartCoroutine(FireBullet());
            if (shootingWeapon[(int)kindOfWeapons.isBazooka]) StartCoroutine(FireBomb());
        }
    }
    void MachineGun()
    {
        Instantiate(projectile[(int)kindOfWeapons.isMachine], bulletSpawn.transform.position, bulletSpawn.transform.rotation);
    }
    void Bazooka()
    {
        Instantiate(projectile[(int)kindOfWeapons.isBazooka], bulletSpawn.transform.position, bulletSpawn.transform.rotation);
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

    IEnumerator FireBullet()
    {
        if (!machineFire)
        {
            machineFire = true;
            MachineGun();
            yield return new WaitForSeconds(machineGunDelay);
            machineFire = false;
        }
    }

    IEnumerator FireBomb()
    {
        if (!bazookaFire)
        {
            bazookaFire = true;
            Bazooka();
            yield return new WaitForSeconds(bazookaDelay);
            bazookaFire = false;
        }
    }
}