using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    enum kindOfWeapons { isMachine = 0, isShot, isBazooka }

    public static bool[] shootingWeapon = { false, false, false };
    public static bool shotGun = false;
    public static bool bazooka = false;
    public static bool machineGun = false;
    public static float gaugeSpeed = 8.0f;

    public GameObject[] guns; //Machine Gun, Shot Gun, Bazooka 

    float changeDelay = 1.25f;
    bool isSwitching = false;
    int cnt = 0;

    void Start()
    {
        InitializeWeapon();
    }

    void Update()
    {
        shotGun = shootingWeapon[(int)kindOfWeapons.isShot];
        machineGun = shootingWeapon[(int)kindOfWeapons.isMachine];
        bazooka = shootingWeapon[(int)kindOfWeapons.isBazooka];

        /*if (GameController.Instance.canShoot && Input.GetKeyDown(KeyCode.Space) && !isSwitching)
        {
            SoundController.Instance.PlayReloadSound();
            ++cnt;
            if (cnt >= guns.Length) cnt = 0;
            StartCoroutine(SwitchDelay(cnt));
        }*/
    }

    public void Func()
    {
        if (GameController.Instance.canShoot && !isSwitching)
        {
            SoundController.Instance.PlayReloadSound();
            ++cnt;
            if (cnt >= guns.Length) cnt = 0;
            StartCoroutine(SwitchDelay(cnt));
        }
    }

    void InitializeWeapon()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(true); //all guns are set active
            shootingWeapon[i] = false; //all guns are not shootable
            Renderer render = guns[i].GetComponent<Renderer>();
            render.material.color = new Color(1, 1, 1, 0); //all guns are transparent in color.
        }
        Renderer mRender = guns[(int)kindOfWeapons.isMachine].GetComponent<Renderer>();
        mRender.material.color = new Color(1, 1, 1, 1); //only machine gun gets own color
        shootingWeapon[(int)kindOfWeapons.isMachine] = true;
    }

    IEnumerator SwitchDelay(int idx)
    {
        isSwitching = true;
        SwitchWeapon(idx);
        yield return new WaitForSeconds(changeDelay);
        shootingWeapon[idx] = true;
        isSwitching = false;
    }

    void SwitchWeapon(int idx)
    {
        Renderer idxRender = guns[idx].GetComponent<Renderer>();
        for (int i = 0; i < guns.Length; i++)
        {
            Renderer render = guns[i].GetComponent<Renderer>();
            render.material.color = new Color(1, 1, 1, 0);
            shootingWeapon[i] = false;
        }
        idxRender.material.color = new Color(1, 1, 1, 1);
    }
}