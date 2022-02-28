using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    private CharacterController cc;
    private Animator animator;

    public bool isHealthMerge = false;

    public int _maxHealth = 300;
    public int _currentHealth;

    public float _speed = 10f;
    public float _rotateSpeed;

    public bool b_isDefeated;

    private void Awake()
    {
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

    void Start()
    {

    }

    void Update()
    {
        
    }
}
