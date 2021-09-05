using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class PrintScore : MonoBehaviour
{
    public Text scoreText;
    public GameObject newRecord;
    public Text[] scoreList;

    string score;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        Initialize();
    }

    void Initialize()
    {
        if (!PlayerPrefs.HasKey("BestScore")) PlayerPrefs.SetString("BestScore", "00:00");
        if (!PlayerPrefs.HasKey("SecondScore")) PlayerPrefs.SetString("SecondScore", "00:00");
        if (!PlayerPrefs.HasKey("ThirdScore")) PlayerPrefs.SetString("ThirdScore", "00:00");
    }

    void Start()
    {
        score = string.Format("{0:D2}:{1:D2}", GameController.min, (int)GameController.sec);
        scoreText.text = "Score\n" + score;
        SaveScore();
        LoadScore();
    }

    void SaveScore()
    {
        int score_min = Convert.ToInt32(score.Substring(0, 2));
        int score_sec = Convert.ToInt32(score.Substring(3, 2));
        int best_min = Convert.ToInt32(PlayerPrefs.GetString("BestScore").Substring(0, 2));
        int best_sec = Convert.ToInt32(PlayerPrefs.GetString("BestScore").Substring(3, 2));
        int second_min = Convert.ToInt32(PlayerPrefs.GetString("SecondScore").Substring(0, 2));
        int second_sec = Convert.ToInt32(PlayerPrefs.GetString("SecondScore").Substring(3, 2));
        int third_min = Convert.ToInt32(PlayerPrefs.GetString("ThirdScore").Substring(0, 2));
        int third_sec = Convert.ToInt32(PlayerPrefs.GetString("ThirdScore").Substring(3, 2));

        if (score_min == best_min && score_sec == best_sec) return;
        if (score_min <= best_min && score_sec < best_sec)
        {
            if (score_min <= second_min && score_sec < second_sec)
            {
                if (score_min <= third_min && score_sec < third_sec) return;
                PlayerPrefs.SetString("ThirdScore", score);
                return;
            }
            PlayerPrefs.SetString("ThirdScore", PlayerPrefs.GetString("SecondScore"));
            PlayerPrefs.SetString("SecondScore", score);
            return;
        }
        StartCoroutine(NewRecord());
        PlayerPrefs.SetString("ThirdScore", PlayerPrefs.GetString("SecondScore"));
        PlayerPrefs.SetString("SecondScore", PlayerPrefs.GetString("BestScore"));
        PlayerPrefs.SetString("BestScore", score);
    }

    void LoadScore()
    {
        scoreList[0].text = PlayerPrefs.GetString("BestScore").ToString();
        scoreList[1].text = PlayerPrefs.GetString("SecondScore").ToString();
        scoreList[2].text = PlayerPrefs.GetString("ThirdScore").ToString();
    }

    IEnumerator NewRecord()
    {
        while (true)
        {
            newRecord.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            newRecord.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
