using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossDamageIndicator : MonoBehaviour
{
    public Material material;
    [SerializeField] Color colorTemp;
    [SerializeField] Color colorNormal;
    [SerializeField] Color colorDamage;

    [Tooltip("Value smaller mean faster.")]
    [SerializeField] float smoothing = .5f;
    float elapsedTime;

    public UnityEvent OnDamageEvent;

    void Awake()
    {
        OnDamageEvent.AddListener(ColorChangeTrigger);
    }

    void Start()
    {
        material.SetColor("MainColor", new Color(1,1,1));
        Debug.Log(material.GetColor("MainColor"));
        colorTemp = material.GetColor("MainColor");
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    ColorValueChange();
        //}

        if (colorTemp != colorNormal)
        {
            Debug.Log("Is changing!");
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / smoothing;

            float r = Mathf.Lerp(colorDamage.r, colorNormal.r, percentageComplete);
            float g = Mathf.Lerp(colorDamage.g, colorNormal.g, percentageComplete);
            float b = Mathf.Lerp(colorDamage.b, colorNormal.b, percentageComplete);
            colorTemp = new Color(r, g, b);

            material.SetColor("MainColor", colorTemp);
        }
    }

    public void ColorValueChange()
    {
        material.SetColor("MainColor", colorDamage);
        colorTemp = material.GetColor("MainColor");

        elapsedTime = 0;
    }

    public void ColorChangeTrigger()
    {
        StartCoroutine(ColorChange());
    }

    public IEnumerator ColorChange()
    {
        material.SetColor("MainColor", Color.red);
        colorTemp = material.GetColor("MainColor");
        yield return new WaitForSeconds(0.5f);
        material.SetColor("MainColor", new Color(1, 1, 1));
    }


}
