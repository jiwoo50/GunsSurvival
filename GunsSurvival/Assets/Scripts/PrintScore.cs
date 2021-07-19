using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PrintScore : MonoBehaviour
{
    public Text score;

    void Start()
    {
        score.text = "Score\n" + string.Format("{0:D2}:{1:D2}", GameController.min, (int)GameController.sec);
    }
}
