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
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
            Pause();
        }
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
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
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator CountDown()
    {
        for(int i = 0; i < countDownTxt.Length; i++)
        {
            countDownTxt[i].SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            countDownTxt[i].SetActive(false);
        }
        Time.timeScale = 1.0f;
        gamePaused = false;
    }
}
