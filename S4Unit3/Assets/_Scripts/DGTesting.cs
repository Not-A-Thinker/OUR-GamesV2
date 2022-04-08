using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DGTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;


        transform.DOMoveY(3, Random.Range(1.8f, 2f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).SetUpdate(false);
        transform.DORotate(new Vector3(360, 360, 360), 3, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetUpdate(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
