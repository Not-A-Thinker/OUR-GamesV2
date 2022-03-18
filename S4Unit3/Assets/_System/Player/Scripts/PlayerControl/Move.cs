using System.Collections;
using UnityEngine;



public class Move : MonoBehaviour
{

    [SerializeField] UIcontrol UIcontrol;


    [Header("Player Move")]
    [SerializeField]
    private float maximumSpeed;
    private float oldTempSpeed;
    private float tempSpeed;



    [SerializeField] private float rotationSpeed;

    [SerializeField] CharacterController characterController;
    Camera cam;

    [SerializeField] PlayerAnimator _animation;

    [SerializeField] GameObject ShootRot;

    [SerializeField] GameObject Cha;
    Rigidbody rb;

    [Header("Player State")]

    public bool isPlayer1;
    public bool isPlayer2;

    public bool IsJoystick;
    public bool isSlowed;
    public bool isImMobilized;
    public bool isDashed;
    public bool isKnockUp;
    public bool isDashClose;

    public bool inCC = false;

    //demo1 used
    private GameObject Boss;

    float gravity= 20f;
    float vSpeed = 0f;
    //dash
    [Header("Player Dash")]

    CapsuleCollider _Collider;
    public float dashSpeed;
    public float dashTime;

    public int _DashTotal;
    int _DashNow;
    public int DashCD;
    //float _DashNowFloat;
    //int DashBar = 100;
    //public int DashUsed;
    //public int DashRestore;

    float angle;


    void Start()
    {
        _DashNow = _DashTotal;
        
        _Collider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        rb = GetComponent<Rigidbody>();
        tempSpeed = maximumSpeed;
        oldTempSpeed = maximumSpeed;
        Boss = GameObject.Find("Boss");
    }



    void Update()
    {
        if(isKnockUp)
        {
            if (!inCC)
            {
                StartCoroutine(KnockUp());
            }          
            characterController.Move(transform.up * 3 *Time.deltaTime);
            characterController.transform.rotation = new Quaternion(0, 90 * Time.deltaTime, 0,0);
        }
        

        if (characterController.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
        }

        ///°{Á×±ø¥R¯à
        //if (_DashNow < _DashTotal)
        //{
            //_DashNowFloat = DashBar;
            //_DashNowFloat += DashRestore * Time.deltaTime;
            //DashBar = Mathf.RoundToInt(_DashNowFloat);
            //Debug.Log(DashBar);
            //for (int i = 1 ; i <= _DashTotal ; i++)
            //{
            //    if (DashBar == DashUsed * i)
            //    {
            //        ///restore one Dash
            //        _DashNow++;
            //        int playerCount = 0;
            //        if (isPlayer1)
            //            playerCount = 1;
            //        if (isPlayer2)
            //            playerCount = 2;
            //        UIcontrol.EnergyBarChange(playerCount, _DashNow, false);
            //        Debug.Log("DashRestored!");
            //    }
            //}
        //}           

        if (isPlayer1)//wasd
        {         
            if (inCC == false)
            {              
                isKnockUp = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                //Move
                float horizontalInput = Input.GetAxis("HorizontalP1");
                float verticalInput = Input.GetAxis("VerticalP1");

                Vector3 movementDirection = new Vector3(horizontalInput, 0, -verticalInput);

                Vector3 velocity = movementDirection * maximumSpeed;

                // apply gravity acceleration to vertical speed:
                vSpeed -= gravity * Time.deltaTime;
                velocity.y = vSpeed;
                // include vertical speed in vel
                // convert vel to displacement and Move the character:
                characterController.Move(velocity * Time.deltaTime);
                //characterController.transform.Rotate(Vector3.up * horizontalInput * Time.deltaTime);

                if (movementDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    Cha.transform.rotation = Quaternion.RotateTowards(Cha.transform.rotation, toRotation, rotationSpeed * 100f * Time.deltaTime);
                    _animation.PlayerWalk(true);
                }

                if (movementDirection == Vector3.zero)
                {
                    _animation.PlayerWalk(false);
                }

                if(!isDashClose)
                {
                    if (Input.GetButtonDown("JumpP1") && _DashNow > 0)
                    {
                        UIcontrol.EnergyBarChange(1, _DashNow, true);
                        isDashed = true;
                        _animation.PlayerDash(true);
                        //Debug.Log("P1 Dashed");
                        StartCoroutine(Dash(movementDirection, horizontalInput, -verticalInput));
                        StartCoroutine(DashRestore());
                        _DashNow = _DashNow - 1;
                    }
                    else if (Input.GetButtonUp("JumpP1"))
                    {
                        isDashed = false;

                        _animation.PlayerDash(false);
                    }
                }          
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            //Rotate Trash
            float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP1");
            float RotverticalInput = Input.GetAxisRaw("RotVerticalP1");

            if (RothorizontalInput != 0 && RotverticalInput != 0)
            {
                //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
                angle = Mathf.Atan2(RothorizontalInput, -RotverticalInput) * Mathf.Rad2Deg;
                //angle = Mathf.Lerp(transform.rotation.y, angle, 0.5f);
                Quaternion target = Quaternion.Euler(0, angle, 0);
                ShootRot.transform.rotation = Quaternion.RotateTowards(ShootRot.transform.rotation, target, 1500f * Time.deltaTime);
                Debug.Log(angle);
            }
            else
            {
                //Debug.Log("locked Boss!");
                BossLockOn();
            }                          
        }

        if (isPlayer2)//arrows
        {
           
            if (inCC == false)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                float horizontalInput = Input.GetAxis("HorizontalP2");
                float verticalInput = Input.GetAxis("VerticalP2");

                Vector3 movementDirection = new Vector3(horizontalInput, 0, -verticalInput);

                Vector3 velocity = movementDirection * maximumSpeed;

                // apply gravity acceleration to vertical speed:
                vSpeed -= gravity * Time.deltaTime;
                velocity.y = vSpeed;
                // include vertical speed in vel
                // convert vel to displacement and Move the character:
                characterController.Move(velocity * Time.deltaTime);

                if (movementDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    Cha.transform.rotation = Quaternion.RotateTowards(Cha.transform.rotation, toRotation, rotationSpeed * 100f * Time.deltaTime);
                }
                if (movementDirection != Vector3.zero)
                {
                    _animation.PlayerWalk(true);
                }
                if (movementDirection == Vector3.zero)
                {
                    _animation.PlayerWalk(false);
                }
                //float angle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
                //Debug.Log(angle);

                //Vector3 v_movement = characterController.transform.forward * verticalInput;
                //characterController.transform.Rotate(Vector3.up * horizontalInput * (100f * Time.deltaTime));
                //characterController.Move(v_movement * maximumSpeed * Time.deltaTime);

                if (!isDashClose)
                {
                    if (Input.GetButtonDown("JumpP2") && _DashNow > 0)
                    {
                        UIcontrol.EnergyBarChange(2, _DashNow, true);
                        isDashed = true;
                        //Debug.Log("P2 Dashed");
                        StartCoroutine(Dash(movementDirection, horizontalInput, verticalInput));
                        StartCoroutine(DashRestore());
                        _animation.PlayerDodge();
                        _DashNow = _DashNow - 1;
                    }

                    else if (Input.GetButtonUp("JumpP2"))
                        isDashed = false;
                }           
                //float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP1");
                //float RotverticalInput = Input.GetAxisRaw("RotVerticalP1");
                //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
            }

            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            if (IsJoystick)
            {
                float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP2");
                float RotverticalInput = Input.GetAxisRaw("RotVerticalP2");

                if (RothorizontalInput != 0 && RotverticalInput != 0)
                {
                    //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
                    angle = Mathf.Atan2(RothorizontalInput, -RotverticalInput) * Mathf.Rad2Deg;
                    //angle = Mathf.Lerp(transform.rotation.y, angle, 0.5f);
                    Quaternion target = Quaternion.Euler(0, angle, 0);
                    ShootRot.transform.rotation = Quaternion.RotateTowards(ShootRot.transform.rotation, target, 1500f * Time.deltaTime);
                    //Debug.Log(angle);
                }
                //if (Input.GetButtonDown("LockOnP2"))
                else
                {
                    //Debug.Log("locked Boss!");
                    BossLockOn();
                }
            }
            //else
            //{
            //    //Aim
            //    Plane plane = new Plane(Vector3.up, ShootRot.transform.position);
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    float hitDist = 0;

            //    if (plane.Raycast(ray, out hitDist))
            //    {
            //        Vector3 targetPoint = ray.GetPoint(hitDist);
            //        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - ShootRot.transform.position);
            //        targetRotation.x = 0;
            //        targetRotation.z = 0;
            //        ShootRot.transform.rotation = Quaternion.Slerp(ShootRot.transform.rotation, targetRotation, 7f * Time.deltaTime);
            //    }
            //}
            //BossLockOn();


        }
    }

    IEnumerator SlowEffectTimer()
    {
        //Debug.Log(transform.name + " is Slow!");
        isSlowed = true;
        oldTempSpeed = maximumSpeed;
        maximumSpeed = maximumSpeed / 2;
        yield return new WaitForSeconds(3);
        isSlowed = false;
        maximumSpeed = oldTempSpeed;
    }

    //IEnumerator ImMobilzer(int sec)
    //{
    //    //Debug.Log(transform.name + " is ImMobilze!");
    //    isImMobilized = true;
    //    maximumSpeed = 0;
    //    yield return new WaitForSeconds(sec);
    //    isImMobilized = false;
    //    maximumSpeed = tempSpeed;
    //}
    IEnumerator Dash(Vector3 velocity, float horizontalInput, float verticalInput)
    {
        //Debug.Log("Dashed");
        float startTime = Time.time;
        velocity = velocity.normalized;
        

        if (horizontalInput == 0 && verticalInput == 0)
        {
            velocity = -transform.forward * 0.1f * maximumSpeed;
        }

        while (Time.time < startTime + dashTime)
        {
            _Collider.enabled = false;
            characterController.Move(velocity * dashSpeed * Time.deltaTime);        
            yield return null;          
        }

        while(Time.time>=startTime+dashTime)
        {
            _Collider.enabled = true;
            yield return null;
        }
    }
    IEnumerator DashRestore()
    {
        yield return new WaitForSeconds(DashCD);
        _DashNow++;
        int playerCount = 0;
        if (isPlayer1)
            playerCount = 1;
        if (isPlayer2)
            playerCount = 2;
        UIcontrol.EnergyBarChange(playerCount, _DashNow, false);
        Debug.Log("DashRestored!");
    }
    public IEnumerator GetFriendlyControl(Vector3 velocity)
    {
        if (isPlayer1)
        {
            inCC = true;
            velocity.y = 0;
            float startTime = Time.time;

            velocity = velocity - transform.position;
            velocity = velocity.normalized * 2f;

            //Debug.Log(velocity);
            //velocity = velocity.normalized;

            while (Time.time < startTime + dashTime)
            {
                characterController.Move(velocity * 30 * Time.deltaTime);

                yield return null;

                inCC = false;                
            }
            //yield return new WaitForSeconds(10);
        }
        if (isPlayer2)
        {
            inCC = true;
            velocity.y = 0;
            float startTime = Time.time;

            velocity = velocity - transform.position;
            velocity = velocity.normalized * 2f;

            //Debug.Log(velocity);
            //velocity = velocity.normalized;

            while (Time.time < startTime + dashTime)
            {
                characterController.Move(velocity * 30 * Time.deltaTime);

                yield return null;

                inCC = false;
            }
            //yield return new WaitForSeconds(10);
        }
    }

    public IEnumerator KnockUp()
    {
        inCC = true;
        isKnockUp = true;
        Debug.Log("KnockUp!");

        yield return new WaitForSeconds(3);
        inCC = false;
        isKnockUp = false;
        GetComponent<PlayerState>().StartInvincible(1);
    }

    ///Only For P1 While Getting New Cube
    public void SpeedSlow(float SpeedDec)
    {
        oldTempSpeed = maximumSpeed;
        maximumSpeed = (float)maximumSpeed * SpeedDec;
    }
    public void SpeedFast()
    {
        maximumSpeed = oldTempSpeed;
        oldTempSpeed = maximumSpeed;
        //Debug.Log(oldTempSpeed);
    }

    public void SpeedReset()
    {
        maximumSpeed = tempSpeed;
        oldTempSpeed = tempSpeed;
    }

    //demo1Used look at boss

    public void BossLockOn()//Boss will always lock on the closest player
    {
        Quaternion targetRotation = Quaternion.LookRotation(Boss.transform.position - ShootRot.transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        ShootRot.transform.rotation = Quaternion.Slerp(ShootRot.transform.rotation, targetRotation, 5 * Time.deltaTime);
    }

    
}

