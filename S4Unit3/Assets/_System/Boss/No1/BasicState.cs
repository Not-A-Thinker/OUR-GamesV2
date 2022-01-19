using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    public bool isHealthMerge = false;

    public int _maxHealth = 300;
    public int _currentHealth;

    public float _speed = 10f;
    public float _rotateSpeed;

    public bool b_isDefeated;

    void Start()
    {
        ///So if the isHealthMerge option is on, it will merge two health bar into one whole health bar.
        if (isHealthMerge)
        {
            _maxHealth *= 2;
            _currentHealth = _maxHealth;
        }
        else
        {
            _currentHealth = _maxHealth;
        }
    }
}
