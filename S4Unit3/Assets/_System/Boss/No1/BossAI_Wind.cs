using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BasicState))]
[RequireComponent(typeof(BossSkillDemo))]
public class BossAI_Wind : MonoBehaviour
{
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

    [Header("Test Tweak")]
    public bool _TestingMode = false;
    public bool _DrawGizmos = true;

    [Header("Player LockOn")]
    [SerializeField] bool lookAtP1;
    [SerializeField] bool lookAtP2;
    [SerializeField] bool isLockOn;
    [SerializeField] bool isMeleeAttacking;

    [Header("Boss Movement")]
    public float timing = 0.2f;
    [SerializeField] float backwardForce = 100;
    [SerializeField] int preMoveCount = 0;
    Vector3 orgPos;
    [SerializeField] bool isMoveFinished;

    [Header("AI")]
    [SerializeField] bool _aiEnable = true;
    [SerializeField] int aiStartTime = 99;
    [SerializeField] float aiReactTimeStage1 = 2.7f;
    [SerializeField] float aiReactTimeStage2 = 1.8f;
    [SerializeField] float aiReactTimeStandoMode = 3.6f;
    public bool IsStage1 = true;
    public bool IsStage2 = false;

    [Header("Skills AI")]
    public bool isStandoMode = false;
    public bool isMain;
    public bool isStando;

    [Header("Skill Range")]
    [SerializeField] float skillRange1 = 20;
    [SerializeField] float skillRange2 = 35;
    [SerializeField] float skillRange3 = 50;

    public float angleOfView = 90f;

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

        StartCoroutine(AIStartTimer());
    }

    //This is only for testing function, should be del soon.
    IEnumerator Test()
    {
        //Boomerang 風刃迴力鏢
        boomerageAlert.SetTrigger("Boomer Alert");
        yield return new WaitForSeconds(0.2f);

        BossSkill.WindBladeBoomerang();

        //lookAtP1 = true;
        //yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

        //isMeleeAttacking = true;
        //BossSkill.BossWingAttack();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))//This is only for testing function, should be del soon.
        {
            //StartCoroutine(Test());
        }

        if (isStando){ Destroy(gameObject, 30); }

        if (isStando && basicState._currentHealth <= 0)
        {
            Debug.Log("Stando is Vanish!");
            GameObject.Find("Boss").GetComponent<BossAI_Wind>().isStandoMode = false;
            Destroy(gameObject);
        }

        //This is for detecting if is condition to stage 2
        //May need to apply a animation to tell if is Stage 2
        if (IsStage1 && !basicState.isHealthMerge)
        {
            if (_TestingMode){return;}

            //This is the version of 2 stage health.
            if (healthBar.health <= 0)
            {
                healthBar.Stage1ToStage2();
                IsStage1 = false;
                IsStage2 = true;

                //This is for rearrange the skill range in stage 2.
                skillRange1 = 20;
                skillRange2 = 35;
                skillRange3 = 50;

                //This is for preventing the stando show up too early, can be change.
                StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime / 2));

                Debug.Log("Switch to Stage2!");
            }
        }
        else if (IsStage1 && basicState.isHealthMerge)
        {
            if (_TestingMode) { return; }

            //This is the version of total health instead of 2 stage health.
            if (healthBar.health <= basicState._maxHealth / 2)
            {
                IsStage1 = false;
                IsStage2 = true;

                //This is for rearrange the skill range in stage 2.
                skillRange1 = 20;
                skillRange2 = 35;
                skillRange3 = 50;

                //This is for preventing the stando show up too early, can be change.
                StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime / 2));

                Debug.Log("Switch to Stage2!");
            }
        }

        //Press Left shift and 1 to change boss AI.
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftShift))
        {
            _aiEnable = !_aiEnable;
            if (_aiEnable)
            {StartCoroutine(AIRestartTimer());}
            else if (!_aiEnable)
            {StopCoroutine(TimeOfThink());}
        }
        if (!_aiEnable)
            return;

        //This is for locking on player itself by shooting a raycast;
        selfPos = new Vector3(transform.position.x, 1, transform.position.z);

        RaycastHit isPlayerGetHit;
        if (Physics.Raycast(selfPos, transform.forward, out isPlayerGetHit, skillRange3))
        {
            if (isPlayerGetHit.transform.tag == "Player")
            {isLockOn = true;}
        }
        else { isLockOn = false; }

        //Debug.DrawRay(selfPos, transform.forward * skillRange3, Color.red);

        PlayerLockOn();

        if (lookAtP1) { BossSetDestination(_Player1.transform.position); }
        else if (lookAtP2) { BossSetDestination(_Player2.transform.position); }

        //The Update ends here.
    }

    public void PlayerDetect()
    {
        if (_Player1 != null && _Player2 != null)
        {
            float distP1 = Vector3.Distance(transform.position, _Player1.transform.position);
            float distP2 = Vector3.Distance(transform.position, _Player2.transform.position);

            //This is use for checking the closest player and if he's dead or not.
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
        if (lookAtP1 && !isMeleeAttacking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_Player1.transform.position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime);
        }
        else if (lookAtP2 && !isMeleeAttacking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_Player2.transform.position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime);
        }
    }
    public void SkillSelection()
    {
        float distP1 = Vector3.Distance(transform.position, _Player1.transform.position);
        float distP2 = Vector3.Distance(transform.position, _Player2.transform.position);

        bool P1isBehind = Vector3.Angle(_Player1.transform.position - transform.position, transform.forward) > angleOfView;
        bool P2isBehind = Vector3.Angle(_Player2.transform.position - transform.position, transform.forward) > angleOfView;

        int AIDecision = 0;

        if (IsStage1 || isStando)
        {
            //This is use for detect who is the closest player to boss or is behind it.
            //But now lookatp1 and lookatp2 are work in the same way.
            if (lookAtP1)//While the Boss is watching Player1...
            {
                if (distP1 > 0 && distP1 < skillRange1)//if the distance between player1 and boss are less than skillRange1, then...
                {
                    AIDecision = 11;
                }
                if (distP1 > skillRange1 && distP1 < skillRange2)//if the distance between player1 and boss are less than skillRange2, then...
                {
                    AIDecision = 12;
                }
                if (distP1 > skillRange2 && distP1 < skillRange3)//if the distance between player1 and boss are less than skillRange3, then...
                {
                    AIDecision = 13;
                }
            }
            if (lookAtP2)//While the Boss is watching Player2...
            {
                if (distP2 > 0 && distP2 < skillRange1)//if the distance between player2 and boss are less than skillRange1, then...
                {
                    AIDecision = 11;//should change back to 21 if we decide to have different attack move to player.
                }
                if (distP2 > skillRange1 && distP2 < skillRange2)//if the distance between player2 and boss are less than skillRange2, then...
                {
                    AIDecision = 12;//should change back to 22 if we decide to have different attack move to player.
                }
                if (distP2 > skillRange1 && distP2 < skillRange3)//if the distance between player2 and boss are less than skillRange3, then...
                {
                    AIDecision = 13;//should change back to 23 if we decide to have different attack move to player.
                }
            }
            if (P1isBehind && distP1 > distP2)//If player1 is behind Boss and is the closest, then...
            { AIDecision = 31;}
            if (P2isBehind && distP1 < distP2)//If player2 is behind Boss and is the closest, then...
            { AIDecision = 32;}

            //第一階大技
            if (basicState.isHealthMerge && !isStando)
            {
                if (healthBar.health <= healthBar.maxHealth - healthBar.maxHealth / 8 * BossSkill._STACount && BossSkill._STACount < 4)
                { AIDecision = 33; }
                //Debug.Log(healthBar.maxHealth - healthBar.maxHealth / 8 * BossSkill._STACount);
            }
            else
            {
                if (healthBar.health <= healthBar.maxHealth - healthBar.maxHealth / 4 * BossSkill._STACount && BossSkill._STACount < 4 && !isStando)
                { AIDecision = 33; }
            }
        }
        if (IsStage2 && !isStando)
        {
            //This is use for detect who is the closest player to boss or is behind it.
            //But now lookatp1 and lookatp2 are work in the same way.
            if (lookAtP1)//While the Boss is watching Player1...
            {
                if (distP1 > 0 && distP1 < skillRange1)//if the distance between player1 and boss are less than skillRange1, then...
                {
                    AIDecision = 41;
                }
                if (distP1 > skillRange1 && distP1 < skillRange2)//if the distance between player1 and boss are less than skillRange2, then...
                {
                    AIDecision = 42;
                }
                if (distP1 > skillRange2 && distP1 < skillRange3)//if the distance between player1 and boss are less than skillRange3, then...
                {
                    AIDecision = 43;
                }
            }
            if (lookAtP2)//While the Boss is watching Player2...
            {
                if (distP2 > 0 && distP2 < skillRange1)//if the distance between player2 and boss are less than skillRange1, then...
                {
                    AIDecision = 41;//should change back to 51 if we decide to have different attack move to player.
                }
                if (distP2 > skillRange1 && distP2 < skillRange2)//if the distance between player2 and boss are less than skillRange2, then...
                {
                    AIDecision = 42;//should change back to 52 if we decide to have different attack move to player.
                }
                if (distP2 > skillRange1 && distP2 < skillRange3)//if the distance between player2 and boss are less than skillRange3, then...
                {
                    AIDecision = 43;//should change back to 53 if we decide to have different attack move to player.
                }
            }
            if (P1isBehind && distP1 > distP2)//If player1 is behind Boss and is the closest, then...
            { AIDecision = 61;}
            if (P2isBehind && distP1 < distP2)//If player2 is behind Boss and is the closest, then...
            { AIDecision = 62;}
            if (preMoveCount == 2 && distP1 < skillRange1)//If player1 is too near boss after 2 backward, then...
            { AIDecision = 63;}
            else if (preMoveCount == 2 && distP2 < skillRange1)//If player2 is too near boss after 2 backward, then...
            { AIDecision = 63;}

            if (!isStandoMode && BossSkill.canStandoAgain)
            {AIDecision = 64;}
        }

        //Debug.Log("Skill is select!");
        coroutineAtk = StartCoroutine(AIOnAttack(AIDecision));

        //This is the End of Skill Selection.
    }

    //This is for AI how to do an attack after the decision make by SkillSelection function.
    public IEnumerator AIOnAttack(int num)
    {
        int rndNum = Random.Range(0, 100);
        print("AI has select: " + num + "and rndNum is: " + rndNum);

        //This is the attack alert animation,
        //and will have to wait at least 0.4 sec to response(may need to change).
        attackAlert.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.4f);

        if (IsStage1 || isStando)
        {
            switch (num)
            {
                case 11:
                    if (rndNum < 50)
                    {
                        //WindBlade 16發風刃 * 3 + S形龍捲風
                        //yield return coroutineTemp = BossSkill.StartCoroutine(BossSkill.WindBlade16(3));
                        //BossSkill.TornadoAttack();

                        //WindBlade 16發風刃 * 3
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //8Tornado 八方龍捲 * 3
                        BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                    }
                    break;
                case 12:
                    if (rndNum < 40)
                    {
                        //Boomerang 風刃迴力鏢
                        boomerageAlert.SetTrigger("Boomer Alert");
                        yield return new WaitForSeconds(0.2f);

                        BossSkill.WindBladeBoomerang();
                        //cameraControl.ChangeTargetWeight(3, 3);
                    }
                    else if (rndNum >= 40 && rndNum < 67)
                    {
                        //WindBlade 16發風刃 * 4 + S形龍捲風
                        //yield return coroutineTemp = BossSkill.StartCoroutine(BossSkill.WindBlade16(4));
                        BossSkill.TornadoAttack();

                    }
                    else if (rndNum >= 67 && rndNum < 100)
                    {
                        //8Tornado八方龍捲 * 4
                        BossSkill.StartCoroutine(BossSkill.EightTornado(4));
                    }
                    break;
                case 13:
                    if (rndNum < 40)
                    {
                        if (BossSkill.tornadoGattaiIsExisted)
                        {
                            //STornado S形龍捲風
                            BossSkill.TornadoAttack();
                        }
                        else
                        {
                            //TornadoGattai 龍捲風合體
                            BossSkill.TornadoGattai();
                            //cameraControl.ChangeTargetWeight(3, 3);
                        }
                    }
                    else if (rndNum >= 40 && rndNum < 100)
                    {
                        //STornado S形龍捲風
                        BossSkill.TornadoAttack();
                    }
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
                        //WindBlade 16發風刃 * 2
                        //BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //8Tornado 八方龍捲 * 3
                        BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                    }
                    break;
                case 32:
                    if (rndNum < 50)
                    {
                        //WindBlade 16發風刃 * 2
                        //BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
                        BossSkill.WindBlade16AnimationTrigger();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //8Tornado 八方龍捲 * 3
                        BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                    }
                    break;
                case 33:
                    //STA 龍龍彈珠台
                    isMoveFinished = true;
                    BossSkill.TornadoSpecialAttack();

                    //yield return new WaitForSeconds(1f);
                    //cameraControl.ChangeTargetWeight(3, 3);
                    break;
            }
            //yield return new WaitForSeconds(1f);
        }

        if (IsStage2 && !isStando)
        {
            switch (num)
            {
                case 41:
                    if (rndNum < 50)
                    {
                        //Wing Area Attack 近戰範圍攻擊
                        BossSkill.BossWingAreaAttack();
                        isMoveFinished = true;
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //Tail Attack 尾巴攻擊
                        BossSkill.BossTailAttack();
                        isMoveFinished = true;
                    }
                    break;
                case 42:
                    if (rndNum < 50)
                    {
                        //Wing Attack 近戰攻擊(翼)
                        yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

                        isMeleeAttacking = true;
                        BossSkill.BossWingAttack();
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //Mist Attack 霧氣攻擊
                        if (BossSkill.canMistAgain)
                        {
                            BossSkill.MistAttack();
                            StartCoroutine(BossSkill.MistCDTimer(BossSkill.mistCDTime));
                        }
                        else
                        {
                            //Wing Attack 近戰攻擊(翼)
                            yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

                            isMeleeAttacking = true;
                            BossSkill.BossWingAttack();
                        }
                    }
                    
                    break;
                case 43:
                    if(rndNum < 50)
                    {
                        //Wind Hole 風柱
                        StartCoroutine(BossSkill.WindHole(1,8));
                    }
                    else if (rndNum >= 50 && rndNum < 100)
                    {
                        //Do something else
                        //Wing Attack 近戰攻擊(翼)
                        yield return coroutineRunAtk = StartCoroutine(BossAttackMovement());

                        isMeleeAttacking = true;
                        BossSkill.BossWingAttack();
                    }
                    break;
                case 61:
                    //Wing Area Attack 近戰範圍攻擊
                    BossSkill.BossWingAreaAttack();
                    isMoveFinished = true;
                    break;
                case 62:
                    //Wind Hole 風柱
                    StartCoroutine(BossSkill.WindHole(1, 8));
                    break;
                case 63:
                    //Wing Area Attack 近戰範圍攻擊
                    BossSkill.BossWingAreaAttack();
                    isMoveFinished = true;
                    break;
                case 64:
                    //Stando! 分身
                    isStandoMode = true;
                    BossSkill.BossStando();
                    StartCoroutine(BossSkill.StandoCDTimer(BossSkill.standoCDTime));
                    break;

            }
        }
        //yield return null;
        yield return new WaitForSeconds(.5f);
        //This is the End of AI Attack.
    }

    public IEnumerator Pre_BossMovement()
    {
        if (preMoveCount <= 2)
        {
            if (lookAtP1 && Vector3.Distance(transform.position, _Player1.transform.position) <= (skillRange1 / 1.5f))
            {
                ani.SetTrigger("IsBackwarding");
                yield return new WaitForSeconds(timing);
                rb.AddForce(backwardForce * -transform.forward, ForceMode.Impulse);
                preMoveCount++;
            }
            if (lookAtP2 && Vector3.Distance(transform.position, _Player2.transform.position) <= (skillRange1 / 1.5f))
            {
                ani.SetTrigger("IsBackwarding");
                yield return new WaitForSeconds(timing);
                rb.AddForce(backwardForce * -transform.forward, ForceMode.Impulse);
                preMoveCount++;
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator BossAttackMovement()
    {
        isMoveFinished = false;

        StartCoroutine(BossRedestinationTimer());
        if (lookAtP1)
        {
            agent.SetDestination(_Player1.transform.position);
            //transform.LookAt(_Player1.transform);

            yield return new WaitUntil(() => Vector3.Distance(transform.position, _Player1.transform.position) <= 10);

            isMoveFinished = true;

            agent.SetDestination(transform.position);
            Debug.Log("Is Moved!");
        }
        else if (lookAtP2)
        {
            agent.SetDestination(_Player2.transform.position);
            //transform.LookAt(_Player2.transform);

            yield return new WaitUntil(() => Vector3.Distance(transform.position, _Player2.transform.position) <= 10);

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

        yield return new WaitForSeconds(4f);
        if (!isMoveFinished)
        {
            isMoveFinished = true;
            agent.SetDestination(transform.position);
            StopCoroutine(BossAttackMovement());

            StopCoroutine(coroutineAtk);
            StopCoroutine(coroutineThink);

            coroutineThink = StartCoroutine(TimeOfThink());

            Debug.Log("Shit Reseted.");
        }

        yield return null;
    }

    void BossSetDestination(Vector3 tarPos)
    {
        if (isStando || IsStage1)
        { return; }

        if (!isMoveFinished)
        { agent.SetDestination(tarPos); }
        else
        { agent.SetDestination(transform.position); }
    }

    IEnumerator AIStartTimer()
    {
        yield return new WaitForSeconds(aiStartTime);
        if (_aiEnable){Debug.Log("'AI' Started");}
        yield return new WaitForSeconds(3);

        if (_aiEnable)
        {
            coroutineThink = StartCoroutine(TimeOfThink());
        }
    }

    IEnumerator AIRestartTimer()
    {
        //yield return new WaitForSeconds(aiStartTime);
        while (true)
        {
            if (!_aiEnable)
                yield return new WaitUntil(() => _aiEnable);
            if (_aiEnable && coroutineThink != null)
            {
                Debug.Log("'AI' Restarted");
                coroutineThink = StartCoroutine(TimeOfThink());
                break;
            }
            yield return new WaitForSeconds(5);
        }
    }

    //This is the whole routine for AI to do in one round loop.
    public IEnumerator TimeOfThink()
    {
        while (IsStage1 && _aiEnable)
        {
            //This is for detect where is the players and will provide the position for boss to target.
            PlayerDetect();
            yield return new WaitUntil(() => isLockOn);

            //This is the skill selection, it is mainly for decide what skill to be use,
            //After the select, the coroutine will be passed to AIOnAttack to perform the skill. 
            SkillSelection();
            yield return coroutineAtk;

            //This is for restate the animator back to Idle State.
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

            isMeleeAttacking = false;

            yield return new WaitForSeconds(aiReactTimeStage1);
            //Debug.Log("Will Start Again...");
        }

        if (IsStage2)
        {
            Debug.Log("Switch to stage2 with sth animation cutscene");
        }

        //They should play the same expect a AI movement decide will be added
        while (IsStage2 && _aiEnable)
        {
            //This is for detect where is the players and will provide the position for boss to target.
            PlayerDetect();
            yield return new WaitUntil(() => isLockOn);

            //This is for detect if the boss need to do a move because of the player is too near by.
            yield return coroutineRun = StartCoroutine(Pre_BossMovement());

            //This is the skill selection, it is mainly for decide what skill to be use
            //After the select, the coroutine will be passed to AIOnAttack to perform the skill. 
            SkillSelection();
            yield return coroutineAtk;

            //This is for restate the animator back to Idle State.
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

            //This is for reset the pre-move counter and melee attack so it can be perform again.
            preMoveCount = 0;
            isMeleeAttacking = false;
            if (isStandoMode) { yield return new WaitForSeconds(aiReactTimeStandoMode); }
            else { yield return new WaitForSeconds(aiReactTimeStage2); }
            
        }
    }

    private void OnDrawGizmos()
    {
        if (_DrawGizmos)
        {
            //This is for showing the skill range on the editor or in play mode.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, skillRange1);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, skillRange2);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, skillRange3);
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
