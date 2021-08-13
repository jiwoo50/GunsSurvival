using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenu;
    public GameObject[] countDownTxt; //3->2->1

    void Update()
    {
        if (gamePaused) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused) Pause();
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        gamePaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        StartCoroutine(CountDown());
    }

    public void ToTitle()
    {
        GameController.Instance.gameOver = true;
        gamePaused = false;
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator CountDown()
    {
        for(int i = 0; i < countDownTxt.Length; i++)
        {
            countDownTxt[i].SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            countDownTxt[i].SetActive(false);
        }
        gamePaused = false;
    }
}
