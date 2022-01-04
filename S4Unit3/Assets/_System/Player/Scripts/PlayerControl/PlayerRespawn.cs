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

    public bool isPlayer1;
    public bool isPlayer2;

    private void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }

    private void OnTriggerStay(Collider obj)
    {
        if(isPlayer1)
        {
            if (obj.tag == "Player"&& obj.name!= "Player1")
            {
                if (Input.GetButton("RespawnP2"))
                {
                    RespawnCountRange = RespawnCountRange + (1 * Time.deltaTime);
                    RespawnCount = (int)RespawnCountRange;
                    UIcontrol.PlayerRespawn(RespawnCountRange, RespawnCount, transform.parent.name);
                    //Debug.Log(RespawnCount);
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
                else if (Input.GetButtonUp("RespawnP2"))
                {
                    RespawnCountRange = 0;
                    RespawnCount = 0;
                    UIcontrol.PlayerRespawnStop();
                    //Debug.Log("RespawnCountReset");
                }
            }
        }
        if (isPlayer2)
        {
            if (obj.tag == "Player" && obj.name != "Player2")
            {
                if (Input.GetButton("RespawnP1"))
                {
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
                else if (Input.GetButtonUp("RespawnP1"))
                {
                    RespawnCountRange = 0;
                    RespawnCount = 0;
                    UIcontrol.PlayerRespawnStop();
                    //Debug.Log("RespawnCountReset");
                }
            }
        }
    }
}
