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


    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    void Start()
    {
        //This is for handling the crystal movement animation.
        //哈利,這比Lerp好用
        foreach (var crystal in _crystalAlter)
        {
            crystal.DOMoveY(_crystalHeight, Random.Range(1.8f, 2f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
            crystal.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

    }

    
    void Update()
    {
        
    }
}
