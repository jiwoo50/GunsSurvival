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

    public static int score = 0;

    public bool gameOver = false;
    public bool startSpawn = false;
    public bool canShoot = false;

    public float startWait = 3.0f;

    public GameObject gameoverText;
    public GameObject readyText;
    public GameObject player;
    
    public Text healthText;
    public Text timerText;

    float sec = 0.0f;
    int min = 0;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        SpawnPlayer();
    }
    
    void Start()
    {
        StartCoroutine(ShowReadyText());
        StartCoroutine(StartWait());
    }

    void Update()
    {
        if(gameOver && Input.GetKeyDown("space"))
        {
            gameOver = false;
            SceneManager.LoadScene("GameOverScene");
        }
        if(startSpawn) Timer();
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

    public void ScoreReset()
    {
        score = 0;
    }

    void SpawnPlayer()
    {
        Instantiate(player, new Vector3(0, 0, -1), Quaternion.identity);
    }

    void Timer()
    {
        sec += Time.deltaTime;
        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
        if((int)sec > 59)
        {
            sec = 0;
            ++min;
        }
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
