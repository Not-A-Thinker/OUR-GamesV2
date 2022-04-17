using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private void Update()
    {
        //if (SceneManager.GetActiveScene().buildIndex != 0)
        //{
        //    if (Input.GetButton("Restart"))
        //        ToGameScene();
        //}
    }
    public void WinScene()
    {
        StartCoroutine(Delay(3));
    }
    public void GameOverScene()
    {
        StartCoroutine(Delay(2));
    }
    public void ToGameScene()
    {
        Time.timeScale = 1;
        StartCoroutine(Delay(1));
    }
    public void ToStarScence()
    {
        Time.timeScale = 1;
        StartCoroutine(Delay(0));
    }
    public void ToLearningScence()
    {
        Time.timeScale = 1;
        StartCoroutine(Delay(4));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
   IEnumerator Delay (int Scence)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(Scence);
    }
}
