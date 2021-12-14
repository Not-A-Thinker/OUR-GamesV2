using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour
{
    //player 
    [SerializeField] GameObject[] Player1Hp;
    [SerializeField] GameObject[] Player2Hp;
    [SerializeField] Slider Player1Energy;
    [SerializeField] Slider Player2Energy;

    //boss
    [SerializeField] Slider BossHP;

    //respawn 
    [SerializeField] GameObject RespawnLoad;
    [SerializeField] Text LoadingText;
    [SerializeField] Slider slider;
    SceneControl sceneControl;
    //[SerializeField] Text DeadCount;

    //push Bar
    [SerializeField] GameObject pushing;
    [SerializeField] Slider push_slider;

    //GameOver
    //[SerializeField] GameObject GameOverText,YouWinTest,RestartTest;
    //bool IsGameOver, IsWinGame;

    [SerializeField] float smoothing = 5;
    [SerializeField] Text DeadCounter;


    private void FixedUpdate()
    {
        if (BossHP.value == 0)
        {
            YouWin();
        }
    }

    //hp
    public void hp_decrease(int new_hp,int playerCount)
    {
        if(playerCount==1)
            Player1Hp[new_hp].SetActive(false);
        else if(playerCount==2)
            Player2Hp[new_hp].SetActive(false);
    }
    public void hp_increase(int new_hp, int playerCount)
    {
        if (playerCount == 1)
            Player1Hp[new_hp].SetActive(true);
        else if (playerCount == 2)
            Player2Hp[new_hp].SetActive(true);
    }

    //respawn
    public void PlayerRespawn(float load,int count,string playerName)
    {
        RespawnLoad.SetActive(true);
        slider.value = Mathf.Lerp(slider.value, load/2, smoothing * Time.deltaTime);
        count++;
        LoadingText.text= playerName+" Respawning in " + count + "s"; 
       
    }
    public void PlayerRespawnStop()
    {
        RespawnLoad.SetActive(false);

        slider.value = 0;
    }
    public void PlayerHpRefew(string playerName)
    {
        if (playerName == "Player1")
        {
            for (int i = 0; i < Player1Hp.Length; i++)
            {
                Player1Hp[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < Player2Hp.Length; i++)
            {
                Player2Hp[i].SetActive(true);
            }
        }
    }
    public void TotalDead(int DeadCount)
    {
        DeadCounter.text = DeadCount.ToString();
    }

    //pushing
    public void PushingBar(float load)
    {
        pushing.SetActive(true);
        push_slider.value = Mathf.Lerp(push_slider.value, load, smoothing * Time.deltaTime);
    }
    public void PushingStop()
    {
        pushing.SetActive(false);

        push_slider.value = 0;
    }

    //end game
    public void GameOver()
    {
        sceneControl = new SceneControl();
        sceneControl.GameOverScene();
    }
    public void YouWin()
    {
        sceneControl = new SceneControl();
        sceneControl.WinScene();
    }

    public void EnergyBarChange(float NowEnergy, int playerCount)
    {
        if (playerCount == 1)
            Player1Energy.value = NowEnergy * 0.01f;
        else if (playerCount == 2)
            Player2Energy.value = NowEnergy * 0.01f;
    }
}
