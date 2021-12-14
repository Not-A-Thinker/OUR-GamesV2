using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossCameraControl : MonoBehaviour
{
    CinemachineTargetGroup targetGroup;

    void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    ChangeTargetWeight(3, 2);
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    ChangeTargetWeight(3, 1);
        //}
    }

    public void ChangeTarget(int tarNum, Transform transform)
    {
        targetGroup.m_Targets[tarNum].target = transform;
    }

    public void ChangeTargetWeight(int tarNum, int weight)
    {
        targetGroup.m_Targets[tarNum].weight = weight;
    }

    public void ChangeTargetRadius(int tarNum, int radius)
    {
        targetGroup.m_Targets[tarNum].radius = radius;
    }
}
