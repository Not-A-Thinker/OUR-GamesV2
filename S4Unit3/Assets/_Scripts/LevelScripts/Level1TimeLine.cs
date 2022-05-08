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

    public CanvasGroup[] UICanvasGroup;

    void Awake()
    {
        DeathScene.SetActive(false);
        for (int i = 0; i < UICanvasGroup.Length; i++)
        {
            UICanvasGroup[i].alpha = 1;
        }
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
            for (int i = 0; i < UICanvasGroup.Length; i++)
            {
                UICanvasGroup[i].alpha = 0;
            }
            Boss.transform.position = BossOrgPoint;
            DeathScene.SetActive(true);
        }
        
    }
}
