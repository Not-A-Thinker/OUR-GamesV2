using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            ToGameScene();
    }

    public void WinScene()
    {
        SceneManager.LoadScene(3);
    }
    public void GameOverScene()
    {
        SceneManager.LoadScene(2);
    }
    public void ToGameScene()
    {
        SceneManager.LoadScene(1);
    }
    public void ToStarScence()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
