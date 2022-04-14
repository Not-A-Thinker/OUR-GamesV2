using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    float RespawnCountRange = 0;
    int RespawnCount = 0;

    PlayerState PlayerState;

    [SerializeField] UIcontrol UIcontrol;

    [SerializeField] Move Move;

    [SerializeField] PlayerSoundEffect soundEffect;

    public bool isPlayer1;
    public bool isPlayer2;

    public bool Respawning;

    public ParticleSystem RespawnEvent;

    private void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        PlayerState = transform.parent.gameObject.GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (RespawnCount > 4)
        {
            if(Respawning)
            {
                RespawnCountRange = RespawnCountRange - (0.2f * Time.deltaTime);
                RespawnCount = (int)RespawnCountRange;               
            }         
        }           
    }

    //public void RespawnRangeTrigger(bool IsOn)
    //{
    //    GetComponent<SphereCollider>().isTrigger = IsOn;
    //}

    private void OnTriggerStay(Collider obj)
    {
        if(isPlayer1)
        {
            if (PlayerState.isDead == true)
            {                     
                if (obj.tag == "Player" && obj.name != "Player1")
                {
                    UIcontrol.PlayerIsClose = true;
                    if (Input.GetButtonDown("HelpFriendP2"))
                    {
                        RespawnCount++;
                        RespawnCountRange = RespawnCount;
                        Respawning = true;
                        Debug.Log(RespawnCount);
                    }
                    if(Input.GetButtonDown("HelpFriendP2") || Respawning)
                    {
                        UIcontrol.PlayerRespawn(RespawnCount);
                        //soundEffect.OnRespawnPlay();
                        if (RespawnEvent != null)
                            RespawnEvent.Play();

                        if (RespawnCount >= 9)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCount = 0;
                            if (RespawnEvent != null)
                                RespawnEvent.Stop();
                            return;
                        }
                    }

                    if (Input.GetButtonUp("HelpFriendP2") || !Respawning)
                    {
                        Respawning = false;
                        //soundEffect.OnResetSound();
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
                    UIcontrol.PlayerIsClose = true;

                    if (Input.GetButtonDown("HelpFriendP1"))
                    {
                        RespawnCount++;
                        RespawnCountRange = RespawnCount;
                        Respawning = true;                      
                    }
                    if (Input.GetButton("HelpFriendP1"))
                    {
                        UIcontrol.PlayerRespawn(RespawnCount);
                        //soundEffect.OnRespawnPlay();
                        if (RespawnEvent != null)
                            RespawnEvent.Play();
                        if (RespawnCount >= 9)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCount = 0;
                            if (RespawnEvent != null)
                                RespawnEvent.Stop();
                            return;
                        }
                    }

                    if (Input.GetButtonUp("HelpFriendP1") || !Respawning)
                    {
                        Respawning = false;
                        //soundEffect.OnResetSound();
                        //RespawnCountRange = 0;
                        //RespawnCount = 0;
                        //UIcontrol.PlayerRespawnStop();
                        //Debug.Log("RespawnCountReset");
                    }
                }
            }              
        }
    }
    private void OnTriggerExit(Collider obj)
    {
        if (isPlayer1)
        {
            if (PlayerState.isDead == true)
            {
                if (obj.tag == "Player" && obj.name != "Player1")
                {
                    UIcontrol.PlayerIsClose = false;
                }
            }
        }
        if (isPlayer2)
        {
            if (PlayerState.isDead == true)
            {
                if (obj.tag == "Player" && obj.name != "Player2")
                {
                    UIcontrol.PlayerIsClose = false;
                }
            }
        }
    }
}
