﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class PrintScore : MonoBehaviour
{
    public Text scoreText;
    public Text[] scoreList;
    string score;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        if (PlayerPrefs.GetString("BestScore").Length == 0) PlayerPrefs.SetString("BestScore", "00:00");
        if (PlayerPrefs.GetString("SecondScore").Length == 0) PlayerPrefs.SetString("SecondScore", "00:00");
        if (PlayerPrefs.GetString("ThirdScore").Length == 0) PlayerPrefs.SetString("ThirdScore", "00:00");
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
        if (score_min <= Convert.ToInt32(PlayerPrefs.GetString("BestScore").Substring(0, 2)) && score_sec < Convert.ToInt32(PlayerPrefs.GetString("BestScore").Substring(3, 2)))
        {
            if (score_min <= Convert.ToInt32(PlayerPrefs.GetString("SecondScore").Substring(0, 2)) && score_sec < Convert.ToInt32(PlayerPrefs.GetString("SecondScore").Substring(3, 2)))
            {
                if (score_min <= Convert.ToInt32(PlayerPrefs.GetString("ThirdScore").Substring(0, 2)) && score_sec < Convert.ToInt32(PlayerPrefs.GetString("ThirdScore").Substring(3, 2))) return;
                PlayerPrefs.SetString("ThirdScore", score);
                return;
            }
            PlayerPrefs.SetString("ThirdScore", PlayerPrefs.GetString("SecondScore"));
            PlayerPrefs.SetString("SecondScore", score);
            return;
        }
        if (score_min == Convert.ToInt32(PlayerPrefs.GetString("BestScore").Substring(0, 2)) && score_sec == Convert.ToInt32(PlayerPrefs.GetString("BestScore").Substring(3, 2))) return;
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
}
