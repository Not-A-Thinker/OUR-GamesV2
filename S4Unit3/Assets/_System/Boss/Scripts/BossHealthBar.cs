using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    BasicState bossState;
    Slider slider;

    [SerializeField] float smoothing = 5;
    public float health;
    public float maxHealth;

    void Start()
    {
        bossState = GameObject.Find("Boss").GetComponent<BasicState>();
        slider = GetComponent<Slider>();

        slider.maxValue = bossState._maxHealth;
        slider.value = bossState._maxHealth;
        maxHealth = bossState._maxHealth;
        health = slider.value;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TakeDamage(30);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
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
}
