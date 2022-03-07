using System.Collections;
using UnityEngine;

public class Skill_TornadoBigOne : MonoBehaviour
{
    PlayerState playerState;
    Move move;
    BossSkillDemo bossSkill;
    BossAI_Wind bossAI;
    BossCameraControl cameraControl;

    public GameObject _Player1;
    public GameObject _Player2;

    [Header("Tracking Mode")]
    [Tooltip("Mode 1 is using counter, 2 is using timer for tracking")]
    [Range(1, 2)] [SerializeField] int _trackingMode = 1;

    [Header("Tracking Basic Setting")]
    [SerializeField] float speed = 6f;
    [SerializeField] float _rotationSpeed = 8f;
    public int playerSelect = 1;

    [Header("Track Mode 1 Setting")]
    [SerializeField] float _angleOfTracking = 15;
    public int _canTrackNum = 3;
    [SerializeField] int _trackCount = 0;

    [Header("Track Mode 2 Setting")]
    public float _trackingTime = 7;

    [Header("Despawn Setting")]
    [SerializeField] float secondToDie = 10f;

    [Header("Debug Testing Log")]
    [SerializeField] bool _outOfTrack;
    [SerializeField] bool _isTracking;
    [SerializeField] bool _firstTime = false;
    [SerializeField] bool _timerStarted = false;
    [SerializeField] bool _showDetectLine = false;

    Vector3 selfPos;
    Vector3 _targetPos;

    float playerAngle;

    void Start()
    {
        if (_Player1 == null && playerSelect == 1)
        { _Player1 = GameObject.Find("Player1"); }
        else if (_Player2 == null && playerSelect == 2)
        { _Player2 = GameObject.Find("Player2"); }

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
            _targetPos.y = transform.position.y;
            playerAngle = Vector3.Angle(_Player1.transform.position - transform.position, transform.forward);
        }
        else if (playerSelect == 2)
        {
            _targetPos = _Player2.transform.position;
            _targetPos.y = transform.position.y;
            playerAngle = Vector3.Angle(_Player2.transform.position - transform.position, transform.forward);
        }
        else
        {
            Debug.Log("Oh fuck, the big one there has no player to select!");
            _targetPos = _Player2.transform.position;
            _targetPos.y = transform.position.y;
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
                if (_trackingMode == 2)
                { _rotationSpeed = 5f;}
                else
                { _rotationSpeed = 8f;}
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Vector3 is zero!: ");
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
        }

        //This part is mainly for tracking the player or distracking after the count number is full.
        if (_firstTime && _isTracking && !_outOfTrack)
        {
            //transform.LookAt(_targetPos);
            transform.position += transform.forward * speed * Time.deltaTime;

            //transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);
        }
        else if (_firstTime && !_isTracking && !_outOfTrack)
        {
            if (isOutOfTrackion && !_timerStarted)
            {
                StartCoroutine(Timer());
                _timerStarted = true;
            }

            //transform.LookAt(_targetPos);
            transform.position += transform.forward * speed * Time.deltaTime;

            //transform.position = Vector3.MoveTowards(transform.position, _targetPos, speed * Time.deltaTime);
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
        if (_trackingMode == 1)
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
        else if (_trackingMode == 2)
        {
            Debug.Log("Start Tracking!");
            yield return new WaitForSeconds(_trackingTime);
            _outOfTrack = true;
            Debug.Log("is out of track!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();
                move = other.GetComponent<Move>();
                StartCoroutine(move.KnockUp());
            }
        }

        if (other.gameObject.tag == "Breakable Wall")
        {
            Destroy(other.gameObject);
        }
    }
}
