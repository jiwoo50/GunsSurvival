using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject laser;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MachineGun());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MachineGun()
    {
        while (true)
        {
            Instantiate(laser, new Vector2(transform.position.x, transform.position.y) + Vector2.up * 0.5f, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
