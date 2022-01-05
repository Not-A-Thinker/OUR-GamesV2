using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TornadoBigOne : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float secondToDie = 10f;

    Vector3 _targetPos;

    PlayerState playerState;

    BossCameraControl cameraControl;
    BossSkillDemo bossSkill;

    private GameObject _Player1;
    private GameObject _Player2;

    public int playerSelect;

    private bool b_ISLocked;

    void Start()
    {
        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();
        bossSkill = GameObject.Find("Boss").GetComponent<BossSkillDemo>();
        if (GameObject.Find("Boss Stando").activeInHierarchy)
        {
            bossSkill = GameObject.Find("Boss Stando").GetComponent<BossSkillDemo>();
        }
    }

    void Update()
    {
        float distTarget = Vector3.Distance(transform.position, _targetPos);

        if (!b_ISLocked)
        {
            if (playerSelect == 1)
            {
                _targetPos = _Player1.transform.position;
                b_ISLocked = true;
            }
            else if (playerSelect == 2)
            {
                _targetPos = _Player2.transform.position;
                b_ISLocked = true;
            }
            else
            {
                Debug.Log("Oh fuck, the big one there is no player selected!");
                _targetPos = _Player2.transform.position;
                b_ISLocked = true;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);

        Destroy(gameObject, secondToDie);
    }

    private void OnDestroy()
    {
        cameraControl.ChangeTargetWeight(3, 1);

        bossSkill.tornadoGattaiIsExisted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();

            }
            Debug.Log("To the moon!");
        }

        if (other.gameObject.tag == "Breakable Wall")
        {
            Destroy(other.gameObject);
        }
    }
}
