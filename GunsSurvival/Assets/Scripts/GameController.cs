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
    public static float triangleHP = 10;
    public static float rectangleHP = 15;
    public static float pentagonHP = 25;

    public float startWait = 3.0f;

    public bool gameOver = false;
    public bool startSpawn = false;
    public bool canShoot = false;

    public Text gameoverText;
    public Text readyText;
    public Text healthText;
    public Text timerText;
    public Text upgradeText;
    public Text maxLevelText;
    public Text levelText;

    int reinforceCnt;

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
    }
    
    void Start()
    { 
        StartCoroutine(ShowReadyText());
        InvokeRepeating("ReinforceEnemy", startWait, 60.0f);
        PlayerHealth();
        PlayerLevel();
    }

    void Update()
    {
        if(gameOver && Input.GetKeyDown("space")) SceneManager.LoadScene("GameOverScene");
        if (PlayerController.completeUpgrade) upgradeText.gameObject.SetActive(false);
        if (PlayerController.achieveMaxLevel) ShowMaxLevelText();
        if (reinforceCnt >= 5) CancelInvoke("ReinforceEnemy"); //repeat 5 times
        if (startSpawn && !gameOver) Timer();
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

    void ReinforceEnemy() //every 1 minutes, enemies are reinforeced(get more HP, damage and speed)
    {
        ++reinforceCnt;
        triangleHP += 3.0f;
        rectangleHP += 3.0f;
        pentagonHP += 3.0f;
        PlayerController.rushDamage += 1; 
        PlayerController.trackingDamage += 1;
        PlayerController.divisiveEnemyDamage += 1;
        TrackingEnemyMove.movePower += 0.5f;
        RushMove.movePower += 0.5f;
    }
}
