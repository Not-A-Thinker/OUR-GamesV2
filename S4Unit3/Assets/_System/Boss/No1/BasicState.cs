using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : MonoBehaviour
{
    private CharacterController cc;
    private Animator animator;

    public int _maxHealth = 300;
    [SerializeField] int _currentHealth;

    public float _speed = 10f;
    public float _rotateSpeed;

    public bool b_isDefeated;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    void Update()
    {
        
    }
}
