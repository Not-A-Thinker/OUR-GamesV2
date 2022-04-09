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
    [Space]
    [SerializeField] Transform[] _crystalOuterAlter;
    [Tooltip("This is use for controlling the height of the outer crystals can flow based on LevelHandler.")]
    [SerializeField] float _crystalOuterHeight = 6f;
    [Space]
    [SerializeField] Transform[] _crystalMediumAlter;
    [Tooltip("This is use for controlling the height of crystals can flow based on LevelHandler.")]
    [SerializeField] float _crystalMediumAlterHeight = 6f;
    [Space]
    [SerializeField] Transform[] _crystalMedium;
    [Tooltip("This is use for controlling the height of the outer crystals can flow based on LevelHandler.")]
    [SerializeField] float _crystalMediumHeight = 15f;
    [Space]
    [SerializeField] Transform[] _crystalLarge;
    [Tooltip("This is use for controlling the height of the outer crystals can flow based on LevelHandler.")]
    [SerializeField] float _crystalLargeHeight = 20f;


    private void Awake()
    {
        //Application.targetFrameRate = 120;

    }

    void Start()
    {
        //This is for handling the crystal movement animation.
        //哈利,這比Lerp好用
        foreach (var crystal in _crystalAlter)
        {
            crystal.DOMoveY(_crystalHeight, Random.Range(1.8f, 2f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.InOutCubic)
                   .SetUpdate(true);
            crystal.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360)
                   .SetLoops(-1, LoopType.Incremental)
                   .SetEase(Ease.Linear)
                   .SetUpdate(true);
        }

        foreach (var crystal in _crystalOuterAlter)
        {
            crystal.DOMoveY(_crystalOuterHeight, Random.Range(1.8f, 2f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.InOutSine)
                   .SetUpdate(true);
            crystal.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360)
                   .SetLoops(-1, LoopType.Incremental)
                   .SetEase(Ease.Linear)
                   .SetUpdate(true);
        }

        //foreach (var crystalMedium in _crystalMediumAlter)
        //{
        //    crystalMedium.DOMoveY(_crystalMediumAlterHeight, Random.Range(1.8f, 2f))
        //           .SetLoops(-1, LoopType.Yoyo)
        //           .SetEase(Ease.InOutSine)
        //           .SetUpdate(true)
        //           .SetDelay(2f);
        //}

        for (int i = 0; i < _crystalMediumAlter.Length; i++)
        {
            _crystalMediumAlter[i].DOMoveY(_crystalMediumAlterHeight, Random.Range(1.8f, 2f))
                                  .SetLoops(-1, LoopType.Yoyo)
                                  .SetEase(Ease.InOutSine)
                                  .SetUpdate(true)
                                  .SetDelay(2f);
        }

        foreach (var crystalMedium in _crystalMedium)
        {
            crystalMedium.DOMoveY(_crystalMediumHeight, Random.Range(3f, 4f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.OutCirc)
                   .SetUpdate(true);
            crystalMedium.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360)
                   .SetLoops(-1, LoopType.Incremental)
                   .SetEase(Ease.Linear)
                   .SetUpdate(true);
        }

        foreach (var crystalLarge in _crystalLarge)
        {
            crystalLarge.DOMoveY(_crystalLargeHeight, Random.Range(3f, 4f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.OutCirc)
                   .SetUpdate(true);
        }
    }

    
    void Update()
    {
        
    }
}
