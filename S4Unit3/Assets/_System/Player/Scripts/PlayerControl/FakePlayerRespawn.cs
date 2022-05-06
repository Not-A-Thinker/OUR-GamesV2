using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerRespawn : MonoBehaviour
{
    float RespawnCountRange = 0;
    int RespawnCount = 0;
    [SerializeField] LearningLevelScripts learningLevelScripts;
    [SerializeField] PlayerSoundEffect soundEffect;
    [SerializeField] GameObject HelpText, RespawnHeart;

    bool PlayerIsClose;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.parent.transform.position);
        pos.y = pos.y + 80;
        if (PlayerIsClose)
        {
            HelpText.SetActive(false);
            RespawnHeart.SetActive(true);
            RespawnHeart.transform.position = pos;
        }
        else
        {
            HelpText.SetActive(true);
            RespawnHeart.SetActive(false);
            pos.x = pos.x + 40;
            HelpText.transform.position = pos;
        }
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            PlayerIsClose = true;

            if (Input.GetButtonDown("HelpFriendP1") || Input.GetButtonDown("HelpFriendP2") )
            {
                RespawnCount++;
                RespawnCountRange = RespawnCount;
                PlayerSoundEffect.PlaySound("Player_Respawn");
                Debug.Log(RespawnCount);
            }
            if (Input.GetButton("HelpFriendP1") || Input.GetButton("HelpFriendP2") )
            {                              
                if (RespawnCount >= 15)
                {
                    RespawnCount = 0;
                    learningLevelScripts.isRespawnDone();
                    return;
                }
            }
        }
    }
    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
           PlayerIsClose = false;
        }
    }
}

