using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class BossDamageIndicator : MonoBehaviour
{
    private CinemachineImpulseSource CIS;

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
        CIS = GetComponent<CinemachineImpulseSource>();

        material.SetColor("_MainColor", new Color(1,1,1));
        colorTemp = material.GetColor("_MainColor");
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.K))
        {
            ColorValueChange();
            CameraShake();
        }

        if (colorTemp != colorNormal)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / smoothing;

            float r = Mathf.Lerp(colorDamage.r, colorNormal.r, percentageComplete);
            float g = Mathf.Lerp(colorDamage.g, colorNormal.g, percentageComplete);
            float b = Mathf.Lerp(colorDamage.b, colorNormal.b, percentageComplete);
            colorTemp = new Color(r, g, b);

            material.SetColor("_MainColor", colorTemp);
        }
    }

    public void ColorValueChange()
    {
        material.SetColor("_MainColor", colorDamage);
        colorTemp = material.GetColor("_MainColor");

        elapsedTime = 0;
    }

    public void ColorChangeTrigger()
    {
        StartCoroutine(ColorChange());
    }

    public IEnumerator ColorChange()
    {
        material.SetColor("_MainColor", Color.red);
        colorTemp = material.GetColor("_MainColor");
        yield return new WaitForSeconds(0.5f);
        material.SetColor("_MainColor", new Color(1, 1, 1));
    }

    public void CameraShake()
    {
        CIS.GenerateImpulse();
    }
}
