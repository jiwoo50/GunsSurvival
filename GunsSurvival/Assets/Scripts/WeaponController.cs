using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponController : MonoBehaviour
{
    enum kindOfWeapons { isMachine = 0, isShot, isBazooka }

    public static bool shotGun = false;
    public static float gaugeSpeed = 8.0f;

    public GameObject[] guns; //Machine Gun, Shot Gun, Bazooka 
    public GameObject[] projectile; //Machine Gun bullet, Shot Gun bullet, Bazooka bomb
    public GameObject bulletSpawn;
    public Image machineGauge;
    public Image bazookaGauge;

    Renderer render;
    
    float machineGunDelay = 0.5f;
    float bazookaDelay = 1.0f;
    float changeDelay = 0.5f;
    float nextFire;
    float machineVal = 0.0f;
    float bazookaVal = 0.0f;

    bool[] shootingWeapon = { false, false, false };
    bool isSwitching = false;
    bool isMchineGauge = false;
    bool isBazookaeGauge = false;
    bool machineOverheat = false;
    bool bazookaOverheat = false;

    int cnt = 0;

    void Start()
    {
        render = guns[(int)kindOfWeapons.isShot].GetComponent<Renderer>();
        InitializeWeapon();
    }

    void Update()
    {
        shotGun = shootingWeapon[(int)kindOfWeapons.isShot];
        if (!shotGun)
        {
            guns[(int)kindOfWeapons.isShot].SetActive(true);
            render.material.color = new Color(1, 1, 1, 0);
        }
        else render.material.color = new Color(1, 1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space) && !isSwitching)
        {
            ++cnt;
            if (cnt >= guns.Length) cnt = 0;
            //StopAllCoroutines();
            StartCoroutine(SwitchDelay(cnt));
        }

        Fire();
        MachineGaugeProgress();
        BazookaeGaugeProgress();
    }

    void Fire()
    {
        if (GameController.Instance.canShoot && Input.GetMouseButton(0) && !PauseMenu.gamePaused)
        {
            if (shootingWeapon[(int)kindOfWeapons.isMachine] && Time.time > nextFire && !machineOverheat)
            {
                nextFire = Time.time + machineGunDelay;
                Instantiate(projectile[(int)kindOfWeapons.isMachine], bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                isMchineGauge = true;
                if (machineVal < 100)
                {
                    machineVal += 2.0f;
                    machineOverheat = false;
                }
            }

            if (shootingWeapon[(int)kindOfWeapons.isBazooka] && Time.time > nextFire && !bazookaOverheat)
            {
                nextFire = Time.time + bazookaDelay;
                Instantiate(projectile[(int)kindOfWeapons.isBazooka], bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                isBazookaeGauge = true;
                if (bazookaVal < 100)
                {
                    bazookaVal += 10.0f;
                    bazookaOverheat = false;
                }
            }  
        }
        else
        {
            isMchineGauge = false;
            isBazookaeGauge = false;
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

    void MachineGaugeProgress()
    {
        if (GameController.Instance.canShoot && isMchineGauge)
        {
            if (machineVal >= 100) machineOverheat = true;
        }
        else
        {
            if (machineVal > 0)
            {
                machineVal -= gaugeSpeed * Time.deltaTime;
                machineOverheat = false;
            }
        }
        machineGauge.fillAmount = machineVal / 100;
    }

    void BazookaeGaugeProgress()
    {
        if (GameController.Instance.canShoot && isBazookaeGauge)
        {
            if (bazookaVal >= 91) bazookaOverheat = true;
        }
        else
        {
            if (bazookaVal > 0)
            {
                bazookaVal -= gaugeSpeed * Time.deltaTime;
                bazookaOverheat = false;
            } 
        }
        bazookaGauge.fillAmount = bazookaVal / 100;
    }
}