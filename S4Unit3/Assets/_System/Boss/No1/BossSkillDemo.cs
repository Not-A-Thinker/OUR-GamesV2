using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossSkillDemo : MonoBehaviour
{
    private GameObject _Player1;
    private GameObject _Player2;

    public GameObject instantiatePoint;

    [Header("Boss Skill Prefabs")]
    public GameObject windBlade;
    public GameObject vacuumArea;
    public GameObject bubbleAttack;
    public GameObject tornadoLinear;
    public GameObject tornadoSForm;
    public GameObject tornadoSmall;
    public GameObject TornadoBigOne;
    public GameObject windBladeBoomerang;
    public GameObject tornadoSpecialAttack;

    public Coroutine TornadoTracking;

    [Header("Skill")]
    public int NumTornadoToSpawn = 2;
    [Range(1, 5)] public int NumBoomerangSpawn = 3;
    [Range(1, 2)] public int BoomerangCase = 1;
    public int waveToSpawnWindBlade = 3;
    public int waveToSpawnTornado = 1;
    public int _STACount = 1;

    [Header("Skill Tornado Gattai")]
    public bool tornadoGattaiIsExisted;

    [Header("State")]
    [SerializeField] float _range = 20f;
    Vector3 orgPos;

    private bool isRushing = false;
    bool isTeleported;

    [Header("Boss Animator")]
    [SerializeField] Animator Boss1Animator;

    void Start()
    {
        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        orgPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //WindBlade();
            StartCoroutine(WindBlade16(2));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            VacuumPressure();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))//To Active Bubble Attack
        {
            if (!isTeleported)
            {
                StartCoroutine(BubbleTranslation());
            }
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

        //The Update ends here.
    }

    #region SkillSets

    public void WindBlade()
    {
        StartCoroutine(animationPlaytime("isSwing"));
        Instantiate(windBlade, transform.position, transform.rotation);
    }

    public IEnumerator WindBlade16(int wave)
    {
        Boss1Animator.SetBool("isBlade", true);
        for (int i = 0; i < wave; i++)//Wave to spawn
        {
            for (int j = 0; j < 16; j++)
            {
                float angleValue = (22.5f * j + (22.5f / 2 * i)) % 360f;

                Quaternion angle = Quaternion.AngleAxis(angleValue + transform.localEulerAngles.y, Vector3.up);
                GameObject go = Instantiate(windBlade, instantiatePoint.transform.position, angle);
                go.name = "WB_" + i + "," + j;
            }          
            yield return new WaitForSeconds(0.6f);
        }  
        yield return null;
        Boss1Animator.SetBool("isBlade", false);
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
        GameObject bubble = Instantiate(bubbleAttack, _Player1.transform.position, Quaternion.identity);
        bubble.GetComponent<Skill_BubbleAttack>().targetName = "Player1";
    }

    public void TornadoAttack()
    {
        StartCoroutine(animationPlaytime("isTonado"));
        //Vector3 rushPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
        //_agent.SetDestination(rushPos);

        Instantiate(tornadoSForm, instantiatePoint.transform.position, instantiatePoint.transform.rotation);
    }

    public void TornadoGattai()//Shoot out two tornado and will merge together
    {
        StartCoroutine(animationPlaytime("isTonado"));
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
            float ranX = Random.Range(transform.position.x - _range, transform.position.x + _range + 1);
            float ranY = Random.Range(transform.position.z - _range, transform.position.z + _range + 1);
            Vector3 ranPos = new Vector3(ranX, 0, ranY);

            if (!Physics.Raycast(ranPos, transform.up * 10, 20))//Will need to change
            {
                GameObject go = Instantiate(tornadoSmall, ranPos, Quaternion.identity);
                go.name = "tornadoSmall_" + id;
                id++;
                go.GetComponent<Skill_TornadoGattai>().playerSelect = playerRan;
            }
            else { i--; }

        }
        TornadoTracking = StartCoroutine(TornadoGattaiTracktion());
    }

    public IEnumerator TornadoGattaiTracktion()//May need to fix
    {
        StartCoroutine(animationPlaytime("isTonado"));
        if (GameObject.Find("tornadoSmall_1") == null)
            yield break;
        
        Skill_TornadoGattai tornado1 = GameObject.Find("tornadoSmall_1").GetComponent<Skill_TornadoGattai>();

        yield return new WaitUntil(() => tornado1.b_CanGattai);

        GameObject go = Instantiate(TornadoBigOne, tornado1.transform.position, Quaternion.identity);
        go.GetComponent<Skill_TornadoBigOne>().playerSelect = tornado1.playerSelect;

        Debug.Log("Here's come the big one! And select:" + tornado1.playerSelect);

        yield return null;
    }

    public IEnumerator EightTornado(int wave)
    {
        StartCoroutine(animationPlaytime("isTonado"));
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
        yield return null;
    }

    public void WindBladeBoomerang()
    {
        switch (BoomerangCase)
        {
            case 1:
                Vector3 forwardLeft = Quaternion.Euler(0, -60, 0) * transform.forward * _range;
                StartCoroutine(animationPlaytime("isSwing"));
                for (int i = 0; i < 5; i++)
                {
                    GameObject go_boomerang = Instantiate(windBladeBoomerang, transform.position, transform.rotation);
                    go_boomerang.transform.name = "Boomerang_" + i;

                    Vector3 vector = transform.position + Quaternion.Euler(0, (150 / 5) * i, 0) * forwardLeft;
                    vector.y = 1;
                    go_boomerang.GetComponent<Skill_WindBladeBoomerang>().tarPos = vector;
                    //Debug.Log(Vector3.Distance(transform.position, go_boomerang.GetComponent<Skill_WindBladeBoomerang>().tarPos));
                }
                break;
            case 2:
                int ran = Random.Range(1, 3);
                StartCoroutine(animationPlaytime("isSwing"));
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
        StartCoroutine(animationPlaytime("isTonado"));
        for (int i = 0; i < 4; i++)
        {
            int rnd = Random.Range(0, 360);
            Quaternion rndRot = Quaternion.Euler(0, rnd, 0);
            GameObject go_STA = Instantiate(tornadoSpecialAttack, transform.position, rndRot);
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
            float ranX = Random.Range(_Player1.transform.position.x - 3, _Player1.transform.position.x + _range + 3);
            float ranY = Random.Range(_Player1.transform.position.z - 3, _Player1.transform.position.z + _range + 3);
            Vector3 ranPos = new Vector3(ranX, 0, ranY);

            if (!Physics.Raycast(ranPos, transform.up * 10, 20))
            {
                isTeleported = true;
                transform.position = ranPos;
                BubbleAttack();
                break;
            }
        }

        yield return new WaitForSeconds(2);

        transform.position = orgPos;
        isTeleported = false;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);     
    }


    IEnumerator animationPlaytime(string TriggerBool)
    {
        Boss1Animator.SetBool(TriggerBool, true);

        yield return new WaitForSeconds(0.5f);

        Boss1Animator.SetBool(TriggerBool, false);
    }
}