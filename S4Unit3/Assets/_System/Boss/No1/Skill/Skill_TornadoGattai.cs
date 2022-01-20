using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TornadoGattai : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float secondToDie = 12f;

    [SerializeField] int gattaiCount = 2;
    [SerializeField] int count = 1;

    Vector3 _targetPos;

    PlayerState playerState;
    BossSkillDemo boss;
    BossAI_Wind bossAI;
    BossCameraControl cameraControl;

    private GameObject _Player1;
    private GameObject _Player2;

    public GameObject TornadoBigOne;

    public int playerSelect;

    public bool b_CanGattai;
    private bool b_ISLocked;

    void Start()
    {
        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        bossAI = GameObject.Find("Boss").GetComponent<BossAI_Wind>();
        
        if (bossAI.isStandoMode)
        {
            boss = GameObject.Find("Boss Stando").GetComponent<BossSkillDemo>();
        }
        else
        {
            boss = GameObject.Find("Boss").GetComponent<BossSkillDemo>();
        }

        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();

        count = 1;
        //gattaiCount = GameObject.FindGameObjectsWithTag("TornadoSmall").Length; //This may not work if the Gattai Function was called more than once.
        gattaiCount = 2;

        b_CanGattai = false;
    }

    void Update()
    {
        if (!b_ISLocked)
        {
            if (playerSelect == 1)
            {
                _targetPos = _Player1.transform.position;
                b_ISLocked = true;
            }
            else if(playerSelect == 2)
            {
                _targetPos = _Player2.transform.position;
                b_ISLocked = true;
            }
            else
            {
                Debug.Log("Oh shit, there is no player selected!");
                _targetPos = _Player2.transform.position;
                b_ISLocked = true;
            }
        }

        if (Vector3.Distance(transform.position, _targetPos) <= 0.1f && GameObject.FindGameObjectsWithTag("TornadoSmall").Length <= 1)
        {
            b_ISLocked = true;
            //int ran = Random.Range(1, 3);
            //if (ran == 1)
            //{
            //    _targetPos = _Player1.transform.position;
            //}
            //else
            //{
            //    _targetPos = _Player2.transform.position;
            //}
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);

        Destroy(gameObject, secondToDie);
    }

    private void OnDestroy()
    {
        LastDetect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TornadoSmall")
        {
            //Debug.Log("Touch!");
            count++;
            //Debug.Log(count);

            if (count == gattaiCount)
            {
                //Debug.Log("Gattai!");
                b_CanGattai = true;

                Destroy(gameObject, .3f);
            }
        }

        if (other.gameObject.tag == "Wall")
        {
            if (boss.TornadoTracking != null)
            {
                boss.StopCoroutine("TornadoGattaiTracktion");
                boss.TornadoTracking = null;
            }

            boss.tornadoGattaiIsExisted = false;
            LastDetect();
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Breakable Wall")
        {
            if (boss.TornadoTracking != null)
            {
                boss.StopCoroutine("TornadoGattaiTracktion");
                boss.TornadoTracking = null;
            }

            LastDetect();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();
                //Debug.Log("Hit!");
            }
        }
    }

    private void LastDetect()
    {
        if (GameObject.FindGameObjectsWithTag("TornadoSmall").Length <= 1)
            cameraControl.ChangeTargetWeight(3, 2);
    }

    
}
