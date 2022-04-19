using System.Collections;
using UnityEngine;



public class Move : MonoBehaviour
{

    [SerializeField] UIcontrol UIcontrol;


    [Header("Player Move")]
    [SerializeField]
    private float maximumSpeed;//可於外部改變速度
    private float oldTempSpeed;//記錄變更前速度
    private float tempSpeed; //記錄用最大速度



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
    public bool autoLockBoss = true;

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

    float friendControlTime = 0.25f;

    float _DashNowFloat=0;
    int DashBar = 100;
    //public int DashUsed;
    public int _DashRestore;

    [Header("Player Dash V2")]
    public float dashDuration;// 控制?刺??
    public float dashSpeedV2;// ?刺速度

    private bool isDash;// 是否在?刺
    private float dashTimeV2;// 剩余?刺??
    private Vector3 directionXOZ;// ?取 xOz 平面的坦克方向
    [SerializeField] string dashButtonName;// 存??于每?玩家的???名?

    float angle;

    PlayerState playerState;

    void Start()
    {
        _DashNow = _DashTotal;
        DashBar = DashBar / _DashTotal;

        dashTimeV2 = dashDuration;

        _Collider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        rb = GetComponent<Rigidbody>();
        tempSpeed = maximumSpeed;
        oldTempSpeed = tempSpeed;
        Boss = GameObject.Find("Boss");

        playerState = GetComponent<PlayerState>();
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

        /// 閃避條充能
        if (_DashNow < _DashTotal)
        {
            int playerCount = 0;
            if (isPlayer1)
                playerCount = 1;
            if (isPlayer2)
                playerCount = 2;
            UIcontrol.EnergyBarChange(playerCount, _DashNow, true, _DashNowFloat);

            if (_DashNowFloat < DashBar)
                _DashNowFloat += _DashRestore * Time.deltaTime;
            else
            {
                _DashNow++;
                _DashNowFloat = 0;
            }
            //Debug.Log(_DashNowFloat);                   
        }

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
                    PlayerSoundEffect.PlaySound("Dog_Move");
                }

                if (movementDirection == Vector3.zero)
                {
                    _animation.PlayerWalk(false);
                    PlayerSoundEffect.PlaySound("Dog_StopMove");
                }

                if(!isDashClose)
                {
                    if (Input.GetButtonDown("JumpP1") && _DashNow > 0)
                    {
                       
                        isDashed = true;
                        StartCoroutine(DashDelay()) ;
                        //Debug.Log("P1 Dashed");
                        StartCoroutine(Dash(movementDirection, horizontalInput, -verticalInput));
                        //DashV2(movementDirection, horizontalInput, -verticalInput);
                        //StartCoroutine(DashRestore());
                        _DashNow = _DashNow - 1;
                    }
                    else if (Input.GetButtonUp("JumpP1"))
                    {
                        isDashed = false;
                    }
                }          
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
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
                        //UIcontrol.EnergyBarChange(2, _DashNow, true);
                        isDashed = true;
                        //Debug.Log("P2 Dashed");
                        StartCoroutine(DashDelay());
                        StartCoroutine(Dash(movementDirection, horizontalInput, verticalInput));
                        //DashV2(movementDirection, horizontalInput, -verticalInput);
                        //StartCoroutine(DashRestore());
                        _animation.PlayerDodge();
                        _DashNow = _DashNow - 1;
                    }

                    else if (Input.GetButtonUp("JumpP2"))
                    {
                        isDashed = false;
                    }
                        
                }           
                //float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP1");
                //float RotverticalInput = Input.GetAxisRaw("RotVerticalP1");
                //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
            }

            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
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

    private void FixedUpdate()
    {
        if (isPlayer1)
        {

            //Rotate Trash
            float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP1");
            float RotverticalInput = Input.GetAxisRaw("RotVerticalP1");

            if (Input.GetButton("LockOnP1"))
            {
                autoLockBoss = !autoLockBoss;
                if (autoLockBoss)
                    UIcontrol.flyText(1, Color.red, "Boss Locked!!");
                else
                    UIcontrol.flyText(1, Color.red, "Boss UnLocked!!");
            }

            if (autoLockBoss)
            { BossLockOn();}
            else
            {
                if (RothorizontalInput != 0 && RotverticalInput != 0)
                {
                    //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
                    angle = Mathf.Atan2(RothorizontalInput, -RotverticalInput) * Mathf.Rad2Deg;
                    //angle = Mathf.Lerp(transform.rotation.y, angle, 0.5f);
                    Quaternion target = Quaternion.Euler(0, angle, 0);
                    ShootRot.transform.rotation = Quaternion.RotateTowards(ShootRot.transform.rotation, target, 800f * Time.fixedDeltaTime);
                    //Debug.Log(angle);
                }
            }
        }

        if (isPlayer2)
        {


            if (IsJoystick)
            {
                float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP2");
                float RotverticalInput = Input.GetAxisRaw("RotVerticalP2");
                if (Input.GetButton("LockOnP2"))
                {
                    autoLockBoss = !autoLockBoss;
                    if (autoLockBoss)
                        UIcontrol.flyText(2, Color.red, "Boss Locked!!");
                    else
                        UIcontrol.flyText(2, Color.red, "Boss UnLocked!!");
                }

                if (autoLockBoss)
                {
                    BossLockOn();
                }
                else
                {
                    if (RothorizontalInput != 0 && RotverticalInput != 0)
                    {
                        //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
                        angle = Mathf.Atan2(RothorizontalInput, -RotverticalInput) * Mathf.Rad2Deg;
                        //angle = Mathf.Lerp(transform.rotation.y, angle, 0.5f);
                        Quaternion target = Quaternion.Euler(0, angle, 0);
                        ShootRot.transform.rotation = Quaternion.RotateTowards(ShootRot.transform.rotation, target, 800f * Time.fixedDeltaTime);
                        //Debug.Log(angle);
                    }
                }
                //if (Input.GetButtonDown("LockOnP2"))
                //else
                //{
                //    //Debug.Log("locked Boss!");

                //}
            }
        }
    }

    IEnumerator DashDelay()
    {
        playerState.DashColorChange(true);
        yield return new WaitForSeconds(0.05f);  
        StartCoroutine(_animation.PlayerDash(dashTime));
        yield return new WaitForSeconds(0.1f);
        playerState._renderer.enabled = false;
        yield return new WaitForSeconds(0.5f);
        playerState.DashColorChange(false);
        playerState._renderer.enabled = true;
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
            //_Collider.enabled = false;
            characterController.Move(velocity * dashSpeed * Time.deltaTime);
            yield return null;
        }

        while(Time.time>=startTime+dashTime)
        {
            //_Collider.enabled = true;
            //playerState.DashColorChange(false);
            yield return null;
        }
    }
    
    void DashV2(Vector3 velocity, float horizontalInput, float verticalInput)
    {
        

        if (!isDash)
        {
            if (Input.GetButton(dashButtonName))
            {
                isDash = true;

                velocity = velocity.normalized;

                if (horizontalInput == 0 && verticalInput == 0)
                {
                    velocity = -transform.forward * 0.1f * maximumSpeed;
                }

                //directionXOZ = transform.forward;// forward 指向物体?前的前方
                //directionXOZ.y = 0f;// 只做平面的上下移?和水平移?，不做高度上的上下移?
                directionXOZ = velocity;
                Debug.Log(dashButtonName + "Pressed.");
            }
        }
        else
        {
            if (dashTimeV2 <= 0)// reset
            {
                isDash = false;

                dashTimeV2 = dashDuration;
            }
            else
            {
                dashTimeV2 -= Time.deltaTime;
                rb.velocity = directionXOZ * dashTimeV2 * dashSpeedV2;// rigidbody = GetComponent<Rigidbody>(); 加在 Start() 函?中
            }
        }
    }

    //IEnumerator DashRestore()
    //{
    //    float time = Time.time;
    //    Debug.Log(time);
    //    yield return new WaitForSeconds(DashCD);
    //    _DashNow++;
    //    int playerCount = 0;
    //    if (isPlayer1)
    //        playerCount = 1;
    //    if (isPlayer2)
    //        playerCount = 2;
    //    UIcontrol.EnergyBarChange(playerCount, _DashNow, false);
    //    Debug.Log("DashRestored!");
    //}
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

            while (Time.time < startTime + friendControlTime)
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

            while (Time.time < startTime + friendControlTime)
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
        playerState.StartInvincible(1.5f);
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
    }

    public void SpeedReset()
    {
        maximumSpeed = tempSpeed;
        oldTempSpeed = tempSpeed;
    }

    public void CubeSpeedDown(int cubeCount,float speedToDecrease)
    {
        float speed = tempSpeed;

        for(int i=0;i<cubeCount;i++)
        {
            speed = speed * speedToDecrease;
            //Debug.Log(speed);
        }
       maximumSpeed = speed;
       oldTempSpeed = maximumSpeed;
    }

    //demo1Used look at boss

    public void BossLockOn()//Boss will always lock on the closest player
    {
        Quaternion targetRotation = Quaternion.LookRotation(Boss.transform.position - ShootRot.transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        ShootRot.transform.rotation = Quaternion.Slerp(ShootRot.transform.rotation, targetRotation, 5 * Time.fixedDeltaTime);
    }  
}

