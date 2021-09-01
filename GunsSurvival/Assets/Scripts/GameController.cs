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

    public static float sec = 0.0f;
    public static float currTriangleHP = 10.0f;
    public static float currRectangleHP = 15.0f;
    public static float currPentagonHP = 25.0f;
    public static float currTrackingSpeed = 2.0f;
    public static float currRushSpeed = 2.0f;
    public static int currRushDmg = 5;
    public static int currTrackingDmg = 10;
    public static int currDivisiveDmg = 15;
    public static int maxEnemy = 15;
    public static int min = 0;

    public float triangleHP = 10.0f;
    public float rectangleHP = 15.0f;
    public float pentagonHP = 25.0f;
    public float rushSpeed = 2.0f;
    public float trackingSpeed = 2.0f;

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
    public Text overUpgradeText;

    public int rushDmg = 5;
    public int trackingDmg = 10;
    public int divisiveDmg = 15;
    public int maxEnemyCnt = 15;

    int reinforceCnt;

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
        if(gameOver && Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("GameOverScene");
        if (PlayerController.choosingUpgrade) upgradeText.gameObject.SetActive(true);
        if (PlayerController.completeUpgrade) upgradeText.gameObject.SetActive(false);
        if (PlayerController.achieveMaxLevel) ShowMaxLevelText();
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
        SetInactiveMaxLevelText();
    }

    public void PlayerHealth()
    {
        healthText.text = PlayerController.currHealth.ToString() + "/" + PlayerController.maxHealth.ToString(); 
    }

    public void SetInactiveMaxLevelText()
    {
        if (PlayerController.achieveMaxLevel)
        {
            PlayerController.achieveMaxLevel = false;
            maxLevelText.gameObject.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        min = 0;
        sec = 0.0f;
    }

   public void PlayerLevel()
   {
        levelText.text = "Lv" + PlayerController.currLevel.ToString();
   }

    public IEnumerator CannotUpgrade()
    {
        overUpgradeText.gameObject.SetActive(true);
        overUpgradeText.color = new Color(overUpgradeText.color.r, overUpgradeText.color.g, overUpgradeText.color.b, 1);
        while(overUpgradeText.color.a > 0.0f)
        {
            overUpgradeText.color = new Color(overUpgradeText.color.r, overUpgradeText.color.g, overUpgradeText.color.b, overUpgradeText.color.a - (Time.deltaTime/2.0f));
            yield return null;
        }
        overUpgradeText.gameObject.SetActive(false);
    }

    void Timer()
    {
        sec += Time.deltaTime;
        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
        if((int)sec > 59)
        {
            sec = 0;
            ++min;
            if (reinforceCnt < 5) ReinforceEnemy();
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
