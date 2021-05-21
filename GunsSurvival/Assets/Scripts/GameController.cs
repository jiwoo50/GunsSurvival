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

    public GameObject gameoverText;
    public GameObject readyText;

    public bool gameOver = false;

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
    }

    void Update()
    {

    }

    public void PlayerDead()
    {
        gameoverText.SetActive(true);
        gameOver = true;
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
}
