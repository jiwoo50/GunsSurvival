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

    public static int min = 0;
    public static float sec = 0.0f;
    
    public bool gameOver = false;
    public bool startSpawn = false;
    public bool canShoot = false;

    public float startWait = 3.0f;

    public Text gameoverText;
    public Text readyText;
    public Text healthText;
    public Text timerText;
    public Text upgradeText;
    public Text maxLevelText;
    public Text levelText;

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }
    
    void Start()
    {
        StartCoroutine(ShowReadyText());
        PlayerHealth();
        PlayerLevel();
    }

    void Update()
    {
        if(gameOver && Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("GameOverScene");
        }
        if(startSpawn && !gameOver) Timer();

        if (PlayerController.achieveMaxLevel) ShowMaxLevelText();
        if (PlayerController.completeUpgrade) upgradeText.gameObject.SetActive(false);
    }

    public void PlayerDead()
    {
        gameoverText.gameObject.SetActive(true);
        gameOver = true;
    }

    public void PlayerHealth()
    {
        healthText.text = PlayerController.currHealth.ToString() + "/" + PlayerController.maxHealth.ToString(); 
    }

    public void ScoreReset()
    {
        min = 0;
        sec = 0.0f;
    }

    public void ShowUpgradeText()
    {
        upgradeText.gameObject.SetActive(true);
    }

   public void PlayerLevel()
    {
        levelText.text = "Lv" + PlayerController.currLevel.ToString();
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

    void ShowMaxLevelText()
    {
        maxLevelText.gameObject.SetActive(true);
    }

    IEnumerator ShowReadyText()
    {
        int cnt = 0;
        while(cnt < 3)
        {
            readyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            readyText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            ++cnt;
        }
        startSpawn = true;
        canShoot = true;
    }    
}
