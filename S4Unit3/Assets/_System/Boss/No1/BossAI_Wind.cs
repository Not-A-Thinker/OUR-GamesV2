using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossSkillDemo))]
public class BossAI_Wind : MonoBehaviour
{
    Rigidbody rb;
    Animator ani;

    BossSkillDemo BossSkill;
    BossHealthBar healthBar;
    BossCameraControl cameraControl;

    private GameObject _Player1;
    private GameObject _Player2;

    public Animator attackAlert;

    Coroutine coroutineAtk;
    Coroutine coroutineThink;
    Coroutine coroutineTemp;
    Coroutine coroutineRun;

    Vector3 selfPos;

    [Header("Player LockOn")]
    [SerializeField] bool lookAtP1;
    [SerializeField] bool lookAtP2;
    [SerializeField] bool isLockOn;

    [Header("Boss Movement")]
    public float timing = 0.2f;
    [SerializeField] float backwardForce = 100;

    [Header("AI")]
    [SerializeField] bool aiEnable = true;
    [SerializeField] int aiStartTime = 99;
    [SerializeField] float aiReactTime = 3;
    public bool IsStage1 = true;
    public bool IsStage2 = false;

    [Header("Skills AI")]
    public bool isMain;
    public bool isStand;

    [Header("Skill Range")]
    [SerializeField] float skillRange1 = 5;
    [SerializeField] float skillRange2 = 10;
    [SerializeField] float skillRange3 = 15;

    public float angleOfView = 90f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        BossSkill = GetComponent<BossSkillDemo>();
        healthBar = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();
        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();

        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        StartCoroutine(AIStartTimer());
    }

    void Update()
    {
        //Press Left shift and 1 to change boss AI.
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftShift))
        {
            aiEnable = !aiEnable;
            if (aiEnable)
            {
                StartCoroutine(AIRestartTimer());
            }
            else if (!aiEnable)
            {
                StopCoroutine(TimeOfThink());
            }
        }
        if (!aiEnable)
            return;

        //This is for detecting if is condition to stage 2
        //May need to apply a animation to tell if is Stage 2
        if (healthBar.health <= 0 && IsStage1)
        {
            healthBar.Stage1ToStage2();
            IsStage1 = false;
            IsStage2 = true;
            Debug.Log("Switch to Stage2!");
        }

        selfPos = new Vector3(transform.position.x, 1, transform.position.z);

        RaycastHit isPlayerGetHit;
        if (Physics.Raycast(selfPos, transform.forward, out isPlayerGetHit, skillRange3))
        {
            if (isPlayerGetHit.transform.tag == "Player")
            {isLockOn = true;}
        }
        else { isLockOn = false; }

        Debug.DrawRay(selfPos, transform.forward * skillRange3, Color.red);

        PlayerLockOn();

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
    }
    public void PlayerLockOn()
    {
        if (lookAtP1)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_Player1.transform.position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime);
        }
        else if (lookAtP2)
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

        if (IsStage1 || isStand)
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
            {
                AIDecision = 31;
            }
            if (P2isBehind && distP1 < distP2)//If player2 is behind Boss and is the closest, then...
            {
                AIDecision = 32;
            }

            //第一階大技
            if (healthBar.health <= healthBar.maxHealth - healthBar.maxHealth / 4 * BossSkill._STACount && BossSkill._STACount < 4)
            {
                AIDecision = 33;
                //Debug.Log("I'm gonna spam the STA!");
            }
        }

        if (IsStage2 && !isStand)
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
            {
                AIDecision = 61;
            }
            if (P2isBehind && distP1 < distP2)//If player2 is behind Boss and is the closest, then...
            {
                AIDecision = 62;
            }
        }

        Debug.Log("Skill is select!");
        coroutineAtk = StartCoroutine(AIOnAttack(AIDecision));
    }

    public IEnumerator AIOnAttack(int num)
    {
        int rndNum = Random.Range(0, 100);
        print("AI has select: " + num + "and rndNum is: " + rndNum);

        //This is the attack alert animation,
        //and will have to wait at least 0.4 sec to response(may need to change).
        attackAlert.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.4f);

        switch (num)
        {
            case 11:
                if (rndNum < 50)
                {
                    //WindBlade 16發風刃 * 3 + S形龍捲風
                    yield return coroutineTemp = BossSkill.StartCoroutine(BossSkill.WindBlade16(3));
                    BossSkill.TornadoAttack();
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
                    BossSkill.WindBladeBoomerang();
                    cameraControl.ChangeTargetWeight(3, 3);
                }
                else if (rndNum >= 40 && rndNum < 67)
                {
                    //WindBlade 16發風刃 * 4 + S形龍捲風
                    yield return coroutineTemp = BossSkill.StartCoroutine(BossSkill.WindBlade16(4));
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
                    //TornadoGattai 龍捲風合體
                    BossSkill.TornadoGattai();
                    cameraControl.ChangeTargetWeight(3, 3);
                }
                else if (rndNum >= 40 && rndNum < 100)
                {
                    //STornado S形龍捲風
                    BossSkill.TornadoAttack();
                }
                break;

            case 21:
                if (rndNum < 50)
                {
                    //WindBlade 16發風刃 * 1
                    BossSkill.StartCoroutine(BossSkill.WindBlade16(1));
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
                    BossSkill.WindBladeBoomerang();
                }
                else if (rndNum >= 33 && rndNum < 67)
                {
                    //WindBlade 16發風刃 * 2
                    BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
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

            case 31:
                if (rndNum < 50)
                {
                    //WindBlade 16發風刃 * 2
                    BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
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
                    BossSkill.StartCoroutine(BossSkill.WindBlade16(2));
                }
                else if (rndNum >= 50 && rndNum < 100)
                {
                    //8Tornado 八方龍捲 * 3
                    BossSkill.StartCoroutine(BossSkill.EightTornado(3));
                }
                break;
            case 33:
                //STA 龍龍彈珠台
                BossSkill.TornadoSpecialAttack();
                cameraControl.ChangeTargetWeight(3, 3);
                break;

            case 41:

                break;
            case 42:

                break;
            case 43:

                break;
            case 61:

                break;
            case 62:

                break;
            case 63:

                break;
        }
        yield return null;
    }

    public IEnumerator Pre_BossMovement()
    {
        if (lookAtP1 && Vector3.Distance(transform.position, _Player1.transform.position) <= (skillRange1 / 2))
        {
            ani.SetTrigger("isBacking");
            yield return new WaitForSeconds(timing);
            rb.AddForce(backwardForce * -transform.forward, ForceMode.Impulse);
        }
        if (lookAtP2 && Vector3.Distance(transform.position, _Player2.transform.position) <= (skillRange1 / 2))
        {
            ani.SetTrigger("isBacking");
            yield return new WaitForSeconds(timing);
            rb.AddForce(backwardForce * -transform.forward, ForceMode.Impulse);
        }

        yield return null;
    }

    IEnumerator AIStartTimer()
    {
        yield return new WaitForSeconds(aiStartTime);
        Debug.Log("'AI' Started");
        yield return new WaitForSeconds(3);

        if (aiEnable)
        {
            coroutineThink = StartCoroutine(TimeOfThink());
        }
    }

    IEnumerator AIRestartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(aiStartTime);

            yield return new WaitForSeconds(5);
            if (aiEnable)
                yield return new WaitUntil(() => !aiEnable);
            if (!aiEnable)
                yield return new WaitUntil(() => aiEnable);
            if (aiEnable && coroutineThink != null)
            {
                Debug.Log("'AI' Restarted");
                coroutineThink = StartCoroutine(TimeOfThink());
                break;
            }
        }
    }

    public IEnumerator TimeOfThink()
    {
        while (IsStage1 && aiEnable)
        {
            PlayerDetect();
            //yield return new WaitForSeconds(0.3f);
            yield return new WaitUntil(() => isLockOn);
            SkillSelection();
            yield return coroutineAtk;
            //Debug.Log("A rountine is FINISH!");
            yield return new WaitForSeconds(aiReactTime);
            //Debug.Log("Will Start Again...");
        }

        if (IsStage2)
        {
            Debug.Log("Switch to stage2 with sth animation cutscene");
        }

        //They should play the same expect a AI movement decide will be added
        while (IsStage2 && aiEnable)
        {
            PlayerDetect();
            //yield return new WaitForSeconds(0.3f);
            yield return new WaitUntil(() => isLockOn);
            SkillSelection();
            yield return coroutineAtk;
            //Debug.Log("A rountine is FINISH!");
            yield return new WaitForSeconds(aiReactTime);
            //Debug.Log("Will Start Again...");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skillRange1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, skillRange2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, skillRange3);
    }

}
