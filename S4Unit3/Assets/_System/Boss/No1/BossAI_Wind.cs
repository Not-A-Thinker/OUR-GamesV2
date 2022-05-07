using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BasicState))]
[RequireComponent(typeof(BossSkillDemo))]
public class BossAI_Wind : MonoBehaviour
{
    public enum AIMode{ AIDisable, Normal, Move, Attack };
    public AIMode AI = AIMode.Normal;

    Rigidbody rb;
    Animator ani;
    NavMeshAgent agent;

    BossSkillDemo BossSkill;
    BasicState basicState;
    BossHealthBar healthBar;
    BossCameraControl cameraControl;

    GameObject _Player1;
    GameObject _Player2;

    Coroutine coroutineAtk;
    Coroutine coroutineThink;
    Coroutine coroutineTemp;
    Coroutine coroutineRun;
    Coroutine coroutineRunAtk;

    Vector3 selfPos;

    [Header("Alerter")]
    public Animator attackAlert;
    public Animator boomerageAlert;
    public Animator[] outerWindBladeAlert;
    public Animator wingAttackAlert;
    public Animator tailAttackAlert;
    public Animator headAttackAlert;
    public Animator AreaAttackAlert;

    [Header("Test Tweak")]
    public bool _TestingMode = false;
    public bool _DrawGizmos = true;

    [Header("Player LockOn")]
    [SerializeField] LayerMask lP1;
    [SerializeField] LayerMask lP2;
    [Space]
    [SerializeField] bool lookAtP1;
    [SerializeField] bool lookAtP2;
    [SerializeField] bool isLockOn;
    [SerializeField] bool _quickLock;
    [SerializeField] bool _isForceLockOn;// 這個只是控制playerLockOn的部分,不影響playerDetect
    [Space]
    [SerializeField] bool isMeleeAttacking;// 影響boss會不會轉向玩家,一般用在需要定向的攻擊上

    [Header("Boss Movement")]
    public float stopDistance = 23;
    public float timing = 0.2f;
    [SerializeField] float backwardForce = 100;
    [SerializeField] int preMoveCount = 0;
    bool _noPreMove;
    [SerializeField] bool isMoveFinished;
    bool _canAttack;
    bool afterDelay = false;

    [Header("AI")]
    [SerializeField] bool _aiEnable = true;
    [Space]
    [SerializeField] int aiStartTime = 99;
    [SerializeField] float aiReactTimeStage1 = 2.7f;
    [SerializeField] float aiReactTimeStage2 = 1.8f;
    [SerializeField] float aiReactTimeStandoMode = 3.6f;
    [Space]
    public bool IsStage1 = true;
    public bool IsStage2 = false;
    public bool IsAfter33 = false;
    [Space]
    [SerializeField] int aIStage2Turn = 1;

    [Header("Skills AI")]
    public bool isStandoMode = false;
    public bool isMain;
    public bool isStando;

    [Header("Skill Sets")]
    [SerializeField] bool b_UseComboSet = true;
    [Range(1,5)] [SerializeField] int _ComboNum = 1;
    [SerializeField] int _ComboMaxNum = 5;

    [Header("Skill Range")]
    [SerializeField] float skillRangeS = 25;
    [SerializeField] float skillRangeL = 50;
    [Space]
    [SerializeField] float skillRange1 = 20;
    [SerializeField] float skillRange2 = 35;
    [SerializeField] float skillRange3 = 50;

    public float angleOfView = 90f;
    bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = transform.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        BossSkill = GetComponent<BossSkillDemo>();
        basicState = GetComponent<BasicState>();
        healthBar = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();
        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();

        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        preMoveCount = 0;
        isMoveFinished = true;
        afterDelay = false;

        if(!isStando)
        {
            outerWindBladeAlert = new Animator[BossSkill.outerWindBladePoint.Length];
            for (int i = 0; i < BossSkill.outerWindBladePoint.Length; i++)
            {
                outerWindBladeAlert[i] = BossSkill.outerWindBladePoint[i].GetComponent<Animator>();
            }
        }
            
        if (!_aiEnable)
            AI = AIMode.AIDisable;
        else
            AI = AIMode.Normal;

        StartCoroutine(AIStartTimer());
    }

    ///This is only for testing function, should be del(or disable) soon.
    IEnumerator Test()
    {
        AreaAttackAlert.SetTrigger("AreaAttack Alert");
        yield return new WaitForSeconds(0.29f);
        BossSkill.BossWingAreaAttack();

        //Boomerang 風刃迴力鏢
        //boomerageAlert.SetTrigger("Boomer Alert");
        //yield return new WaitForSeconds(0.2f);
        //BossSkill.WindBladeBoomerang();

        //lookAtP1 = true;
        //yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

        //isMeleeAttacking = true;
        //BossSkill.BossWingAttack();
    }

    void Test2()
    {
        //wingAttackAlert.SetTrigger("WingAttack Alert");
        ////yield return new WaitForSeconds(0.35f);
        //BossSkill.BossWingAttack();

        //tailAttackAlert.SetTrigger("TailAttack Alert");
        //BossSkill.BossTailAttack();

        isMeleeAttacking = true;
        tailAttackAlert.SetTrigger("TailAttack Alert");
        BossSkill.BossTailAttack();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))//This is only for testing function, should be del soon.
        {
            //StartCoroutine(Test());
            //Test2();
        }

        if (isStando && Level1GameData.b_isCutScene) { Destroy(gameObject, .5f);}
        if (isStando){ Destroy(gameObject, 20);}
        if (isStando && healthBar.health <= 0 && basicState.isHealthMerge) { Destroy(gameObject, .5f); }

        if (isStando && basicState._currentHealth <= 0)
        {
            Debug.Log("Stando is Vanish!");
            GameObject.Find("Boss").GetComponent<BossAI_Wind>().isStandoMode = false;
            Destroy(gameObject);
        }

        ///Handle when Boss is DEAD.
        if (healthBar.health <= 0 && basicState.isHealthMerge && !isDead)
        {
            Level1GameData.b_isCutScene = true;

            isDead = true;
            isMoveFinished = true;
            isMeleeAttacking = true;
            ani.SetBool("IsDead", true);
            ani.SetTrigger("DeadTrigger");
        }
        else if (healthBar.health > 0 && basicState.isHealthMerge)
        {
            isDead = false;
        }

        #region StageDetect
        ///This is for detecting if is condition to stage 2
        ///May need to apply a animation to tell if is Stage 2
        if (IsStage1 && !basicState.isHealthMerge)
        {
            if (_TestingMode){return;}

            ///This is the version of 2 stage health.
            if (healthBar.health <= 0)
            {
                healthBar.Stage1ToStage2();
                IsStage1 = false;
                IsStage2 = true;

                _ComboNum = 1;
                _isForceLockOn = true;
                ChangePlayerTargetRandom();
                StartCoroutine(MoveDelayor());

                ///This is for rearrange the skill range in stage 2.
                skillRange1 = 20;
                skillRange2 = 35;
                skillRange3 = 50;

                ///This is for preventing the stando show up too early, can be change.
                StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime / 2));

                Debug.Log("Switch to Stage2!");
            }
        }
        else if (IsStage1 && basicState.isHealthMerge)
        {
            if (_TestingMode) { return; }

            ///This is the version of total health instead of 2 stage health.
            if (healthBar.health <= basicState._maxHealth / 2 && IsAfter33)
            {
                IsStage1 = false;
                IsStage2 = true;

                _ComboNum = 1;
                _isForceLockOn = true;
                ChangePlayerTargetRandom();
                StopCoroutine(coroutineThink);
                coroutineThink = StartCoroutine(TimeOfThink());
                StartCoroutine(MoveDelayor());

                ///This is for rearrange the skill range in stage 2.
                skillRange1 = 20;
                skillRange2 = 35;
                skillRange3 = 50;

                ///This is for preventing the stando show up too early, can be change.
                StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime / 2));

                Debug.Log("Switch to Stage2!");
            }
        }
        #endregion

        ///Press Left shift and 1 to change boss AI.
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftShift))
        {
            _aiEnable = !_aiEnable;
            if (_aiEnable)
            {StartCoroutine(AIRestartTimer()); AI = AIMode.Normal; }
            else if (!_aiEnable)
            {StopCoroutine(TimeOfThink()); AI = AIMode.AIDisable; }
        }
        if (!_aiEnable)
            return;

        ///This is for locking on player itself by shooting a raycast;
        selfPos = new Vector3(transform.position.x, 1, transform.position.z);
        #region Detect if player get hit by RaycastHit
        if (lookAtP1)
        {
            RaycastHit isPlayerGetHit;
            if (Physics.Raycast(selfPos, transform.forward, out isPlayerGetHit, skillRangeL, lP1))
            {
                if (isPlayerGetHit.transform.tag == "Player")
                { isLockOn = true; }
            }
            else { isLockOn = false; }
        }
        else if (lookAtP2)
        {
            RaycastHit isPlayerGetHit;
            if (Physics.Raycast(selfPos, transform.forward, out isPlayerGetHit, skillRangeL, lP2))
            {
                if (isPlayerGetHit.transform.tag == "Player")
                { isLockOn = true; }
            }
            else { isLockOn = false; }
        }
        //Debug.DrawRay(selfPos, transform.forward * skillRangeL, Color.red);
        #endregion

        PlayerLockOn();

        if (lookAtP1) { BossSetDestination(_Player1.transform.position); }
        else if (lookAtP2) { BossSetDestination(_Player2.transform.position); }

        if (IsStage2 && !isStando && afterDelay)
        {
            if (Level1GameData.b_isBoss1Defeated)
                return;
            BossStage2Movement();
        }

        //The Update ends here.
    }

    /// <summary>
    /// 根據玩家距離判定並鎖定最近或還在生的玩家
    /// </summary>
    public void PlayerDetect()
    {
        if (_isForceLockOn)
            return;

        if (_Player1 != null && _Player2 != null)
        {
            float distP1 = Vector3.Distance(transform.position, _Player1.transform.position);
            float distP2 = Vector3.Distance(transform.position, _Player2.transform.position);

            ///This is use for checking the closest player and if he's dead or not.
            if (distP1 < distP2 && !_Player1.GetComponent<PlayerState>().GetPlayerIsDead())
            {
                lookAtP1 = true;
                lookAtP2 = false;
            }
            else if (distP2 < distP1 && !_Player2.GetComponent<PlayerState>().GetPlayerIsDead())
            {
                lookAtP1 = false;
                lookAtP2 = true;
            }
            else if (_Player2.GetComponent<PlayerState>().GetPlayerIsDead())
            {
                lookAtP1 = true;
                lookAtP2 = false;
            }
            else
            {
                lookAtP1 = false;
                lookAtP2 = true;
            }
        }
        else { Debug.Log("Player is missing!"); }
    }
    public void PlayerLockOn()
    {
        ///This is use for rotating the boss to face to the player who is selected.
        if (lookAtP1 && !isMeleeAttacking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_Player1.transform.position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            if (_quickLock)
            { transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 60f * Time.deltaTime);}
            else
            { transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime);}
        }
        else if (lookAtP2 && !isMeleeAttacking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_Player2.transform.position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            if (_quickLock)
            { transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 60f * Time.deltaTime); }
            else
            { transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime); }
        }
    }

    /// <summary>
    /// 處理AI對玩家檢測和決定接下來的攻擊
    /// </summary>
    public void SkillSelection()
    {
        float distP1 = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(_Player1.transform.position.x, 0, _Player1.transform.position.z));
        float distP2 = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(_Player2.transform.position.x, 0, _Player1.transform.position.z));

        bool P1isBehind = Vector3.Angle(_Player1.transform.position - transform.position, transform.forward) > angleOfView;
        bool P2isBehind = Vector3.Angle(_Player2.transform.position - transform.position, transform.forward) > angleOfView;

        int AIDecision = 0;

        if (IsStage1 || isStando)
        {
            ///當不使用攻擊組時, AI會自行根據玩家的位置作出回應
            ///This is use for detect who is the closest player to boss or is behind it.
            ///But now lookatp1 and lookatp2 are work in the same way.
            if (lookAtP1 && !b_UseComboSet)//While the Boss is watching Player1...
            {
                if (distP1 > 0 && distP1 < skillRange1)//if the distance between player1 and boss are less than skillRange1, then...
                {
                    AIDecision = 11;
                }
                else if (distP1 > skillRange1 && distP1 < skillRange2)//if the distance between player1 and boss are less than skillRange2, then...
                {
                    AIDecision = 12;
                }
                else if (distP1 > skillRange2 && distP1 < skillRange3)//if the distance between player1 and boss are less than skillRange3, then...
                {
                    AIDecision = 13;
                }
                //if (distP1 > 0 && distP1 < skillRangeS)
                //{
                //    AIDecision = 14;
                //}
                //else if (distP1 > skillRangeS && distP1 < skillRangeL)
                //{
                //    AIDecision = 15;
                //}
            }
            if (lookAtP2 && !b_UseComboSet)//While the Boss is watching Player2...
            {
                if (distP2 > 0 && distP2 < skillRange1)//if the distance between player2 and boss are less than skillRange1, then...
                {
                    AIDecision = 11;//should change back to 21 if we decide to have different attack move to player.
                }
                else if(distP2 > skillRange1 && distP2 < skillRange2)//if the distance between player2 and boss are less than skillRange2, then...
                {
                    AIDecision = 12;//should change back to 22 if we decide to have different attack move to player.
                }
                else if(distP2 > skillRange1 && distP2 < skillRange3)//if the distance between player2 and boss are less than skillRange3, then...
                {
                    AIDecision = 13;//should change back to 23 if we decide to have different attack move to player.
                }
                //if (distP2 > 0 && distP2 < skillRangeS)
                //{
                //    AIDecision = 14;
                //}
                //else if (distP2 > skillRangeS && distP2 < skillRangeL)
                //{
                //    AIDecision = 15;
                //}
            }
            ///31 32為背後偵測攻擊,需要時再開啟
            ///If player1 is behind Boss and is the closest, then...
            //if (P1isBehind && distP1 > distP2)
            //{ AIDecision = 31;}
            ///If player2 is behind Boss and is the closest, then...
            //if (P2isBehind && distP1 < distP2)
            //{ AIDecision = 32;}

            ///當使用攻擊組時,AI只會按數字作出決定
            if (b_UseComboSet)
            {
                if (_ComboNum == 1 || _ComboNum == 3)
                {
                    AIDecision = 14;
                }
                else if (_ComboNum == 2 || _ComboNum == 5)
                {
                    AIDecision = 15;
                }
                else if (_ComboNum == 4)
                {
                    AIDecision = 16;
                }
                print("Combo Time!");
            }

            ///第一階大技/至二階過場動畫
            if (basicState.isHealthMerge && !isStando)
            {
                //if (healthBar.health <= healthBar.maxHealth - healthBar.maxHealth / 8 * BossSkill._STACount && BossSkill._STACount < 4)
                //{ AIDecision = 33; }
                //Debug.Log(healthBar.maxHealth - healthBar.maxHealth / 8 * BossSkill._STACount);

                if (healthBar.health <= healthBar.maxHealth - healthBar.maxHealth / 2)
                { AIDecision = 33; }
            }
            else if(!isStando)
            {
                //if (healthBar.health <= healthBar.maxHealth - healthBar.maxHealth / 4 * BossSkill._STACount && BossSkill._STACount < 4)
                //{ AIDecision = 33; }

                if (healthBar.health <= 0)
                { AIDecision = 33; }
            }
        }
        if (IsStage2 && !isStando)
        {
            ///This is use for detect who is the closest player to boss or is behind it.
            ///But now lookatp1 and lookatp2 are work in the same way.
            if (lookAtP1)//While the Boss is watching Player1...
            {
                if (distP1 > 0 && distP1 < skillRange1)//if the distance between player1 and boss are less than skillRange1, then...
                {AIDecision = 41;}
                else if (distP1 > skillRange1 && distP1 < skillRange2)//if the distance between player1 and boss are less than skillRange2, then...
                {AIDecision = 42;}
                else if (distP1 > skillRange2 && distP1 < skillRange3)//if the distance between player1 and boss are less than skillRange3, then...
                {AIDecision = 43;}
                if (distP1 > 0 && distP1 < skillRangeS)
                {AIDecision = 44;}
                else if (distP1 > skillRangeS && distP1 < skillRangeL)
                {AIDecision = 45;}
            }
            if (lookAtP2)//While the Boss is watching Player2...
            {
                if (distP2 > 0 && distP2 < skillRange1)//if the distance between player2 and boss are less than skillRange1, then...
                {
                    AIDecision = 41;//should change back to 51 if we decide to have different attack move to player.
                }
                else if (distP2 > skillRange1 && distP2 < skillRange2)//if the distance between player2 and boss are less than skillRange2, then...
                {
                    AIDecision = 42;//should change back to 52 if we decide to have different attack move to player.
                }
                else if (distP2 > skillRange1 && distP2 < skillRange3)//if the distance between player2 and boss are less than skillRange3, then...
                {
                    AIDecision = 43;//should change back to 53 if we decide to have different attack move to player.
                }
                if (distP2 > 0 && distP2 < skillRangeS)
                {AIDecision = 44;}
                else if (distP2 > skillRangeS && distP2 < skillRangeL)
                {AIDecision = 45;}
            }
            if (P1isBehind && distP1 > distP2)//If player1 is behind Boss and is the closest, then...
            { AIDecision = 61;}
            if (P2isBehind && distP1 < distP2)//If player2 is behind Boss and is the closest, then...
            { AIDecision = 62;}
            if (preMoveCount == 2 && distP1 < skillRange1)//If player1 is too near boss after 2 backward, then...
            { AIDecision = 63;}
            else if (preMoveCount == 2 && distP2 < skillRange1)//If player2 is too near boss after 2 backward, then...
            { AIDecision = 63;}

            if (!isStandoMode && BossSkill.canStandoAgain)//Stando Spawn
            { AIDecision = 65;}

            if (b_UseComboSet)
            {
                if (_ComboNum == 1 || _ComboNum == 2 || _ComboNum == 3)//Short Range Attack
                {
                    AIDecision = 44;
                }
                else if (_ComboNum == 4)//Long Range Attack
                {
                    AIDecision = 45;
                }
                else if (_ComboNum == 5)//定點攻擊 + Stando
                { AIDecision = 46; }
            }
        }

        //Debug.Log("Skill is select!");
        coroutineAtk = StartCoroutine(AIOnAttack(AIDecision));

        ///This is the End of Skill Selection.
    }

    ///<summary>
    ///處理來自SkillSelection的決定, 判斷並處理後續結果的程序
    ///</summary>
    public IEnumerator AIOnAttack(int num)
    {
        AI = AIMode.Attack;
        int rndNum = Random.Range(0, 100);
        print("AIDecision: " + num + " and rndNum: " + rndNum);

        ///This is the attack alert animation,
        ///and will have to wait at least 0.4 sec to response(may need to change).
        if (num != 33)
        {
            attackAlert.SetTrigger("isAttacking");
            yield return new WaitForSeconds(0.4f);
        }
        
        if (IsStage1 || isStando)
        {
            _ComboNum++;
            Debug.Log("Combo Num is: " + _ComboNum);
            switch (num)
            {
                #region Stage1_Case11-13
                case 11:
                    if (rndNum < 50)
                    {
                        ///WindBlade 16發風刃 * 3 + S形龍捲風
                        //yield return coroutineTemp = BossSkill.StartCoroutine(BossSkill.WindBlade16(3));
                        //BossSkill.TornadoAttack();

                        ///WindBlade 16發風刃 * 3
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        ///8Tornado 八方龍捲 * 3
                        BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                    }
                    break;
                case 12:
                    if (rndNum < 40)
                    {
                        ///Boomerang 風刃迴力鏢
                        boomerageAlert.SetTrigger("Boomer Alert");
                        yield return new WaitForSeconds(0.2f);

                        BossSkill.WindBladeBoomerang();
                        //cameraControl.ChangeTargetWeight(3, 3);
                    }
                    else if (rndNum >= 40 && rndNum < 67)
                    {
                        ///WindBlade 16發風刃 * 4 + S形龍捲風
                        //yield return coroutineTemp = BossSkill.StartCoroutine(BossSkill.WindBlade16(4));
                        BossSkill.TornadoAttack();

                    }
                    else if (rndNum >= 67 && rndNum < 100)
                    {
                        ///8Tornado八方龍捲 * 4
                        BossSkill.StartCoroutine(BossSkill.EightTornado(4));
                    }
                    break;
                case 13:
                    if (rndNum < 40)
                    {
                        if (BossSkill.tornadoGattaiIsExisted)
                        {
                            ///STornado S形龍捲風
                            BossSkill.TornadoAttack();
                        }
                        else
                        {
                            ///TornadoGattai 龍捲風合體
                            BossSkill.TornadoGattai();
                            //cameraControl.ChangeTargetWeight(3, 3);
                        }
                    }
                    else if (rndNum >= 40 && rndNum < 100)
                    {
                        ///STornado S形龍捲風
                        BossSkill.TornadoAttack();
                    }
                    break;
                #endregion

                ///Case14-16為Combo組合攻擊組
                case 14:
                    ///遠距離攻擊
                    //if (rndNum + 1 <= 25) 
                    //{
                    //    ///8Tornado八方龍捲 * 8
                    //    BossSkill.StartCoroutine(BossSkill.EightTornado(8));
                    //}
                    //else if (rndNum + 1 >= 26 && rndNum + 1 <= 50)
                    //{
                    //    ///Boomerang 風刃迴力鏢
                    //    boomerageAlert.SetTrigger("Boomer Alert");
                    //    yield return new WaitForSeconds(0.2f);
                    //    BossSkill.WindBladeBoomerang();
                    //}
                    //else if (rndNum + 1 >= 51 && rndNum + 1 <= 75)
                    //{
                    //    ///Outer WindBlade 風刃分割版
                    //    BossSkill.StartCoroutine(BossSkill.OuterWindBlade(4));
                    //}
                    //else if (rndNum + 1 >= 76 && rndNum + 1 <= 100)
                    //{
                    //    ///This should be the Wind Wall, but for now is Outer WindBlade
                    //    BossSkill.StartCoroutine(BossSkill.OuterWindBlade(4));
                    //}

                    if (rndNum < 33)
                    {
                        ///8Tornado八方龍捲 * 8
                        BossSkill.StartCoroutine(BossSkill.EightTornado(6));
                    }
                    else if (rndNum >=33 && rndNum < 67)
                    {
                        ///Boomerang 風刃迴力鏢
                        boomerageAlert.SetTrigger("Boomer Alert");
                        yield return new WaitForSeconds(0.2f);
                        BossSkill.WindBladeBoomerang();
                    }
                    else if (rndNum >=67 && rndNum < 100)
                    {
                        if (isStando)
                        {
                            ///8Tornado八方龍捲 * 8
                            BossSkill.StartCoroutine(BossSkill.EightTornado(6));
                        }
                        else
                        {
                            ///This should be the Wind Wall, but for now is Outer WindBlade
                            BossSkill.StartCoroutine(BossSkill.OuterWindBlade(4));
                        }
                        
                    }
                    break;
                case 15:
                    ///近距離攻擊
                    if (rndNum + 1 >= 1 && rndNum + 1 <= 50)
                    {
                        ///STornado S形龍捲風
                        BossSkill.TornadoAttack();
                    }
                    else if (rndNum + 1 >= 51 && rndNum + 1 <= 100)
                    {
                        if (isStando)
                        {
                            ///STornado S形龍捲風
                            BossSkill.TornadoAttack();
                        }
                        else
                        {
                            ///Wind Balls 風球
                            isMeleeAttacking = true;
                            BossSkill.BossWindBalls();
                        }
                    }
                    break;
                case 16:
                    ///場外攻擊-TornadoGattai 龍捲風合體
                    BossSkill.TornadoGattai();
                    break;

                #region Stage1_Case21-23
                //If there are player2 perf. then should change the skill.
                case 21:
                    if (rndNum < 50)
                    {
                        //WindBlade 16發風刃 * 1
                        //BossSkill.StartCoroutine(BossSkill.WindBlade16(1));
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //8Tornado 八方龍捲
                        BossSkill.StartCoroutine(BossSkill.EightTornado(1));
                    }
                    break;
                case 22:
                    if (rndNum < 33)
                    {
                        //Boomerang 風刃迴力鏢
                        boomerageAlert.SetTrigger("Boomer Alert");
                        yield return new WaitForSeconds(0.2f);

                        BossSkill.WindBladeBoomerang();
                    }
                    else if (rndNum >= 33 && rndNum < 67)
                    {
                        //WindBlade 16發風刃 * 2
                        //BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 67 && rndNum < 100)
                    {
                        //8Tornado八方龍捲 * 2
                        BossSkill.StartCoroutine(BossSkill.EightTornado(2));
                    }
                    break;
                case 23:
                    if (rndNum < 50)
                    {
                        //TornadoGattai 龍捲風合體
                        BossSkill.TornadoGattai();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //STornado S形龍捲風
                        BossSkill.TornadoAttack();
                    }
                    break;
                #endregion

                case 31:
                    if (rndNum < 50)
                    {
                        ///WindBlade 16發風刃 * 2
                        //BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        ///8Tornado 八方龍捲 * 3
                        BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                    }
                    break;
                case 32:
                    if (rndNum < 50)
                    {
                        ///WindBlade 16發風刃 * 2
                        //BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        ///8Tornado 八方龍捲 * 3
                        BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                    }
                    break;
                case 33:
                    ///STA 龍龍彈珠台(現在為過埸動畫)
                    isMoveFinished = true;
                    isMeleeAttacking = true;
                    BossSkill.TornadoSpecialAttack();

                    yield return new WaitForSeconds(7.5f);
                    //cameraControl.ChangeTargetWeight(3, 3);
                    break;
            }
            //yield return new WaitForSeconds(1f);
        }

        if (IsStage2 && !isStando)
        {
            _ComboNum++;
            Debug.Log("The Next Combo Num is: " + _ComboNum);
            switch (num)
            {
                case 41:
                    if (rndNum < 33)
                    {
                        ///Wing Area Attack 近戰範圍攻擊
                        isMeleeAttacking = true;
                        AreaAttackAlert.SetTrigger("AreaAttack Alert");
                        yield return new WaitForSeconds(0.26f);
                        BossSkill.BossWingAreaAttack();
                        isMoveFinished = true;
                    }
                    else if (rndNum >= 33 && rndNum < 67)
                    {
                        ///Tail Attack 尾巴攻擊
                        isMeleeAttacking = true;
                        tailAttackAlert.SetTrigger("TailAttack Alert");
                        BossSkill.BossTailAttack();
                        isMoveFinished = true;
                    }
                    else if (rndNum >= 67 && rndNum < 100)
                    {
                        ///Wind Balls 風球
                        isMeleeAttacking = true;
                        BossSkill.BossWindBalls();
                    }
                    break;
                case 42:
                    if (rndNum < 50)
                    {
                        ///Wing Attack 近戰攻擊(翼)
                        yield return coroutineRunAtk = StartCoroutine(BossAttackMovement(15));

                        isMeleeAttacking = true;
                        wingAttackAlert.SetTrigger("WingAttack Alert");
                        BossSkill.BossWingAttack();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        ///Mist Attack 霧氣攻擊
                        if (BossSkill.canMistAgain)
                        {
                            BossSkill.MistAttack();
                            StartCoroutine(BossSkill.MistCDTimer(BossSkill.mistCDTime));
                        }
                        else
                        {
                            ///Wing Attack 近戰攻擊(翼), 現先改成頭衝
                            yield return coroutineRunAtk = StartCoroutine(BossAttackMovement(15));

                            isMeleeAttacking = true;
                            //wingAttackAlert.SetTrigger("WingAttack Alert");
                            //BossSkill.BossWingAttack();
                            headAttackAlert.SetTrigger("HeadAttack Alert");
                            BossSkill.BossHeadAttack();
                            isMoveFinished = true;
                        }
                    }
                    break;
                case 43:
                    if(rndNum < 50)
                    {
                        ///Wind Hole 風柱
                        int _wHSpawnNum = Random.Range(8, 11);
                        StartCoroutine(BossSkill.WindHole(1, _wHSpawnNum));
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        ///Do something else
                        ///Wing Attack 近戰攻擊(翼)
                        //yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

                        //isMeleeAttacking = true;
                        //wingAttackAlert.SetTrigger("WingAttack Alert");
                        //BossSkill.BossWingAttack();

                        ///Wind Hole 風柱
                        int _wHSpawnNum = Random.Range(8, 11);
                        StartCoroutine(BossSkill.WindHole(1, _wHSpawnNum));
                    }
                    break;

                ///Case44-46為Combo組合攻擊組
                case 44:
                    ///近距離攻擊
                    if (rndNum < 25)
                    {
                        ///Wing Area Attack 近戰範圍攻擊
                        isMeleeAttacking = true;
                        AreaAttackAlert.SetTrigger("AreaAttack Alert");
                        yield return new WaitForSeconds(0.26f);
                        BossSkill.BossWingAreaAttack();
                        isMoveFinished = true;
                    }
                    else if (rndNum >= 25 && rndNum < 50)
                    {
                        ///Wing Attack 近戰攻擊(翼)
                        yield return coroutineRunAtk = StartCoroutine(BossAttackMovement(15));

                        isMeleeAttacking = true;
                        wingAttackAlert.SetTrigger("WingAttack Alert");
                        BossSkill.BossWingAttack();
                    }
                    else if (rndNum >= 50 && rndNum < 80)
                    {
                        if (aIStage2Turn % 2 == 0)
                        {
                            ///Tail Attack 尾巴攻擊
                            isMeleeAttacking = true;
                            tailAttackAlert.SetTrigger("TailAttack Alert");
                            BossSkill.BossTailAttack();
                            isMoveFinished = true;
                        }
                        else if (aIStage2Turn % 2 == 1)
                        {
                            ///Head Attack 頭衝攻擊
                            yield return coroutineRunAtk = StartCoroutine(BossAttackMovement(15));

                            isMeleeAttacking = true;
                            headAttackAlert.SetTrigger("HeadAttack Alert");
                            BossSkill.BossHeadAttack();
                            isMoveFinished = true;
                        }
                    }
                    else if (rndNum >= 80 && rndNum < 100)
                    {
                        ///Mist Attack 霧氣攻擊
                        if (BossSkill.canMistAgain)
                        {
                            BossSkill.MistAttack();
                            StartCoroutine(BossSkill.MistCDTimer(BossSkill.mistCDTime));
                        }
                        else
                        {
                            ///Wing Attack 近戰攻擊(翼)
                            yield return coroutineRunAtk = StartCoroutine(BossAttackMovement(15));

                            isMeleeAttacking = true;
                            wingAttackAlert.SetTrigger("WingAttack Alert");
                            BossSkill.BossWingAttack();
                        }
                    }
                    break;
                case 45:
                    ///遠距離攻擊
                    if (rndNum < 50)
                    {
                        ///Wind Hole 風柱
                        isMoveFinished = true;
                        isMeleeAttacking = true;
                        int _wHSpawnNum = Random.Range(8, 11);
                        StartCoroutine(BossSkill.WindHole(1, _wHSpawnNum));
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        ///Do something else
                        ///Wing Attack 近戰攻擊(翼)
                        //yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

                        //isMeleeAttacking = true;
                        //wingAttackAlert.SetTrigger("WingAttack Alert");
                        //BossSkill.BossWingAttack();

                        ///Wind Hole 風柱
                        isMoveFinished = true;
                        isMeleeAttacking = true;
                        int _wHSpawnNum = Random.Range(8, 11);
                        StartCoroutine(BossSkill.WindHole(1, _wHSpawnNum));
                    }
                    break;
                case 46:
                    ///定點攻擊 + 替身攻擊
                    if (!isStandoMode && BossSkill.canStandoAgain)
                    {
                        ///Stando! 分身
                        isStandoMode = true;
                        BossSkill.BossStando();
                        StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime));
                    }
                    else
                    {
                        ///Wind Balls 風球
                        isMeleeAttacking = true;
                        BossSkill.BossWindBalls();
                    }
                    break;

                case 61:
                    ///Wing Area Attack 近戰範圍攻擊
                    isMeleeAttacking = true;
                    AreaAttackAlert.SetTrigger("AreaAttack Alert");
                    yield return new WaitForSeconds(0.29f);
                    BossSkill.BossWingAreaAttack();
                    isMoveFinished = true;
                    break;
                case 62:
                    ///Wind Hole 風柱
                    StartCoroutine(BossSkill.WindHole(1, 8));
                    break;
                case 63:
                    ///Wing Area Attack 近戰範圍攻擊
                    isMeleeAttacking = true;
                    AreaAttackAlert.SetTrigger("AreaAttack Alert");
                    yield return new WaitForSeconds(0.29f);
                    BossSkill.BossWingAreaAttack();
                    isMoveFinished = true;
                    break;
                case 65:
                    ///Stando! 分身
                    isStandoMode = true;
                    BossSkill.BossStando();
                    StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime));
                    break;

            }
        }
        //yield return null;
        yield return new WaitForSeconds(.8f);
        ///This is the End of AI Attack.
    }

    public IEnumerator Pre_BossMovement()
    {
        if (preMoveCount <= 2)
        {
            if (lookAtP1 && Vector3.Distance(selfPos, _Player1.transform.position) <= (skillRange1 * 0.85f))
            {
                ani.SetTrigger("IsBackwarding");
                yield return new WaitForSeconds(timing);
                rb.AddForce(backwardForce * -transform.forward, ForceMode.Impulse);
                preMoveCount++;
            }
            if (lookAtP2 && Vector3.Distance(selfPos, _Player2.transform.position) <= (skillRange1 * 0.85f))
            {
                ani.SetTrigger("IsBackwarding");
                yield return new WaitForSeconds(timing);
                rb.AddForce(backwardForce * -transform.forward, ForceMode.Impulse);
                preMoveCount++;
            }
            yield return new WaitForSeconds(0.2f);
        }
        _noPreMove = true;
    }

    /// <summary>
    /// 用於協助Boss在二階時,跟玩家之間的距離
    /// </summary>
    void BossStage2Movement()
    {
        if (isDead)
            return;

        if (!_canAttack)
        {
            AI = AIMode.Move;

            if (lookAtP1)
            {
                agent.SetDestination(_Player1.transform.position);
            }
            else if (lookAtP2)
            {
                agent.SetDestination(_Player2.transform.position);
            }
        }
        else if (_canAttack && isMoveFinished)
        {
            agent.ResetPath();
        }
    }

    IEnumerator MoveDelayor()
    {
        yield return new WaitForSeconds(2f);
        afterDelay = true;
        Debug.Log("After Delay is finish!");
    }

    /// <summary>
    /// 用於處理Boss部分攻擊需要跟玩家更近距離時用
    /// </summary>
    /// <param name="attackRange"></param>
    /// <returns></returns>
    IEnumerator BossAttackMovement(float attackRange)
    {
        isMoveFinished = false;
        AI = AIMode.Move;

        StartCoroutine(BossRedestinationTimer());
        if (lookAtP1)
        {
            agent.SetDestination(_Player1.transform.position);
            //transform.LookAt(_Player1.transform);
            agent.stoppingDistance = attackRange - 1;

            yield return new WaitUntil(() => Vector3.Distance(selfPos, _Player1.transform.position) <= attackRange + 1);

            isMoveFinished = true;

            agent.SetDestination(transform.position);
            Debug.Log("Is Moved!");
        }
        else if (lookAtP2)
        {
            agent.SetDestination(_Player2.transform.position);
            //transform.LookAt(_Player2.transform);
            agent.stoppingDistance = attackRange - 1;

            yield return new WaitUntil(() => Vector3.Distance(selfPos, _Player2.transform.position) <= attackRange + 1);

            isMoveFinished = true;

            agent.SetDestination(transform.position);
            Debug.Log("Is Moved!");
        }
        yield return new WaitUntil(() => isMoveFinished);
        agent.ResetPath();
        //yield return new WaitForSeconds(1);
        //agent.SetDestination(orgPos);
    }

    IEnumerator BossRedestinationTimer()
    {
        yield return new WaitForSeconds(3f);
        if (!isMoveFinished)
        {
            isMoveFinished = true;
            agent.SetDestination(transform.position);
            StopCoroutine(BossAttackMovement(15));

            StopCoroutine(coroutineAtk);
            StopCoroutine(coroutineThink);

            coroutineThink = StartCoroutine(TimeOfThink());

            if (_ComboNum == 1)
            { _ComboNum = 1;}
            else
            { _ComboNum--;}

            Debug.Log("Shit Reseted.");
        }
    }

    /// <summary>
    /// 當isMoveFinished為false時,Boss會移動至目標的位置
    /// </summary>
    void BossSetDestination(Vector3 tarPos)
    {
        if (isStando || IsStage1)
        { return; }

        if (!isMoveFinished)
        { agent.SetDestination(tarPos); }
        else
        { agent.SetDestination(transform.position); }
    }

    /// <summary>
    /// 用於隨機選擇一個玩家作鎖定的功能
    /// </summary>
    void ChangePlayerTargetRandom()
    {
        int playerRnd = Random.Range(1, 3);
        if (playerRnd == 1)
        {
            lookAtP1 = true;
            lookAtP2 = false;
        }
        else if (playerRnd == 2)
        {
            lookAtP1 = false;
            lookAtP2 = true;
        }
        if (_Player1.GetComponent<PlayerState>().GetPlayerIsDead())
        {
            lookAtP1 = false;
            lookAtP2 = true;
        }
        else if(_Player2.GetComponent<PlayerState>().GetPlayerIsDead())
        {
            lookAtP1 = true;
            lookAtP2 = false;
        }
    }

    IEnumerator AIStartTimer()
    {
        yield return new WaitForSeconds(aiStartTime);

        if (_aiEnable)
        {
            if (_aiEnable) { Debug.Log("'AI' Started"); }
            yield return new WaitForSeconds(3);
            coroutineThink = StartCoroutine(TimeOfThink());
        }
    }

    IEnumerator AIRestartTimer()
    {
        //yield return new WaitForSeconds(aiStartTime);
        //while (true)
        //{
        //    if (!_aiEnable)
        //        yield return new WaitUntil(() => _aiEnable);
        //    if (_aiEnable && coroutineThink == null)
        //    {
        //        Debug.Log("'AI' Restarted");
        //        coroutineThink = StartCoroutine(TimeOfThink());
        //        break;
        //    }
        //    yield return new WaitForSeconds(5);
        //}
        if (_aiEnable)
        {
            coroutineThink = StartCoroutine(TimeOfThink());
            Debug.Log("'AI' Restarted");
        }
        yield return null;
    }

    /// <summary>
    /// 簡稱TOT, AI思考並做出決定的完整流程回圈
    /// </summary>
    public IEnumerator TimeOfThink()
    {
        AI = AIMode.Normal;
        while (IsStage1 && _aiEnable || isStando)
        {
            ///This is for detect where is the players and will provide the position for boss to target.
            PlayerDetect();
            yield return new WaitUntil(() => isLockOn);

            ///This is the skill selection, it is mainly for decide what skill to be use,
            ///After the select, the coroutine will be passed to AIOnAttack to perform the skill. 
            SkillSelection();
            yield return coroutineAtk;

            ///This is for restate the animator back to Idle State.
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            AI = AIMode.Normal;
            isMeleeAttacking = false;

            if (IsStage2)
                break;

            ///This is for the combo Set extra time rest after five attacks, for now.
            if (_ComboNum >= _ComboMaxNum + 1 && b_UseComboSet)
            {
                _ComboNum = 1;
                Debug.Log("Take a Break!");
                yield return new WaitForSeconds(aiReactTimeStage1 + 3);
            }
            else
            {
                if (isStando) { yield return new WaitForSeconds(aiReactTimeStage1 + 1);}
                else { yield return new WaitForSeconds(aiReactTimeStage1);}
            }
            AI = AIMode.Normal;
            Debug.Log("Stage1 Loop Ends Here.");
        }

        if (IsStage2)
        {
            Debug.Log("Switch to stage2 with sth animation cutscene");
        }

        ///They should play the same expect a AI movement decide will be added
        while (IsStage2 && _aiEnable)
        {
            ///This is for restate the animator back to Idle State. Test for will dc33 break the game.
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

            ///This is for detect where is the players and will provide the position for boss to target.
            PlayerDetect();
            yield return new WaitUntil(() => isLockOn);

            ///This is for detect if the boss need to do a move because of the player is too near by.
            _noPreMove = false;
            yield return coroutineRun = StartCoroutine(Pre_BossMovement());
            yield return new WaitUntil(() => _noPreMove);
            Debug.Log("PreMove End!");

            ///This is for the boss to stick on one player in order to be better perform the Skill Sets.
            if(lookAtP1) yield return new WaitUntil(() => Vector3.Distance(selfPos, _Player1.transform.position) <= skillRangeS + 1);
            if(lookAtP2) yield return new WaitUntil(() => Vector3.Distance(selfPos, _Player2.transform.position) <= skillRangeS + 1);

            _canAttack = true;

            ///This is the skill selection, it is mainly for decide what skill to be use
            ///After the select, the coroutine will be passed to AIOnAttack to perform the skill. 
            SkillSelection();
            yield return coroutineAtk;
            yield return new WaitUntil(() => isMoveFinished);
            agent.stoppingDistance = stopDistance;
            Debug.Log("Just Pass The 'isMoveFinished' Gate");

            ///This is for restate the animator back to Idle State.
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

            ///This is for reset the pre-move counter and melee attack so it can be perform again.
            preMoveCount = 0;
            isMeleeAttacking = false;
            _canAttack = false;
            aIStage2Turn++;
            AI = AIMode.Normal;

            //if (isStandoMode) { yield return new WaitForSeconds(aiReactTimeStandoMode); }
            //else { yield return new WaitForSeconds(aiReactTimeStage2); }

            ///This is for the combo Set extra time rest after five attacks, for now.
            if (_ComboNum >= _ComboMaxNum + 1 && b_UseComboSet)
            {
                _ComboNum = 1;
                Debug.Log("Take a Break!");
                if (isStandoMode) { yield return new WaitForSeconds(aiReactTimeStandoMode + 2); }
                else { yield return new WaitForSeconds(aiReactTimeStage2 + 2); }
            }
            else
            {
                if (isStandoMode) { yield return new WaitForSeconds(aiReactTimeStandoMode); }
                else { yield return new WaitForSeconds(aiReactTimeStage2); }
            }
            if (_isForceLockOn)
            {ChangePlayerTargetRandom();}

            Debug.Log("Stage2 Loop Ends Here!");
        }
    }

    private void OnDrawGizmos()
    {
        if (_DrawGizmos)
        {
            ///This is for showing the skill range on the editor or in play mode.
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, skillRange1);
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, skillRange2);
            //Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(transform.position, skillRange3);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, skillRangeS);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, skillRangeL);
        }
    }

    private void OnDestroy()
    {
        //Maybe spawn some particle when destroy.
        if (isStando)
        {
            Debug.Log("Stando is Vanish.");
            GameObject.Find("Boss").GetComponent<BossAI_Wind>().isStandoMode = false;
        }
    }
}
