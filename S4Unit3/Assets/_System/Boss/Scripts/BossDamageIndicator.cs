using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossDamageIndicator : MonoBehaviour
{
    
    public Material material;

    public UnityEvent OnDamageEvent;

    void Awake()
    {
        OnDamageEvent.AddListener(ColorChangeTrigger);
    }

    void Start()
    {
        material.SetColor("MainColor", new Color(1,1,1));
    }


    void Update()
    {
        
    }

    public void ColorChangeTrigger()
    {
        StartCoroutine(ColorChange());
    }

    public IEnumerator ColorChange()
    {
        material.SetColor("MainColor", Color.red);
        yield return new WaitForSeconds(0.5f);
        material.SetColor("MainColor", new Color(1, 1, 1));
    }
}
