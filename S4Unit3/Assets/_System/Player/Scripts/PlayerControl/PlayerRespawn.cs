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

    private void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        PlayerState = transform.parent.gameObject.GetComponent<PlayerState>();
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
                    if (Input.GetButton("RespawnP2") || Respawning)
                    {
                        soundEffect.OnRespawnPlay();
                        RespawnCountRange = RespawnCountRange + (1 * Time.deltaTime);
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
                    else if (Input.GetButtonUp("RespawnP2") || !Respawning)
                    {
                        soundEffect.OnResetSound();
                        RespawnCountRange = 0;
                        //RespawnCount = 0;
                        UIcontrol.PlayerRespawnStop();
                        Debug.Log("RespawnCountReset");
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
                    if (Input.GetButton("RespawnP1") || Respawning)
                    {
                        soundEffect.OnRespawnPlay();
                        RespawnCountRange = RespawnCountRange + (1 * Time.deltaTime);
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
                    else if (Input.GetButtonUp("RespawnP1") || !Respawning)
                    {
                        soundEffect.OnResetSound();
                        RespawnCountRange = 0;
                        //RespawnCount = 0;
                        UIcontrol.PlayerRespawnStop();
                        Debug.Log("RespawnCountReset");
                    }
                }
            }              
        }
    }
}
