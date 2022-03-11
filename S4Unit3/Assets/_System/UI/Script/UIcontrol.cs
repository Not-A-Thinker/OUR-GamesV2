using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIcontrol : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject[] Player1Hp;
    [SerializeField] GameObject[] Player2Hp;
    //[SerializeField] Slider Player1Energy;
    //[SerializeField] Slider Player2Energy;
    [SerializeField] Image[] Player1EnergyImg;
    [SerializeField] Image[] Player2EnergyImg;

    [Header("Boss")]
    [SerializeField] Slider BossHP;
    [SerializeField] BossAI_Wind BossAI;

    [Header("Respawn")]
    [SerializeField] GameObject RespawnLoad;
    [SerializeField] TextMeshProUGUI LoadingText;
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
    [SerializeField] TextMeshProUGUI DeadCounter;
    [SerializeField] GameObject DeadCounterGobj;
    GameObject P1,P2;
    bool _isPause = false;
    [SerializeField] GameObject Pause_UI;
    //[SerializeField] Image SuckObjectCount;

    private void Start()
    {
        P1 = GameObject.Find("Player1").gameObject;
        P2 = GameObject.Find("Player2").gameObject;
    }
    
    private void Update()
    {
        if (pushing) 
        {
            Vector3 wantedPos = Camera.main.WorldToScreenPoint(P1.transform.position);

            if(pushingCD.activeInHierarchy)
                wantedPos.y = wantedPos.y - 30 ;
            else
                wantedPos.y = wantedPos.y - 20;
            pushing.transform.position = wantedPos;
        }

        if(pushingCD.activeInHierarchy)
        {
            Vector3 P1pos = Camera.main.WorldToScreenPoint(P1.transform.position);
            P1pos.y = P1pos.y - 20;
            pushingCD.transform.position = P1pos;
        }
        if (SuckingCD.activeInHierarchy)
        {
            Vector3 P2pos = Camera.main.WorldToScreenPoint(P2.transform.position);
            P2pos.y = P2pos.y - 20;
            SuckingCD.transform.position = P2pos;
        }    
        
        if (Input.GetButtonDown("Restart"))
        {
            _isPause = !_isPause;
            Pause();
        }
    }
    private void FixedUpdate()
    {
        if (BossHP.value <= 0 && BossAI.IsStage2)
        {
            YouWin();
        }
    }
    #region GameSystemUI
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

    public void TotalDeadCounterControl(bool State)
    {
        DeadCounterGobj.SetActive(State);
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
    #endregion

    #region PlayerUI
    private void Pause()
    {
        if (_isPause)
        {
            Time.timeScale = 0;       
        }
        else
        {
            Time.timeScale = 1;
        }

        Pause_UI.SetActive(_isPause);
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
    //Warnning Text(暫時性)
    public void flyText(int PlayerNum, Color color, string content)
    {
        Vector3 Player_pos;
        if (PlayerNum==1)       
            Player_pos = Camera.main.WorldToScreenPoint(P1.transform.position);     
        else
            Player_pos = Camera.main.WorldToScreenPoint(P2.transform.position);
        GameObject go = Instantiate(Resources.Load<GameObject>("FlyText/Text_FlyText"), Player_pos, Quaternion.identity) as GameObject;
        go.transform.SetParent(GameObject.Find("GUI").transform);
        UI_FlyText ft = go.GetComponent<UI_FlyText>();
        ft.color = color;
        ft.content = content;
    }
    //PlayerSuckPushCD
    public void PushingCDBar(float load)
    {
        //pushingCD.SetActive(Ready);
        pushCD_slider.value = Mathf.Lerp(load, pushCD_slider.value, smoothing * Time.deltaTime);
        if (load == 1)
            pushingCD.SetActive(false);
        else
            pushingCD.SetActive(true);
    }
    public void SuckingCDBar(float load)
    {
        //SuckingCD.SetActive(Ready);
        SuckCD_slider.value = Mathf.Lerp(load, SuckCD_slider.value, smoothing * Time.deltaTime);
        if (load >= 3)
            SuckingCD.SetActive(false);
        else
            SuckingCD.SetActive(true);
    }
    public void EnergyBarChange(int playerCount, int _DashNow, bool _DashWasUsed)//float NowEnergy,
    {
        ///舊閃避條UI用
        //if (Player1Energy.isActiveAndEnabled)
        //{
        //    if (playerCount == 1)
        //        Player1Energy.value = NowEnergy * 0.01f;
        //    else if (playerCount == 2)
        //        Player2Energy.value = NowEnergy * 0.01f;
        //}
        //if (Player1EnergyImg[0].isActiveAndEnabled)
        //{
        if(_DashWasUsed)
        {
             if (playerCount == 1)
                 Player1EnergyImg[_DashNow - 1].sprite = Resources.Load("UIImage/UI_Player_Dog_Dodge_BG", typeof(Sprite)) as Sprite;
             else if (playerCount == 2)
                 Player2EnergyImg[_DashNow - 1].sprite = Resources.Load("UIImage/UI_Player_Cat_Dodge_BG", typeof(Sprite)) as Sprite;
        }
        else
        {
             if (playerCount == 1)
                Player1EnergyImg[_DashNow - 1].sprite = Resources.Load("UIImage/UI_Player_Dog_Dodge", typeof(Sprite)) as Sprite; 
             else if (playerCount == 2)
                Player2EnergyImg[_DashNow - 1].sprite = Resources.Load("UIImage/UI_Player_Cat_Dodge", typeof(Sprite)) as Sprite; 
        }
        //}
    }
    ///為什麼用你會閃爍
    IEnumerator UiEndDelay(GameObject _DelayUi , int time)
    {
        yield return new WaitForSeconds(time);
        _DelayUi.SetActive(false) ;
    }

    #endregion

    //public void SuckCount(int Count)
    //{
    //    if (Count < 2)
    //        SuckObjectCount.color = new Color32(0, 130, 0, 255);
    //    else if (Count == 2)
    //        SuckObjectCount.color = new Color32(255, 230, 0, 255);
    //    else
    //        SuckObjectCount.color = new Color32(255, 0, 0, 255);
    //}
}
