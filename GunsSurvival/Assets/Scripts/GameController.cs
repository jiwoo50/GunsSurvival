using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance
    {
        get
        {
            if (!instance) return null;
            return instance;
        }
    }
    private static GameController instance = null;

    public bool gameOver = false;
    public bool startSpawn = false;
    public bool canShoot = false;

    public float startWait = 3.0f;

    public GameObject gameoverText;
    public GameObject readyText;
    
    public Text healthText;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    
    void Start()
    {
        StartCoroutine(ShowReadyText());
        StartCoroutine(StartWait());
    }

    public void PlayerDead()
    {
        gameoverText.SetActive(true);
        gameOver = true;
    }

    public void PlayerHealth()
    {
        healthText.text = PlayerController.currentHealth + "/" + PlayerController.maxHealth;
    }

    IEnumerator ShowReadyText()
    {
        int cnt = 0;
        while(cnt < 3)
        {
            readyText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            readyText.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            ++cnt;
        }
    }

    IEnumerator StartWait()
    {
        startSpawn = false;
        canShoot = false;
        yield return new WaitForSeconds(startWait);
        startSpawn = true;
        canShoot = true;
    }
}
