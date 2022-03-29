using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerState : MonoBehaviour
{
    private CinemachineCollisionImpulseSource CCIS;
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
     //PlayerTotalDead playerTotalDead;
     PlayerState OthePlayerState;
    [SerializeField] Renderer _renderer;
    [SerializeField] PlayerAnimator _animation;
    [SerializeField] GameObject Hula;
    Color color;  
    //[SerializeField] GameObject SuckRange;
    //bool isColliding;

    bool isInvincible = false;
    
    private void Start()
    {
        color = _renderer.material.GetColor("_MainColor");
        //color = playerMat.shader.;     
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        //playerTotalDead = GameObject.Find("PlayerDeadCount").GetComponent<PlayerTotalDead>();
        _Collider = GetComponent<CapsuleCollider>();
        move = GetComponent<Move>();

        CCIS = GetComponent<CinemachineCollisionImpulseSource>();

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

        //Resurrect_range.SetActive(false);
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
        _renderer.material.SetColor("_MainColor", color);
        ///重置玩家成初始狀態
        isDead = false;
        Resurrect_range.SetActive(false);
        Hula.GetComponent<SpriteRenderer>().color = color;
        //Resurrect_range.GetComponent<PlayerRespawn>().RespawnRangeTrigger(false);
        _currentHealth = _maxHealth;   
        GetComponent<CapsuleCollider>().enabled = true;
        UIcontrol.RespawnText(false);
        StopCoroutine(Invincible(0));
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
        move.SpeedFast();
        move.isDashClose = false;
        move.inCC = false;
    }

    public void StartInvincible(float time)
    {
        StartCoroutine(Invincible(time));
    }

    //PlayerState Reset When Dead
    public void PlayerIsDead()
    {
        //死亡設置
        isDead = true;
        Resurrect_range.SetActive(true);
        Hula.GetComponent<SpriteRenderer>().color = new Color(128, 128, 128);
        _renderer.material.SetColor("_MainColor", new Color(128, 128, 128));
        //Resurrect_range.GetComponent<SpriteRenderer>().color = Color.red;
        //Resurrect_range.GetComponent<PlayerRespawn>().RespawnRangeTrigger(true);
        GetComponent<CapsuleCollider>().enabled = false;
        UIcontrol.RespawnText(true);
        //rb.useGravity = false;
        StartCoroutine(Invincible(999));
        if (isPlayer1)
        {
            ForceCast_TopDown forceCast_TopDown = this.GetComponent<ForceCast_TopDown>();
            forceCast_TopDown.ResetOldQue();
            forceCast_TopDown.enabled = false;        
        }
        else
        {
            ForceRepel_TopDown forceRepel_TopDown = GetComponentInChildren<ForceRepel_TopDown>();
            forceRepel_TopDown.resetObject();
            forceRepel_TopDown.enabled = false;
        }
        move.isKnockUp = false;
        move.SpeedSlow(0.25f);
        move.isDashClose = true;
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
     IEnumerator Invincible(float time)
    {
        _Collider.enabled = false;
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
        if (_renderer.enabled == true)       
            _renderer.enabled = false;      
        else
            _renderer.enabled = true;
    }
}