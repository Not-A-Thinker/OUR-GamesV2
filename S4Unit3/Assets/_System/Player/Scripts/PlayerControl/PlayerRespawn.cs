using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    //float RespawnCountRange = 0;
    int RespawnCount = 0;

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
        //if (RespawnCountRange > 0)
        //    RespawnCountRange = RespawnCountRange - (0.2f * Time.deltaTime);
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
                    if (Input.GetButtonDown("RespawnP2"))
                    {
                        RespawnCount++;
                        Respawning = true;                     
                    }
                    if(Input.GetButtonDown("RespawnP2") || Respawning)
                    {
                        UIcontrol.PlayerRespawn(RespawnCount);
                        soundEffect.OnRespawnPlay();

                        if (RespawnCount >= 8)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCount = 0;
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
                    UIcontrol.PlayerIsClose = true;

                    if (Input.GetButtonDown("RespawnP1"))
                    {
                        RespawnCount++;
                        Respawning = true;                                         
                    }
                    if (Input.GetButton("RespawnP1"))
                    {
                        UIcontrol.PlayerRespawn(RespawnCount);
                        soundEffect.OnRespawnPlay();

                        if (RespawnCount >= 8)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCount = 0;
                            return;
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
