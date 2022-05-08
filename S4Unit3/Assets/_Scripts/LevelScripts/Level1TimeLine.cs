using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Level1TimeLine : MonoBehaviour
{
    public GameObject DeathScene;
    bool isActive = false;

    public GameObject Boss;
    Vector3 BossOrgPoint;

    void Awake()
    {
        DeathScene.SetActive(false);
    }

    void Start()
    {
        BossOrgPoint = Boss.transform.position;
    }

    void Update()
    {
        if (Level1GameData.b_isBossDeathCutScene && !isActive)
        {
            isActive = true;

            Boss.transform.position = BossOrgPoint;
            DeathScene.SetActive(true);
        }
        
    }
}
