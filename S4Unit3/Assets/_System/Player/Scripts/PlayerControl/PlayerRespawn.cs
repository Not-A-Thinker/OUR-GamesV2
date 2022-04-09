using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    float RespawnCountRange = 0;
    //int RespawnCount = 0;

    PlayerState PlayerState;

    [SerializeField] UIcontrol UIcontrol;

    [SerializeField] Move Move;

    [SerializeField] PlayerSoundEffect soundEffect;

    public bool isPlayer1;
    public bool isPlayer2;

    public bool Respawning;

    private void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        PlayerState = transform.parent.gameObject.GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (RespawnCountRange > 0)
            RespawnCountRange = RespawnCountRange - (0.2f * Time.deltaTime);
    }

    //public void RespawnRangeTrigger(bool IsOn)
    //{
    //    GetComponent<SphereCollider>().isTrigger = IsOn;
    //}

    private void OnTriggerStay(Collider obj)
    {
        if(isPlayer1)
        {
            if (obj.tag == "Player" && obj.name!= "Player1")
            {
                if(PlayerState.isDead == true)
                {
                    if(Input.GetButtonDown("RespawnP2"))
                    {
                        RespawnCountRange = RespawnCountRange + 0.1f;
                    }

                    if (Input.GetButton("RespawnP2") || Respawning)
                    {
                        Respawning = true;
                        soundEffect.OnRespawnPlay();                     
                        //RespawnCount = (int)RespawnCountRange;
                        UIcontrol.PlayerRespawn(RespawnCountRange,transform.parent.name);
                        //Debug.Log(RespawnCount);
                        if (RespawnCountRange >= 2)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCountRange = 0;
                            //RespawnCount = 0;
                            return;
                        }
                    }
                    if (Input.GetButtonUp("RespawnP2") || !Respawning)
                    {
                        Respawning = false;
                        soundEffect.OnResetSound();                   
                        //RespawnCount = 0;
                        //UIcontrol.PlayerRespawnStop();
                        //Debug.Log("RespawnCountReset");
                    }
                }                         
            }
        }
        if (isPlayer2)
        {
            if (PlayerState.isDead == true)
            {
                if (obj.tag == "Player" && obj.name != "Player2")
                {
                    if (Input.GetButtonDown("RespawnP1"))
                    {
                        RespawnCountRange = RespawnCountRange + 0.1f;
                    }

                    if (Input.GetButton("RespawnP1") || Respawning)
                    {
                        Respawning = true;
                        soundEffect.OnRespawnPlay();                   
                        //RespawnCount = (int)RespawnCountRange;
                        UIcontrol.PlayerRespawn(RespawnCountRange, transform.parent.name);
                        //Debug.Log(RespawnCount);
                        if (RespawnCountRange >= 2)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCountRange = 0;
                            //RespawnCount = 0;
                            //Debug.Log("RespawnCountReset");
                        }
                    }
                    if (Input.GetButtonUp("RespawnP1") || !Respawning)
                    {
                        Respawning = false;
                        soundEffect.OnResetSound();                  
                        //RespawnCountRange = 0;
                        //RespawnCount = 0;
                        //UIcontrol.PlayerRespawnStop();
                        //Debug.Log("RespawnCountReset");
                    }
                }
            }              
        }
    }
}
