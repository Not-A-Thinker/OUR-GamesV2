using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DGTesting : MonoBehaviour
{
    [SerializeField] Transform[] _crystalAlter;
    void Start()
    {
        //Time.timeScale = 0;

        foreach (var crystal in _crystalAlter)
        {
            crystal.DOMoveY(2, Random.Range(1.8f, 2f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
            crystal.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
