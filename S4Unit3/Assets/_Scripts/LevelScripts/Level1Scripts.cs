using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//This Script should be use as the whole level control.
public class Level1Scripts : MonoBehaviour
{
    [SerializeField] Transform[] _crystalAlter;
    [Tooltip("This is use for controlling the height of crystals can flow based on LevelHandler.")]
    [SerializeField] float _crystalHeight = 6f;
        

    void Start()
    {
        var sequence = DOTween.Sequence();

        foreach (var crystal in _crystalAlter)
        {
            crystal.DOMoveY(_crystalHeight, Random.Range(1.8f, 2f)).SetLoops(-1, LoopType.Yoyo);
        }
    }

    
    void Update()
    {
        
    }
}
