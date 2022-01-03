using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    BasicState bossState;
    Slider slider;
    BossAI_Wind bossAI;

    [SerializeField] float smoothing = 5;
    public float health;
    public float maxHealth;

    void Start()
    {
        bossState = GameObject.Find("Boss").GetComponent<BasicState>();
        bossAI = GameObject.Find("Boss").GetComponent<BossAI_Wind>();
        slider = GetComponent<Slider>();

        slider.maxValue = bossState._maxHealth;
        slider.value = bossState._maxHealth;
        maxHealth = bossState._maxHealth;
        health = slider.value;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            TakeDamage(100);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) 
        {
            Healing(40);
        }

        if (slider.value != health)
        {
            slider.value = Mathf.Lerp(slider.value, health, smoothing * Time.deltaTime);
        }
    }

    public void TakeDamage(float value)//Should Improve
    {
        if (bossAI.isStandoMode)
        {
            Debug.Log("No Damage!");
            return;
        }

        if (health - value <= 0 )
        {
            health = 0;
            slider.value = 0;
        }
        else
        {
            health -= value;
        }
    } 

    public void Healing(float value)//Should Improve
    {
        if (health + value >= bossState._maxHealth)
        {
            health = bossState._maxHealth;
            slider.value = bossState._maxHealth;
        }
        else
        {
            health += value;
        }
    }

    public void SetHealthBar(float value)
    {
        health = value;
    }

    public void Stage1ToStage2()
    {
        health = maxHealth;
    }
}
