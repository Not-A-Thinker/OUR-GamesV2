using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TornadoBigOne : MonoBehaviour
{
    [Header("Tracking Setting")]
    [SerializeField] float speed = 6f;
    public float _angleOfTracking = 25;
    public int _canTrackNum = 3;
    public int playerSelect = 1;

    [Header("Despawn Setting")]
    [SerializeField] float secondToDie = 10f;

    Vector3 _targetPos;

    PlayerState playerState;
    BossSkillDemo bossSkill;
    BossAI_Wind bossAI;
    BossCameraControl cameraControl;

    private GameObject _Player1;
    private GameObject _Player2;

    private bool b_ISLocked;

    [Header("Debug Testing Log")]
    [SerializeField] int _trackCount = 0;
    [SerializeField] bool _outOfTrack;
    [SerializeField] bool _isTracking;
    [SerializeField] bool _firstTime = false;
    [SerializeField] bool _timerStarted = false;
    [SerializeField] bool _showDetectLine = false;


    Vector3 selfPos;

    float playerAngle;

    void Start()
    {
        _Player1 = GameObject.Find("Player1");
        _Player2 = GameObject.Find("Player2");

        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();

        bossAI = GameObject.Find("Boss").GetComponent<BossAI_Wind>();

        if (bossAI.isStandoMode)
        {
            bossSkill = GameObject.Find("Boss Stando").GetComponent<BossSkillDemo>();
        }
        else
        {
            bossSkill = GameObject.Find("Boss").GetComponent<BossSkillDemo>();
        }
    }

    void Update()
    {
        //This is for detect who to chase for.
        if (playerSelect == 1)
        {
            _targetPos = _Player1.transform.position;
            playerAngle = Vector3.Angle(_Player1.transform.position - transform.position, transform.forward);
        }
        else if (playerSelect == 2)
        {
            _targetPos = _Player2.transform.position;
            playerAngle = Vector3.Angle(_Player2.transform.position - transform.position, transform.forward);
        }
        else
        {
            Debug.Log("Oh fuck, the big one there has no player to select!");
            _targetPos = _Player2.transform.position;
            playerAngle = Vector3.Angle(_Player2.transform.position - transform.position, transform.forward);
        }

        bool isOutOfTrackion = playerAngle > _angleOfTracking;

        //This part is only for debugging.
        if (_showDetectLine)
        {
            //The Angle Axis Version of track showing
            var lineTrack = transform.position + (transform.forward * 30);
            Vector3 rotatedLine = Quaternion.AngleAxis(_angleOfTracking, transform.up) * lineTrack;
            rotatedLine.y = transform.position.y;
            Vector3 rotaterLineI = Quaternion.AngleAxis(-_angleOfTracking, transform.up) * lineTrack;
            rotaterLineI.y = transform.position.y;

            //Vector3 trackAngleLeft = Quaternion.Euler(0, _angleOfTracking, 0) * transform.forward;
            //trackAngleLeft.y = 0;
            //Vector3 trackAngleRight = Quaternion.Euler(0, -_angleOfTracking, 0) * transform.forward;
            //trackAngleRight.y = 0;

            Debug.DrawLine(transform.position, rotatedLine , Color.red);
            Debug.DrawLine(transform.position, rotaterLineI , Color.red);
        }

        //If is not out of track, then will look to the target position.
        if (!_outOfTrack)
        {
            Vector3 targetLookRotation = _targetPos - transform.position;
            if (targetLookRotation != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetLookRotation);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
            else
            {
                Debug.Log("Vector3 is zero!: ");
            }
        }

        //This part is for detecting the player is in the line. If so, is time to chase.
        selfPos = new Vector3(transform.position.x, 1, transform.position.z);

        RaycastHit isPlayerGetHit;
        if (Physics.Raycast(selfPos, transform.forward, out isPlayerGetHit, 30))
        {
            if (isPlayerGetHit.transform.tag == "Player")
            { _isTracking = true; _firstTime = true; }
        }
        else { _isTracking = false; }

        if (_showDetectLine)
        { Debug.DrawRay(selfPos, transform.forward * 30, Color.red); }

        //This part is mainly for tracking the player or distracking after the count number is full.
        if (_firstTime && _isTracking && !_outOfTrack)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);
        }
        else if (_firstTime && !_isTracking && !_outOfTrack)
        {
            if (isOutOfTrackion && !_timerStarted)
            {
                StartCoroutine(Timer());
                _timerStarted = true;

                transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);
            }
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, transform.forward * 10, speed * Time.deltaTime);
        }

        //transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);

        Destroy(gameObject, secondToDie);
    }

    private void OnDestroy()
    {
        cameraControl.ChangeTargetWeight(3, 2);

        bossSkill.tornadoGattaiIsExisted = false;
    }

    IEnumerator Timer()
    {
        _trackCount++;
        if (_trackCount == 3)
        {
            _outOfTrack = true;
            Debug.Log("is out of track!");
        }
        yield return new WaitForSeconds(1);
        _timerStarted = false;
        Debug.Log("Timer ends.");
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
        }

        if (other.gameObject.tag == "Breakable Wall")
        {
            Destroy(other.gameObject);
        }
    }
}
