using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    private Animator animator;

    [Header("Player Health")]
    int _maxHealth = 3;
    [SerializeField] int _currentHealth;


    [Header("Player State")]
    public bool isDead = false;
    public bool isMove = false;
    public bool isDash = false;
    public bool  isPlayer1;
    public bool  isPlayer2;

    [Header("Player GetComponent")]
    [SerializeField] UIcontrol UIcontrol;
    [SerializeField] GameObject Resurrect_range;
    [SerializeField] Move move;
    [SerializeField] PlayerTotalDead playerTotalDead;
    [SerializeField] PlayerState OthePlayerState;
    [SerializeField] Renderer _renderer;
    [SerializeField] PlayerAnimator _animation;
    //[SerializeField] GameObject SuckRange;
    //bool isColliding;

    bool isInvincible = false;
    private void Start()
    {           
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        playerTotalDead = GameObject.Find("PlayerDeadCount").GetComponent<PlayerTotalDead>();
        move = GetComponent<Move>();

        if(GetComponent<JoyStickMovement>())

       //檢查玩家編號
        if (isPlayer1)
        {
            _maxHealth = 5;
            OthePlayerState = GameObject.Find("Player2").GetComponent<PlayerState>();
        }
        if(isPlayer2)
        {
            OthePlayerState = GameObject.Find("Player1").GetComponent<PlayerState>();
        }

        _currentHealth = _maxHealth;

        Resurrect_range.SetActive(false);
    }

    void Update()
    {
        //isColliding = false;

        if(isPlayer2)
        {
            UIcontrol.EnergyBarChange(move.DashBar, 2);
        }
      

        //Invincible作弊
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isInvincible = !isInvincible;
            Debug.Log("Player Invincible is" + isInvincible);
        }
           
        //if (isMove&& playerCount==1)
        //    _animation.PlayerWalk(true);
        //else if (!isMove && playerCount == 1)
        //    _animation.PlayerWalk(false);

    }

    public bool GetPlayerIsDead()
    {
        return isDead;
    }

    public void hp_decrease()
    {
        if (!isInvincible)
        {
            _currentHealth--;

            if (isPlayer1)
            {
                P1GetCube p1GetCube= GetComponent<P1GetCube>();
                p1GetCube.PlayerGoneCube();
            }
            //Debug.Log(_currentHealth);     
            if (_currentHealth > 0)
            {
                StartCoroutine(Invincible(3));
            }
            if (_currentHealth == 0)
            {
                PlayerIsDead();
            }
            if (_currentHealth < 0)
                _currentHealth = 0;
            int playerCount=1;
            if (isPlayer1)
                playerCount = 1;
            if (isPlayer2)
                playerCount = 2;

            UIcontrol.hp_decrease(_currentHealth, playerCount);
            StartCoroutine(_animation.PlayerDamaged()) ;
        }
    }

    public void hp_increase()
    {
        _currentHealth++;
        UIcontrol.hp_increase(_currentHealth, );
    }

    //Player RespawnReset
    public void PlayerRespawn()
    {
        isDead = false;
        Resurrect_range.SetActive(false);
        _currentHealth = _maxHealth;
        move.inCC = false;
        GetComponent<CapsuleCollider>().enabled = true;
        StartCoroutine(Invincible(1));

        if (isPlayer1)
        {
            ForceCast_TopDown forceCast_TopDown = GetComponent<ForceCast_TopDown>();
            forceCast_TopDown.enabled = true;
        }
        else
        {
            ForceRepel_TopDown forceRepel_TopDown = GetComponentInChildren<ForceRepel_TopDown>();
            forceRepel_TopDown.enabled = true;
        }
        //rb.useGravity = true;
    }

    //PlayerState Reset When Dead
    public void PlayerIsDead()
    {
        isDead = true;
        Resurrect_range.SetActive(true);
        move.inCC = true;
        GetComponent<CapsuleCollider>().enabled = false;
        //rb.useGravity = false;

        if (isPlayer1)
        {
            ForceCast_TopDown forceCast_TopDown = this.GetComponent<ForceCast_TopDown>();
            forceCast_TopDown.enabled = false;
        }
        else
        {
            ForceRepel_TopDown forceRepel_TopDown = GetComponentInChildren<ForceRepel_TopDown>();
            forceRepel_TopDown.enabled = false;
        }
        playerTotalDead.TotalLifeDecrease();

        if (OthePlayerState._currentHealth == 0)
        {
            UIcontrol.GameOver();
        }
    }

    //GetHitByAttack
    private void OnTriggerEnter(Collider other)
    {
        //This is for Player to detect if the boss melee attack is hit or not.
        if (other.gameObject.tag == "WingAttack")
        {
            hp_decrease();
            Debug.Log("Wing Hit!");
        }
        if (other.gameObject.tag == "TailAttack")
        {
            hp_decrease();
            Debug.Log("Tail Hit!");
        }

    }

    //Hyper Muteki Gamer
    IEnumerator Invincible(int time)
    {
        isInvincible = true;
        Debug.Log("Is Fucking Invincible" + isInvincible);
        _renderer.enabled = false;
        InvokeRepeating("InvincibleRend", 0.2f, 0.2f);
       
        yield return new WaitForSeconds(time);
        CancelInvoke();
        _renderer.enabled = true;   
        isInvincible = false;
        Debug.Log("Invincible" + isInvincible);
    }

    //Cheat
    void InvincibleRend()
    {
        if (_renderer.enabled == true)       
            _renderer.enabled = false;      
        else
            _renderer.enabled = true;
    }
}