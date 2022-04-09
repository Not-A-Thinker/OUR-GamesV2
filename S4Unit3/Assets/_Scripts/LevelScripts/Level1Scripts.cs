using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//This Script should be use as the whole level control.
public class Level1Scripts : MonoBehaviour
{
    [SerializeField] Transform[] _crystalAlter;
    [SerializeField] float _crystalHeight = 6f;
    [Space]
    [SerializeField] Transform[] _crystalOuterAlter;
    [SerializeField] float _crystalOuterHeight = 6f;
    [Space]
    [SerializeField] Transform[] _crystalMediumAlter;
    [SerializeField] float _crystalMediumAlterHeight = 6f;
    [Space]
    [SerializeField] Transform[] _crystalMedium;
    [SerializeField] float _crystalMediumHeight = 15f;
    [Space]
    [SerializeField] Transform[] _crystalLarge;
    [SerializeField] float _crystalLargeHeight = 20f;
    [Space]
    [SerializeField] Transform[] _crystalOuterIsland;
    [SerializeField] float _crystalOuterIslandHeight = 8f;

    private void Awake()
    {
        //Application.targetFrameRate = 120;

    }

    void Start()
    {
        //This is for handling the crystal movement animation.
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
            crystalMedium.DOMoveY(_crystalMediumHeight, Random.Range(2f, 2.2f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.OutCirc)
                   .SetUpdate(true);
            //crystalMedium.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360)
            //       .SetLoops(-1, LoopType.Incremental)
            //       .SetEase(Ease.Linear)
            //       .SetUpdate(true);
        }

        foreach (var crystalLarge in _crystalLarge)
        {
            crystalLarge.DOMoveY(_crystalLargeHeight, Random.Range(3f, 4f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.OutCirc)
                   .SetUpdate(true);
        }

        foreach (var crystal in _crystalOuterIsland)
        {
            crystal.DOMoveY(_crystalOuterIslandHeight, Random.Range(1.8f, 2f))
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.InOutCubic)
                   .SetUpdate(true);
            crystal.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360)
                   .SetLoops(-1, LoopType.Incremental)
                   .SetEase(Ease.Linear)
                   .SetUpdate(true);
        }
    }

    
    void Update()
    {
        
    }
}
