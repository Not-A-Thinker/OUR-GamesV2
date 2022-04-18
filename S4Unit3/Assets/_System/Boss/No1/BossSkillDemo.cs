using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillDemo : MonoBehaviour
{
    Rigidbody rb;

    BossCameraControl cameraControl;
    BossAI_Wind bossAI;

    private GameObject _Player1;
    private GameObject _Player2;

    public GameObject instantiatePoint;
    public GameObject windBallPoint;

    [Header("Debug")]
    [SerializeField] bool DrawGizmo = false;
    [SerializeField] float _wingBladeRange = 75f;

    [Header("Boss Skill Prefabs")]
    public GameObject windBlade;
    public GameObject outerWindBlade;
    public GameObject[] outerWindBladePoint;
    public GameObject outerWindBladeGenerateVfx;
    public GameObject vacuumArea;
    public GameObject bubbleAttack;
    public GameObject tornadoLinear;
    public GameObject tornadoSForm;
    public GameObject tornadoSmall;
    public GameObject TornadoBigOne;
    public GameObject windBladeBoomerang;
    public GameObject tornadoSpecialAttack;
    public GameObject mistObj;
    public GameObject windBall;

    [Header("Boss Stando Prefabs")]
    public GameObject bossStando;
    public GameObject SpawnPoint;

    [Header("Coroutine")]
    public Coroutine TornadoTracking;

    [Header("Skill Status")]
    public bool isMeleeAttacking;
    public bool canMistAgain = true;
    public bool canStandoAgain;

    [Header("Skill CD Timer")]
    public float mistCDTime = 15;
    public float standoCDTime = 30;

    [Header("Skill Tweak")]
    public int NumTornadoToSpawn = 2;
    [Space]
    [Range(1, 5)] public int NumBoomerangSpawn = 3;
    [Range(1, 2)] public int BoomerangCase = 1;
    [Space]
    public int waveToSpawnWindBlade = 3;
    public int waveToSpawnTornado = 1;
    public int _STACount = 1;

    [Header("Skill Outer WindBlade")]
    [Tooltip("Mode1 是漸進,Mode2 是同步發射")]
    [Range(1, 2)] [SerializeField] int owb_WorkMode = 1;
    [SerializeField] bool owb_DebugLog = false;

    [Header("Skill Tornado Gattai")]
    public bool tornadoGattaiIsExisted;

    [Header("Skill Wind Hole")]
    public Transform skillPoint;
    public GameObject windHole;
    public LayerMask layerMask;
    public Vector3[] otherPos = new Vector3[5];

    [Header("Skill Tail Attack")]
    [SerializeField] float tailForwardForce = 10000;
    [Space]
    [SerializeField] float tailDashDuration;// 控制尾部衝刺時間
    [SerializeField] float tailDashSpeed;// 尾部衝刺速度
    bool isTailDash;// 是否在衝刺
    float tailDashTime;// 剩餘衝刺時間

    [Header("Skill Head Attack")]
    [SerializeField] float headDashDuration;// 控制頭部衝刺時間
    [SerializeField] float headDashSpeed;// 頭部衝刺速度
    bool isHeadDash;// 是否在衝刺
    float headDashTime;// 剩餘衝刺時間
    private Vector3 directionXOZ;

    [Header("State")]
    [SerializeField] float _skillRange = 20f;// 主要為控制召喚技能的範圍
    Vector3 orgPos;

    private bool isRushing = false;
    //private bool isTeleported;

    [Header("Boss Animator")]
    [SerializeField] Animator Boss1Animator;

    private void Awake()
    {
        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        bossAI = GetComponent<BossAI_Wind>();
        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        orgPos = transform.position;

        tailDashTime = tailDashDuration;
        headDashTime = headDashDuration;
    }

    void Update()
    {
        #region KeyCodeSets
        if (bossAI._TestingMode || true)
        {
            //This Key Input is Just for testing, should be remove after getting release.
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //WindBlade();
                //StartCoroutine(WindBlade16(2));
                WindBlade16AnimationTrigger();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //WindBallsAttack();
                BossWindBalls();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))//To Active Bubble Attack
            {
                StartCoroutine(OuterWindBlade(3));
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                TornadoAttack();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                TornadoGattai();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartCoroutine(EightTornado(waveToSpawnTornado));
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                WindBladeBoomerang();
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                TornadoSpecialAttack();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                StartCoroutine(WindHole(1, 10));
            }
        }
        #endregion

        directionXOZ = transform.forward;// forward 指向物體當前的前方
        directionXOZ.y = 0f;// 只做平面的上下移動和水平移動，不做高度上的上下移動

        //The Update ends here.
    }

    private void FixedUpdate()
    {
        if (isTailDash)
        {
            if (tailDashTime <= 0)// reset
            {
                isTailDash = false;

                tailDashTime = tailDashDuration;
            }
            else
            {
                tailDashTime -= Time.deltaTime;
                rb.velocity = directionXOZ * tailDashTime * tailDashSpeed;
            }
        }
        if (isHeadDash)
        {
            if (headDashTime <= 0)// reset
            {
                isHeadDash = false;

                headDashTime = headDashDuration;
            }
            else
            {
                headDashTime -= Time.deltaTime;
                rb.velocity = directionXOZ * headDashTime * headDashSpeed;
            }
        }
    }


    #region Stage1_SkillSets
    public void WindBlade()
    {
        Boss1Animator.SetTrigger("Skill_SwingAttack");
        Instantiate(windBlade, transform.position, transform.rotation);
    }

    public IEnumerator WindBlade16(int wave)
    {
        Boss1Animator.SetBool("isBlade", true);
        for (int i = 0; i < wave; i++)//Wave to spawn
        {
            //Boss1Animator.SetTrigger("Skill_WindBlade");
            for (int j = 0; j < 16; j++)
            {
                float angleValue = (22.5f * j + (22.5f / 2 * i) + i) % 360f + 45;

                Quaternion angle = Quaternion.AngleAxis(angleValue + transform.localEulerAngles.y, Vector3.up);
                GameObject go = Instantiate(windBlade, instantiatePoint.transform.position, angle);
                go.name = "WB_" + i + "," + j + "," + go.transform.localEulerAngles.y;
            }          
            yield return new WaitForSeconds(0.5f);
        }

        Boss1Animator.SetBool("isBlade", false);
        yield return null;
    }

    public void WindBlade16AnimationTrigger()
    {
        Boss1Animator.SetTrigger("Skill_WindBlade");
    }

    public void BossOuterWindBlade()
    {
        Boss1Animator.SetTrigger("Skill_OWB");
    }

    public IEnumerator OuterWindBlade(int sNum)
    {
        Boss1Animator.SetTrigger("Skill_OWB");

        int[] iNum = new int[sNum];

        for (int i = 0; i < sNum; i++)
        {
            //The Upper Area is detect which point to spawn the blade.
            int ran = Random.Range(0, outerWindBladePoint.Length); //Draw a random number between the Length of outerWindBladePoint.

            if (i > 1)
            {
                //This loop is use to detect does the number get a opposite side pos 
                for (int j = 0; j < i; j++)
                {
                    if (iNum[j] == ran || iNum[j] - ran == 0)
                    {
                        i--;
                        if (owb_DebugLog) Debug.Log("Draw an Opposite or Same Number! And the Num is: " + ran + ", and the iNum is: " + iNum[j]);
                    }
                    else { iNum[i] = ran; }
                }
            }
            else { iNum[i] = ran; } //This make sure the data can be compare with the other i.
        }

        //This Area is decide which method to spawn the blade.
        if (owb_WorkMode == 1)
        {
            for (int i = 0; i < sNum; i++)
            {
                if (owb_DebugLog) Debug.Log(i + ": " + iNum[i]);

                bossAI.outerWindBladeAlert[iNum[i]].SetTrigger("WindBlade Alert");
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(InstantiateOuterWindBlade( outerWindBladePoint[iNum[i]].transform.position, outerWindBladePoint[iNum[i]].transform.rotation,1));
                //Instantiate(outerWindBlade, outerWindBladePoint[iNum[i]].transform.position, outerWindBladePoint[iNum[i]].transform.rotation);
            }
        }
        else if (owb_WorkMode == 2)
        {
            for (int i = 0; i < sNum; i++)
            {
                if (owb_DebugLog) Debug.Log(i + ": " + iNum[i]);
                bossAI.outerWindBladeAlert[iNum[i]].SetTrigger("WindBlade Alert");
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < sNum; i++)
            {
                StartCoroutine(InstantiateOuterWindBlade(outerWindBladePoint[iNum[i]].transform.position, outerWindBladePoint[iNum[i]].transform.rotation,1));
                //Instantiate(outerWindBlade, outerWindBladePoint[iNum[i]].transform.position, outerWindBladePoint[iNum[i]].transform.rotation);
            }
        }
    }
    IEnumerator InstantiateOuterWindBlade(  Vector3 pos, Quaternion rot , float waitTime)
    {
        GameObject m_outerWindBladeGenerateVfx =  Instantiate(outerWindBladeGenerateVfx, pos, rot);

        yield return new WaitForSeconds(waitTime);

        GameObject m_outerWindBlade =  Instantiate(outerWindBlade, pos, rot);

        if (m_outerWindBladeGenerateVfx.GetComponent<objectPortal>() != null)
        {
            var op = m_outerWindBladeGenerateVfx.GetComponent<objectPortal>(); 
            if(op.distanceTraget != null)
            {
                var dt = op.distanceTraget.GetComponent<DistanceTraget>();
                dt.m_objectTraget = m_outerWindBlade.transform;
            }
        }


        Destroy(m_outerWindBladeGenerateVfx, 1);

    }

    public void VacuumPressure()
    {
        int ran = Random.Range(1, 3);
        if (ran == 1)
        {
            Instantiate(vacuumArea, _Player1.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(vacuumArea, _Player2.transform.position, Quaternion.identity);
        }
    }

    public void BubbleAttack()
    {
        GameObject bubble = Instantiate(bubbleAttack, _Player1.transform.position, Quaternion.Euler(0, 180, 0));
        bubble.GetComponent<Skill_BubbleAttack>().targetName = "Player1";
    }

    public void TornadoAttack()
    {
        //StartCoroutine(animationPlaytime("isTonado"));
        Boss1Animator.SetTrigger("Skill_Tornado");
        //Vector3 rushPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
        //_agent.SetDestination(rushPos);

        Instantiate(tornadoSForm, instantiatePoint.transform.position, instantiatePoint.transform.rotation);
    }

    public void TornadoGattai()//Shoot out two tornado and will merge together
    {
        //StartCoroutine(animationPlaytime("isTonado"));
        Boss1Animator.SetTrigger("Skill_Tornado");

        tornadoGattaiIsExisted = true;

        //Select one of the player to attack on
        int playerRan;
        if (_Player1.GetComponent<PlayerState>().GetPlayerIsDead() && !_Player2.GetComponent<PlayerState>().GetPlayerIsDead())
            playerRan = 2;
        else if (!_Player1.GetComponent<PlayerState>().GetPlayerIsDead() && _Player2.GetComponent<PlayerState>().GetPlayerIsDead())
            playerRan = 1;
        else
            playerRan = Random.Range(1, 3);

        //Spawn some Small Tornado toward the selected player 
        int id = 1;
        for (int i = 0; i < NumTornadoToSpawn; i++)
        {
            float ranX = Random.Range(instantiatePoint.transform.position.x - _skillRange, instantiatePoint.transform.position.x + _skillRange + 1);
            float ranY = Random.Range(instantiatePoint.transform.position.z - _skillRange, instantiatePoint.transform.position.z + _skillRange + 1);
            Vector3 ranPos = new Vector3(ranX, 0, ranY);

            if (!Physics.Raycast(ranPos, transform.up * 10, 20))//Will need to change
            {
                GameObject go = Instantiate(tornadoSmall, ranPos, Quaternion.identity);
                go.name = "tornadoSmall_" + id;
                id++;
                Skill_TornadoGattai goSkill = go.GetComponent<Skill_TornadoGattai>();
                goSkill.playerSelect = playerRan;
                if (goSkill.playerSelect == 1) { goSkill._Player1 = _Player1; }
                else if (goSkill.playerSelect == 2) { goSkill._Player2 = _Player2; }
                
            }
            else { i--; }

        }
        TornadoTracking = StartCoroutine(TornadoGattaiTracktion());
    }

    public IEnumerator TornadoGattaiTracktion()//May need to fix
    {
        //StartCoroutine(animationPlaytime("isTonado"));
        //Boss1Animator.SetTrigger("Skill_Tornado");
        if (GameObject.Find("tornadoSmall_1") == null)
            yield break;
        
        Skill_TornadoGattai tornado1 = GameObject.Find("tornadoSmall_1").GetComponent<Skill_TornadoGattai>();

        yield return new WaitUntil(() => tornado1.b_CanGattai);

        GameObject go = Instantiate(TornadoBigOne, tornado1.transform.position, Quaternion.identity);
        Skill_TornadoBigOne goBigOne = go.GetComponent<Skill_TornadoBigOne>();
        goBigOne.playerSelect = tornado1.playerSelect;
        if(goBigOne.playerSelect == 1) { goBigOne._Player1 = _Player1; }
        else if (goBigOne.playerSelect == 2) { goBigOne._Player2 = _Player2; }
        
        tornadoGattaiIsExisted = false;
        Debug.Log("Here's come the big one! And select:" + tornado1.playerSelect);

        yield return null;
    }

    public IEnumerator EightTornado(int wave)
    {
        //StartCoroutine(animationPlaytime("isTonado"));
        Boss1Animator.SetTrigger("Skill_EightTornado");
        for (int i = 0; i < wave; i++)//Wave to spawn
        {
            float SelfPosY = transform.rotation.y;
            for (int j = 0; j < 8; j++)
            {
                float angleValue = (45 * j) % 360f;

                //Quaternion rotation = Quaternion.Euler(0, angleValue, 0);
                Quaternion angle = Quaternion.AngleAxis(angleValue + transform.localEulerAngles.y, Vector3.up);
                Instantiate(tornadoLinear, instantiatePoint.transform.position, angle);
            }
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void WindBladeBoomerang()
    {
        switch (BoomerangCase)
        {
            case 1:
                Vector3 forwardLeft = Quaternion.Euler(0, -60, 0) * transform.forward * _skillRange;
                Boss1Animator.SetTrigger("Skill_Boomerang");
                for (int i = 0; i < 5; i++)
                {
                    GameObject go_boomerang = Instantiate(windBladeBoomerang, new Vector3(transform.position.x, 1.85f,transform.position.z), transform.rotation);
                    go_boomerang.transform.name = "Boomerang_" + i;

                    Vector3 vector = transform.position + Quaternion.Euler(0, (150 / 5) * i, 0) * forwardLeft;
                    vector.y = 1;
                    go_boomerang.GetComponent<Skill_WindBladeBoomerang>().tarPos = vector;
                    //Debug.Log(Vector3.Distance(transform.position, go_boomerang.GetComponent<Skill_WindBladeBoomerang>().tarPos));
                }
                break;
            case 2:
                int ran = Random.Range(1, 3);
                Boss1Animator.SetTrigger("Skill_Boomerang");
                GameObject go = Instantiate(windBladeBoomerang, instantiatePoint.transform.position, transform.rotation);
                if (ran == 1)
                    go.GetComponent<Skill_WindBladeBoomerang>().tarPos = _Player1.transform.position;
                else if (ran == 2)
                    go.GetComponent<Skill_WindBladeBoomerang>().tarPos = _Player2.transform.position;
                break;
        }
    }

    public void TornadoSpecialAttack()
    {
        //StartCoroutine(animationPlaytime("isTonado"));
        Boss1Animator.SetTrigger("Skill_PinBall");
        Level1GameData.b_isCutScene = true;
        //Boss1Animator.SetTrigger("Skill_Tornado");
        
    }
    public void TornadoSpecialAttackAnimation()
    {
        for (int i = 0; i < 4; i++)
        {
            int rnd = Random.Range(0, 360);
            Quaternion rndRot = Quaternion.Euler(0, rnd, 0);
            GameObject go_STA = Instantiate(tornadoSpecialAttack, instantiatePoint.transform.position, rndRot);
            go_STA.name = "STA_" + (i + 1);
        }
        _STACount++;
    }

    IEnumerator EnemyRush(Vector3 targetPos)
    {
        float step = 1f;

        while (isRushing)
        {
            if (step >= (targetPos - transform.position).magnitude)
            {
                transform.position = targetPos;
                isRushing = false;
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator BubbleTranslation()//Teleport to other players, now is only for targeting player1
    {
        while (true)
        {
            float ranX = Random.Range(_Player1.transform.position.x - 3, _Player1.transform.position.x + _skillRange + 3);
            float ranY = Random.Range(_Player1.transform.position.z - 3, _Player1.transform.position.z + _skillRange + 3);
            Vector3 ranPos = new Vector3(ranX, 0, ranY);

            if (!Physics.Raycast(ranPos, transform.up * 10, 20))
            {
                //isTeleported = true;
                transform.position = ranPos;
                BubbleAttack();
                break;
            }
        }

        yield return new WaitForSeconds(2);

        transform.position = orgPos;
        //isTeleported = false;
    }

    //This is the end of stage1 skill sets.
    #endregion

    #region Stage2_SkillSets
    public IEnumerator WindHole(int wave, int spawnNum)
    {
        Boss1Animator.SetTrigger("Skill_WindHole");
        for (int i = 0; i < wave; i++)
        {
            for (int p = 0; p < otherPos.Length; p++)
            {
                otherPos[p] = Vector3.zero;
            }

            for (int j = 0; j < spawnNum; j++)
            {
                float ranX = Random.Range(skillPoint.position.x - _skillRange + 1, skillPoint.position.x + _skillRange - 1);
                float ranY = Random.Range(skillPoint.position.z - _skillRange + 1, skillPoint.position.z + _skillRange - 1);
                Vector3 ranPos = new Vector3(ranX, 0.1f, ranY);

                otherPos[j] = ranPos;
                bool isPass = false;

                RaycastHit cirHit;
                if (!Physics.SphereCast(ranPos, 3, transform.up * .5f, out cirHit, 20, layerMask))
                {

                    for (int k = 0; k < otherPos.Length; k++)
                    {
                        //Debug.Log(Vector3.Distance(otherPos[j], otherPos[k]));
                        if (Vector3.Distance(otherPos[j], otherPos[k]) <= 3 && otherPos[j] != otherPos[k])
                        {
                            Debug.Log("Too Close!");
                            isPass = true;
                            break;
                        }
                    }

                    if (!isPass)
                    {
                        Instantiate(windHole, ranPos, Quaternion.identity);
                        //Debug.Log("Spawn!");
                        yield return new WaitForSeconds(0.15f);
                    }
                    else { j--; }
                }
                else { j--; }
            }
            yield return new WaitForSeconds(0.15f);
        }
        yield return null;
    }

    public void BossStando()
    {
        GameObject stando =  Instantiate(bossStando, SpawnPoint.transform.position, Quaternion.Euler(0, 180, 0));
        stando.name = "Boss Stando";
        cameraControl.ChangeTarget(4, stando.transform);
    }

    public void BossTailAttackAnimation()
    {
        //rb.AddForce(tailForwardForce * transform.forward, ForceMode.Impulse);
        //rb.velocity = Vector3.zero;
        isTailDash = true;
    }

    public void BossTailAttack()
    {
        Boss1Animator.SetTrigger("Skill_TailAttack");
    }

    public void BossWingAreaAttack()
    {
        Boss1Animator.SetTrigger("Skill_WingAreaAttack");
    }

    public void BossWingAttack()
    {
        Boss1Animator.SetTrigger("Skill_WingAttack");
    }

    public void BossHeadAttackAnimation()
    {
        //rb.AddForce(tailForwardForce * transform.forward, ForceMode.Impulse);
        //rb.velocity = Vector3.zero;
        isHeadDash = true;
    }

    public void BossHeadAttack()
    {
        Boss1Animator.SetTrigger("Skill_HeadAttack");
    }

    public void MistAttack()
    {
        Instantiate(mistObj, transform.position, Quaternion.identity);
    }

    //This is the end of stage2 skill sets.
    #endregion

    #region AllStage_SkillSets
    public void BossWindBalls()
    {
        Boss1Animator.SetTrigger("Skill_WindBall");
    }

    public IEnumerator WindBalls(int sNum, int wave)
    {
        for (int w = 0; w < wave; w++)
        {
            for (int i = 0; i < sNum + w * 2; i++)
            {
                Vector2 cirRnd = Random.insideUnitCircle * (_skillRange / 2 * 0.85f) * (w / 2 + .85f) * 1.15f;
                float rndX = transform.position.x + cirRnd.x;
                float rndY = Random.Range(windBallPoint.transform.position.y + 4, windBallPoint.transform.position.y + 5f);
                float rndZ = transform.position.z + cirRnd.y;
                Vector3 forcePos = new Vector3(rndX, rndY, rndZ);

                GameObject wB = Instantiate(windBall, windBallPoint.transform.position, Quaternion.identity);
                wB.GetComponent<Rigidbody>().AddForce(forcePos * 12, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(.6f);
        }
    }
    //This is the end of all stage skill sets.
    #endregion

    public IEnumerator MistCDTimer(float cdTime)
    {
        canMistAgain = false;
        yield return new WaitForSeconds(cdTime);
        canMistAgain = true;
    }

    public IEnumerator StandoCDTimer(float cdTime)
    {
        canStandoAgain = false;
        yield return new WaitForSeconds(cdTime);
        canStandoAgain = true;
    }

    private void OnDrawGizmos()
    {
        if (DrawGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _skillRange);

            Gizmos.DrawWireSphere(transform.position, _wingBladeRange);
        }
    }


    IEnumerator animationPlaytime(string TriggerBool)
    {
        Boss1Animator.SetBool(TriggerBool, true);

        yield return new WaitForSeconds(0.5f);

        Boss1Animator.SetBool(TriggerBool, false);
    }

}
