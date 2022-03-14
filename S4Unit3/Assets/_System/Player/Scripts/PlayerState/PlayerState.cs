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
    CapsuleCollider _Collider;
     UIcontrol UIcontrol ;
    [SerializeField] GameObject Resurrect_range;
     Move move;
     PlayerTotalDead playerTotalDead;
     PlayerState OthePlayerState;
    [SerializeField] Renderer _renderer;
    [SerializeField] PlayerAnimator _animation;
    //[SerializeField] GameObject SuckRange;
    //bool isColliding;

    bool isInvincible = false;
    private void Start()
    {           
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        //playerTotalDead = GameObject.Find("PlayerDeadCount").GetComponent<PlayerTotalDead>();
        _Collider = GetComponent<CapsuleCollider>();
        move = GetComponent<Move>();

        //if(GetComponent<JoyStickMovement>())
       //檢查玩家編號
        if (isPlayer1)
        {
            _maxHealth = 4;
            OthePlayerState = GameObject.Find("Player2").GetComponent<PlayerState>();
        }
        if(isPlayer2)
        {
            OthePlayerState = GameObject.Find("Player1").GetComponent<PlayerState>();
        }

        ///_currentHealth當前血量 _maxHealth最大血量
        _currentHealth = _maxHealth;

        Resurrect_range.SetActive(false);
    }

    void Update()
    {
        //isColliding = false;

        //if(isPlayer2)
        //{
        //    UIcontrol.EnergyBarChange(move._DashBar, 2);
        //}

        //Invincible作弊
        if (Input.GetKey(KeyCode.LeftControl)&& Input.GetKeyDown(KeyCode.CapsLock))
        {
            ///請在沒受傷的時候開這個無敵 不然會重置然後失去無敵
            isInvincible = !isInvincible;
            _Collider.enabled = !isInvincible;

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
        if(!isInvincible)
        {
            _currentHealth--;
            if (isPlayer1)
            {
                ///P1受傷要把方塊都丟掉
                P1GetCube p1GetCube = GetComponent<P1GetCube>();
                p1GetCube.PlayerGoneCube();
                ForceCast_TopDown _TopDown = GetComponent<ForceCast_TopDown>();
                _TopDown.ResetOldQue();
            }
            //Debug.Log(_currentHealth);     
            if (_currentHealth > 0)
            {
                ///受攻擊無敵
                StartInvincible(3);
            }
            if (_currentHealth == 0)
            {
                PlayerIsDead();
            }
            if (_currentHealth < 0)
                _currentHealth = 0;
            int playerCount = 1;
            if (isPlayer1)
                playerCount = 1;
            if (isPlayer2)
                playerCount = 2;
            ///UI
            UIcontrol.hp_decrease(_currentHealth, playerCount);
            StartCoroutine(_animation.PlayerDamaged());
        }          
    }

    public void hp_increase()
    {
        ///回血
        int playerCount = 1;
        if (isPlayer1)
            playerCount = 1;
        if (isPlayer2)
            playerCount = 2;
        _currentHealth++;
        UIcontrol.hp_increase(_currentHealth, playerCount);
    }

    //Player RespawnReset
    public void PlayerRespawn()
    {
        ///重置玩家成初始狀態
        isDead = false;
        Resurrect_range.SetActive(false);
        _currentHealth = _maxHealth;
        move.SpeedReset();
        GetComponent<CapsuleCollider>().enabled = true;
        StartInvincible(1);

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

    public void StartInvincible(int time)
    {
        StartCoroutine(Invincible(time));
    }

    //PlayerState Reset When Dead
    public void PlayerIsDead()
    {
        //死亡設置
        isDead = true;
        Resurrect_range.SetActive(true);
        move.isKnockUp = false;
        move.SpeedSlow(0.25f);
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
        //playerTotalDead.TotalLifeDecrease();

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
        //Debug.Log("Is Fucking Invincible" + isInvincible);
        _renderer.enabled = false;
        InvokeRepeating("InvincibleRend", 0.2f, 0.2f);
        yield return new WaitForSeconds(time);
        CancelInvoke();
        _renderer.enabled = true;
        _Collider.enabled = true;
        isInvincible = false;
        //Debug.Log("Invincible" + isInvincible);
    }

    //閃爍
    void InvincibleRend()
    {
        _Collider.enabled = false;
        if (_renderer.enabled == true)       
            _renderer.enabled = false;      
        else
            _renderer.enabled = true;
    }
}