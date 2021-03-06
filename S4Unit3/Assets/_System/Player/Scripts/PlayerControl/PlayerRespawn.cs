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

    PlayerSoundEffect soundEffect;
    [SerializeField] PlayerAnimator animator;
    public bool isPlayer1;
    public bool isPlayer2;

    public bool Respawning;


    private void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        PlayerState = transform.parent.gameObject.GetComponent<PlayerState>();
        soundEffect = GameObject.Find("SFXAudioManager").GetComponent<PlayerSoundEffect>();
    }

    private void Update()
    {
        if (RespawnCount > 3)
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
                        PlayerSoundEffect.PlaySound("Player_Respawn");
                        //Debug.Log(RespawnCount);
                    }
                    if(Input.GetButton("HelpFriendP2") || Respawning)
                    {
                        UIcontrol.PlayerRespawn(RespawnCount);
                        animator.RespawnEffectPlay();
                        if (RespawnCount >= 9)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCount = 0;
                            animator.RespawnEffectStop();
                            return;
                        }
                    }

                    if (Input.GetButtonUp("HelpFriendP2") || !Respawning)
                    {
                        Respawning = false;
                        animator.RespawnEffectStop();
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
                        PlayerSoundEffect.PlaySound("Player_Respawn");
                        //Debug.Log(RespawnCount);
                    }
                    if (Input.GetButton("HelpFriendP1"))
                    {
                        UIcontrol.PlayerRespawn(RespawnCount);
                        animator.RespawnEffectPlay();
                        //soundEffect.OnRespawnPlay();                     
                        if (RespawnCount >= 9)
                        {
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCount = 0;
                            animator.RespawnEffectStop();
                            return;
                        }
                    }

                    if (Input.GetButtonUp("HelpFriendP1") || !Respawning)
                    {
                        Respawning = false;
                        animator.RespawnEffectStop();
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
