using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    //private CinemachineCollisionImpulseSource CCIS; //Disable due to the unstable solution.
    private CinemachineImpulseSource CIS;

    [Header("Debug & Cheats")]
    [SerializeField] bool CamShakeOff = false;
    [SerializeField] bool alwaysInvincible = false;

    [Header("Player Health")]
    [SerializeField] int _currentHealth;
    int _maxHealth = 3;
    
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
    [SerializeField] public Renderer _renderer;
    [SerializeField] PlayerAnimator _animation;
    [SerializeField] SpriteRenderer Hula;
    [SerializeField] SpriteRenderer Arrow;
    Color color,hula_color,ArrowColor;

    public Color DeadColor,DamageColor,DashColor;
    //[SerializeField] GameObject SuckRange;
    //bool isColliding;

    
    private void Start()
    {    
        color = _renderer.material.GetColor("_MainColor");
        hula_color = Hula.color;
        ArrowColor = Arrow.color;
        //color = playerMat.shader.;     
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        //playerTotalDead = GameObject.Find("PlayerDeadCount").GetComponent<PlayerTotalDead>();
        _Collider = GetComponent<CapsuleCollider>();
        move = GetComponent<Move>();

        //CCIS = GetComponent<CinemachineCollisionImpulseSource>();
        CIS = GetComponent<CinemachineImpulseSource>();

        alwaysInvincible = false;

        //if(GetComponent<JoyStickMovement>())
        //檢查玩家編號
        if (isPlayer1)
        {
            _maxHealth = 4;
            OthePlayerState = GameObject.Find("Player2").GetComponent<PlayerState>();
            Physics.IgnoreLayerCollision(12, 6, false);
            Physics.IgnoreLayerCollision(12, 7, false);
            Physics.IgnoreLayerCollision(12, 9, false);
        }
        if(isPlayer2)
        {
            OthePlayerState = GameObject.Find("Player1").GetComponent<PlayerState>();
            Physics.IgnoreLayerCollision(13, 6, false);
            Physics.IgnoreLayerCollision(13, 7, false);
            Physics.IgnoreLayerCollision(13, 9, false);
        }

        ///_currentHealth當前血量 _maxHealth最大血量
        _currentHealth = _maxHealth;

        //Resurrect_range.SetActive(false);
    }

    void Update()
    {
        if(Level1GameData.b_isCutScene)
        {
            if (isPlayer1)
            {
                ForceCast_TopDown forceCast_TopDown = GetComponent<ForceCast_TopDown>();
                forceCast_TopDown.enabled = false;
            }
            else
            {
                ForceRepel_TopDown forceRepel_TopDown = GetComponentInChildren<ForceRepel_TopDown>();
                forceRepel_TopDown.enabled = false;
            }
        }

        if (Level1GameData.b_isCutScene==false)
        {
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
        }
        //isColliding = false;

        //if(isPlayer2)
        //{
        //    UIcontrol.EnergyBarChange(move._DashBar, 2);
        //}
        var gamepad = Gamepad.current;
        //Invincible作弊
        if (Input.GetKey(KeyCode.LeftControl)&& Input.GetKeyDown(KeyCode.CapsLock))
        {
            ///請在沒受傷的時候開這個無敵 不然會重置然後失去無敵
            alwaysInvincible = !alwaysInvincible;

            Debug.Log("Player Invincible is" + alwaysInvincible);
        }

        if (alwaysInvincible)
        {_Collider.enabled = false;}
        else
        {_Collider.enabled = true;}
           
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
        if(!Level1GameData.b_isCutScene)
        {
            //StartCoroutine(Vibration(0.5f, 0.1f));
            _currentHealth--;
            PlayerSoundEffect.PlaySound("Player_GetDamage");


            if (!CamShakeOff) CIS.GenerateImpulse(); //This is use to create a impulase when get hit by a car.JK

            if (isPlayer1)
            {
                ///P1受傷要把方塊都丟掉
                P1GetCube p1GetCube = GetComponent<P1GetCube>();
                p1GetCube.PlayerGoneCube();
                ForceCast_TopDown _TopDown = GetComponent<ForceCast_TopDown>();
                _TopDown.ResetOldQue();
                _TopDown.StartCoroutine("ShootCD");
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
            _renderer.material.SetColor("_MainColor", DamageColor);
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
        Hula.color = hula_color;
        Arrow.color = ArrowColor;
        //Resurrect_range.GetComponent<PlayerRespawn>().RespawnRangeTrigger(false);
        _currentHealth = _maxHealth;   
        GetComponent<CapsuleCollider>().enabled = true;
        UIcontrol.RespawnText(false);
        UIcontrol.PlayerIsClose=false;
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
        Hula.GetComponent<SpriteRenderer>().color = DeadColor;
        Arrow.GetComponent<SpriteRenderer>().color = DeadColor;
        _renderer.material.SetColor("_MainColor", DeadColor);
        //Resurrect_range.GetComponent<SpriteRenderer>().color = Color.red;
        //Resurrect_range.GetComponent<PlayerRespawn>().RespawnRangeTrigger(true);
        GetComponent<CapsuleCollider>().enabled = false;
        UIcontrol.RespawnText(true);
        //rb.useGravity = false;
        StartCoroutine(Invincible(999999));
        if (isPlayer1)
        {
            ForceCast_TopDown forceCast_TopDown = this.GetComponent<ForceCast_TopDown>();
            forceCast_TopDown.ResetOldQue();
            forceCast_TopDown.enabled = false;
            PlayerSoundEffect.PlaySound("Dog_Dead");
        }
        else
        {
            ForceRepel_TopDown forceRepel_TopDown = GetComponentInChildren<ForceRepel_TopDown>();
            forceRepel_TopDown.resetObject();
            forceRepel_TopDown.enabled = false;
            PlayerSoundEffect.PlaySound("Cat_Dead");
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
        Debug.Log("Is Fucking Invincible");
        _Collider.enabled = false;
        _renderer.enabled = false;
        if (isPlayer1)
        {
            Physics.IgnoreLayerCollision(12, 6, true);
            Physics.IgnoreLayerCollision(12, 7, true);
            Physics.IgnoreLayerCollision(12, 9, true);
        }
        else if (isPlayer2)
        {
            Physics.IgnoreLayerCollision(13, 6, true);
            Physics.IgnoreLayerCollision(13, 7, true);
            Physics.IgnoreLayerCollision(13, 9, true);
        }       
        InvokeRepeating("InvincibleRend", 0.2f, 0.2f);
        yield return new WaitForSeconds(0.6f);
        _renderer.material.SetColor("_MainColor", color);
        yield return new WaitForSeconds(time);
        CancelInvoke();
        _renderer.enabled = true;
        _Collider.enabled = true;
        if (isPlayer1)
        {
            Physics.IgnoreLayerCollision(12, 6, false);
            Physics.IgnoreLayerCollision(12, 7, false);
            Physics.IgnoreLayerCollision(12, 9, false);
        }
        else if (isPlayer2)
        {
            Physics.IgnoreLayerCollision(13, 6, false);
            Physics.IgnoreLayerCollision(13, 7, false);
            Physics.IgnoreLayerCollision(13, 9, false);
        }
        //Debug.Log("Invincible" + isInvincible);
    }

    public void DashColorChange(bool isDsah)
    {
        if (isDsah) 
        {
            _renderer.material.SetColor("_MainColor", DashColor);
            if (isPlayer1)
            {
                Physics.IgnoreLayerCollision(12, 6, true);
                Physics.IgnoreLayerCollision(12, 7, true);
                Physics.IgnoreLayerCollision(12, 9, true);
            }
            else if (isPlayer2)
            {
                Physics.IgnoreLayerCollision(13, 6, true);
                Physics.IgnoreLayerCollision(13, 7, true);
                Physics.IgnoreLayerCollision(13, 9, true);
            }
        }
        else
        {
            _renderer.material.SetColor("_MainColor", color);
            if (isPlayer1)
            {
                Physics.IgnoreLayerCollision(12, 6, false);
                Physics.IgnoreLayerCollision(12, 7, false);
                Physics.IgnoreLayerCollision(12, 9, false);
            }
            else if (isPlayer2)
            {
                Physics.IgnoreLayerCollision(13, 6, false);
                Physics.IgnoreLayerCollision(13, 7, false);
                Physics.IgnoreLayerCollision(13, 9, false);
            }
        }
    }

    //閃爍
    void InvincibleRend()
    {
        if (_renderer.enabled == true)
        {
            _renderer.enabled = false;
        }           
        else
        {
            _renderer.enabled = true;
            _renderer.material.SetColor("_MainColor", color);
        }         
    }

    private static IEnumerator Vibration(float lowFrequency, // 低周波（左）モーターの強さ（0.0 ～ 1.0）
    float highFrequency )// 高周波（右）モーターの強さ（0.0 ～ 1.0）
    {
        var gamepad = Gamepad.current;
        if(gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            yield return new WaitForSeconds(0.3f); 
            gamepad.SetMotorSpeeds(0, 0);
        }      
    }
}