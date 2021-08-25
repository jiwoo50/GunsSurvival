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

    public float currTriangleHP = 10.0f;
    public float currRectangleHP = 15.0f;
    public float currPentagonHP = 25.0f;
    public float currTrackingSpeed = 0.0f;
    public float currRushSpeed = 0.0f;
    public int currRushDmg = 5;
    public int currTrackingDmg = 10;
    public int currDivisiveDmg = 15;
    public int maxEnemy = 15;

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

    float triangleHP = 10.0f;
    float rectangleHP = 15.0f;
    float pentagonHP = 25.0f;
    float rushSpeed = 2.0f;
    float trackingSpeed = 2.0f;

    int reinforceCnt;
    int rushDmg = 5;
    int trackingDmg = 10;
    int divisiveDmg = 15;
    int maxEnemyCnt = 15;

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
    }
    
    void Start()
    { 
        StartCoroutine(ShowReadyText());
        PlayerHealth();
        InitializeEnemy();
    }

    void Update()
    {
        if(gameOver && Input.GetKeyDown("space")) SceneManager.LoadScene("GameOverScene");
        if (PlayerController.completeUpgrade) upgradeText.gameObject.SetActive(false);
        if (PlayerController.achieveMaxLevel) ShowMaxLevelText();
        if (reinforceCnt >= 5) CancelInvoke("ReinforceEnemy"); //repeat 5 times
        if (startSpawn && !gameOver) Timer();
        PlayerLevel();
    }

    void InitializeEnemy()
    {
        currTriangleHP = triangleHP;
        currRectangleHP = rectangleHP;
        currPentagonHP = pentagonHP;
        currRushDmg = rushDmg;
        currTrackingDmg = trackingDmg;
        currDivisiveDmg = divisiveDmg;
        currRushSpeed = rushSpeed;
        currTrackingSpeed = trackingSpeed;
        maxEnemy = maxEnemyCnt;
    }

    public void PlayerDead()
    {
        gameoverText.gameObject.SetActive(true);
        gameOver = true;
        InitializeGame();
    }

    public void PlayerHealth()
    {
        healthText.text = PlayerController.currHealth.ToString() + "/" + PlayerController.maxHealth.ToString(); 
    }

    public void InitializeGame()
    {
        PlayerController.exp = 0;
        PlayerController.currLevel = 0;
        if (PlayerController.achieveMaxLevel)
        {
            PlayerController.achieveMaxLevel = false;
            maxLevelText.gameObject.SetActive(false);
        }
        for (int i = 0; i < WeaponController.shootingWeapon.Length; i++) WeaponController.shootingWeapon[i] = false;
        WeaponController.shootingWeapon[0] = true; //machine gun is shootable
    }

    public void ResetScore()
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
            ReinforceEnemy();
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

        currTriangleHP += 3.0f;
        currRectangleHP += 3.0f;
        currPentagonHP += 3.0f;

        currRushDmg += 1; 
        currTrackingDmg += 1;
        currDivisiveDmg += 1;

        currTrackingSpeed += 0.5f;
        currRushSpeed += 0.5f;

        ++maxEnemy;
    }
}
