using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    float RespawnCountRange = 0;
    int RespawnCount = 0;

    [SerializeField] PlayerState PlayerState;

    [SerializeField] UIcontrol UIcontrol;

    [SerializeField] Move Move;

    [SerializeField] PlayerSoundEffect soundEffect;

    public bool isPlayer1;
    public bool isPlayer2;

    public bool Respawning;

    private void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }

    public void RespawnRangeTrigger(bool IsOn)
    {
        GetComponent<SphereCollider>().isTrigger = IsOn;
    }

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
                        RespawnCount = (int)RespawnCountRange;
                        UIcontrol.PlayerRespawn(RespawnCountRange, RespawnCount, transform.parent.name);
                        Debug.Log(RespawnCount);
                        if (RespawnCount == 2)
                        {
                            PlayerState = transform.parent.gameObject.GetComponent<PlayerState>();
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCountRange = 0;
                            RespawnCount = 0;
                            return;
                        }
                    }
                    else if (Input.GetButtonUp("RespawnP2") || !Respawning)
                    {
                        soundEffect.OnResetSound();
                        RespawnCountRange = 0;
                        RespawnCount = 0;
                        UIcontrol.PlayerRespawn(RespawnCountRange, RespawnCount, transform.parent.name);
                        UIcontrol.PlayerRespawnStop();
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
                    if (Input.GetButton("RespawnP1") || Respawning)
                    {
                        soundEffect.OnRespawnPlay();
                        RespawnCountRange = RespawnCountRange + (1 * Time.deltaTime);
                        RespawnCount = (int)RespawnCountRange;
                        UIcontrol.PlayerRespawn(RespawnCountRange, RespawnCount, transform.parent.name);
                        //Debug.Log(RespawnCount);
                        if (RespawnCount == 2)
                        {
                            PlayerState = transform.parent.gameObject.GetComponentInParent<PlayerState>();
                            PlayerState.PlayerRespawn();
                            UIcontrol.PlayerRespawnStop();
                            UIcontrol.PlayerHpRefew(transform.parent.name);
                            RespawnCountRange = 0;
                            RespawnCount = 0;
                            //Debug.Log("RespawnCountReset");
                        }
                    }
                    else if (Input.GetButtonUp("RespawnP1") || Respawning)
                    {
                        soundEffect.OnResetSound();
                        RespawnCountRange = 0;
                        RespawnCount = 0;
                        UIcontrol.PlayerRespawnStop();
                        //Debug.Log("RespawnCountReset");
                    }
                }
            }              
        }
    }
}
