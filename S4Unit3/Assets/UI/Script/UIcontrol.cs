using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject[] Player1Hp;
    [SerializeField] GameObject[] Player2Hp;
    [SerializeField] Slider Player1Energy;
    [SerializeField] Slider Player2Energy;

    [Header("Boss")]
    [SerializeField] Slider BossHP;
    [SerializeField] BossAI_Wind BossAI;

    [Header("Respawn")]
    [SerializeField] GameObject RespawnLoad;
    [SerializeField] Text LoadingText;
    [SerializeField] Slider slider;
    SceneControl sceneControl;
    //[SerializeField] Text DeadCount;

    [Header("Push Bar")]
    [SerializeField] GameObject pushing;
    [SerializeField] Slider push_slider;
    
    [SerializeField] GameObject pushingCD;
    [SerializeField] Slider pushCD_slider;

    [Header("SuckBar")]
    [SerializeField] GameObject SuckingCD;
    [SerializeField] Slider SuckCD_slider;
    //GameOver
    //[SerializeField] GameObject GameOverText,YouWinTest,RestartTest;
    //bool IsGameOver, IsWinGame;
    [Header("Other")]
    [SerializeField] float smoothing = 5;
    [SerializeField] Text DeadCounter;
    [SerializeField] GameObject target;

    private void Start()
    {
        target = GameObject.Find("Player1").gameObject;
    }

    private void Update()
    {
        if (pushing) 
        {
            Vector3 wantedPos = Camera.main.WorldToScreenPoint(target.transform.position);
            wantedPos.y = wantedPos.y -20;
            pushing.transform.position = wantedPos;
        }      
    }

    private void FixedUpdate()
    {
        if (BossHP.value <= 0 && BossAI.IsStage2)
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
        push_slider.value = Mathf.Lerp(load, push_slider.value, smoothing * Time.deltaTime);
    }
    public void PushingStop()
    {
        pushing.SetActive(false);

        push_slider.value = 0;
    }

    //PlayerSuckPushCD
    public void PushingCDBar(float load)
    {
        //pushingCD.SetActive(Ready);
        pushCD_slider.value = Mathf.Lerp(load, pushCD_slider.value, smoothing * Time.deltaTime);
    }
    public void SuckingCDBar(bool Ready)
    {
        SuckingCD.SetActive(Ready);
        //SuckCD_slider.value = Mathf.Lerp(load, SuckCD_slider.value, smoothing * Time.deltaTime);
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
