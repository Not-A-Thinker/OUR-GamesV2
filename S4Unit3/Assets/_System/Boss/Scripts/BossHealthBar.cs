using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    Slider slider;
    public Slider backSlider;

    BasicState bossState;
    BossAI_Wind bossAI;

    [Header("Boss Health")]
    public float health;
    public float maxHealth;

    [Header("Health Bar Setting")]
    [Tooltip("Value smaller mean faster.")]
    [SerializeField] float smoothing = .5f;
    [Tooltip("Value smaller mean faster.")]
    [SerializeField] float backerSmoothing = .5f;
    [Tooltip("Value smaller mean the yellow thing will move early")]
    [SerializeField] float _backerTTC = .8f;

    float elapsedTime;
    float backerElapsedTime;
    float tempedHealth;
    [SerializeField] bool _isChanged = false;

    void Start()
    {
        bossState = GameObject.Find("Boss").GetComponent<BasicState>();
        bossAI = GameObject.Find("Boss").GetComponent<BossAI_Wind>();
        slider = GetComponent<Slider>();

        slider.maxValue = bossState._maxHealth;
        slider.value = bossState._maxHealth;

        backSlider.maxValue = bossState._maxHealth;
        backSlider.value = bossState._maxHealth;

        maxHealth = bossState._maxHealth;
        health = slider.value;
    }
    
    void Update()
    {
        //Hot keys for testing
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            TakeDamage(100);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) 
        {
            Healing(40);
        }


        //When the health is changed, slider value will change in a short time.
        if (slider.value != health)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / smoothing;

            slider.value = Mathf.Lerp(tempedHealth, health, percentageComplete);

            if (!_isChanged)
            {
                StartCoroutine(BackerValueChange());
            }
        }

        if (_isChanged)
        {
            backerElapsedTime += Time.deltaTime;
            float percentageComplete = backerElapsedTime / backerSmoothing;

            backSlider.value = Mathf.Lerp(tempedHealth, health, percentageComplete);

            if (backSlider.value == health)
            {
                _isChanged = false;
            }
        }
    }

    public void TakeDamage(float value)//Should Improve
    {
        //Because the decision have made, thus the code is closeing.
        //if (bossAI.isStandoMode)
        //{
        //    Debug.Log("No Damage!");
        //    return;
        //}

        if (health - value <= 0 )
        {
            tempedHealth = health;
            health = 0;
            slider.value = 0;

            elapsedTime = 0;
            backerElapsedTime = 0;
        }
        else
        {
            tempedHealth = health;
            health -= value;

            elapsedTime = 0;
            backerElapsedTime = 0;
        }
    } 

    public void Healing(float value)//Should Improve
    {
        if (health + value >= bossState._maxHealth)
        {
            health = bossState._maxHealth;
            slider.value = bossState._maxHealth;

            elapsedTime = 0;
            backerElapsedTime = 0;
        }
        else
        {
            tempedHealth = health;
            health += value;

            elapsedTime = 0;
            backerElapsedTime = 0;
        }
    }

    public void SetHealthBar(float value)
    {
        tempedHealth = health;
        health = value;

        elapsedTime = 0;
        backerElapsedTime = 0;
    }

    public void Stage1ToStage2()
    {
        tempedHealth = health;
        health = maxHealth;

        elapsedTime = 0;
        backerElapsedTime = 0;
    }

    IEnumerator BackerValueChange()
    {

        yield return new WaitForSeconds(_backerTTC);
        _isChanged = true;
    }
}
