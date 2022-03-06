using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

public class JoyStickMovement : MonoBehaviour
{
    [SerializeField] UIcontrol UIcontrol;

    [Header("Player Components")]
    [SerializeField] JoystickControl inputActions;
    [SerializeField] CharacterController characterController;

    [SerializeField] GameObject Char;
    [SerializeField] PlayerAnimator _animation;
    [SerializeField] GameObject ShootRot;
    [SerializeField] PlayerRespawn playerRespawn;

    GameObject Boss;
    ForceCast_TopDown forceCast_TopDown;
    ForceRepel_TopDown ForceRepel_TopDown;

    [Header("Player State")]
    public bool isPlayer1;
    public bool isPlayer2;

    public bool isSlowed;
    public bool isImMobilized;
    public bool isDashed;
    public bool isShoot;
    public bool isFriendlyPush;
    public bool isKnockUp;

    public bool inCC = false;

    [Header("Player Move Settings")]
    public float moveSpeed = 10;
    public float rotationSpeed = 100;
    private float tempSpeed;
    float gravity = 9.8f;
    float vSpeed = 0f;

    [Header("Player Dash")]
    public float dashSpeed;
    public float dashTime;
    public int DashCD;

    //public int DashBar = 100;
    //public int DashUsed;
    //public float DashRestore;
     public int _DashTotal;
     int _DashNow;

    [Header("Player Vectors")]
    Vector2 vector2d = Vector2.zero;
    Vector2 Rotvector2d = Vector2.zero;
    Vector3 vector3d = Vector3.zero;

#if UNITY_EDITOR
    private void OnValidate()
    {
        characterController = GetComponent<CharacterController>();
    }
#endif
    private void Awake()
    {
        _DashNow = _DashTotal;
        //Debug.Log(_DashNow);
        tempSpeed = moveSpeed;

        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        if (isPlayer1)
            forceCast_TopDown = GetComponent<ForceCast_TopDown>();
        else
        {
            ForceRepel_TopDown = GetComponentInChildren<ForceRepel_TopDown>();
        }

        inputActions = new JoystickControl();
        inputActions.Enable();
    }


    #region OnInputTrigger
    public void OnMove(InputAction.CallbackContext context)
    {
        vector2d = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDashed = true;
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        Rotvector2d = context.ReadValue<Vector2>();
    }

    public void OnLockedBoss(InputAction.CallbackContext context)
    {
        BossLockOn();
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPlayer1)
            {
                forceCast_TopDown.Charge = true;
            }
            if (isPlayer2)
            {
                ForceRepel_TopDown.ButtonDonwEvent();
            }
        }
        if (context.performed)
        {
            //Debug.Log("ShootAcumating");
        }
        //isShoot = context.action.triggered;
        if (context.canceled)
        {
            //Debug.Log("ShootUp");
            forceCast_TopDown.isShooted = true;
        }
    }
    public void OnFriendlyHelp(InputAction.CallbackContext context)
    {
        isFriendlyPush = context.action.triggered;
    }
    public void OnRespawn(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerRespawn.Respawning = context.action.triggered;
    }
    #endregion

    #region InputActions
    //Move
    private void Move(Vector3 vector3d)
    {
        if (vector2d != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(vector3d, Vector3.up);
            Char.transform.rotation = Quaternion.RotateTowards(Char.transform.rotation, toRotation, rotationSpeed * 5 * Time.deltaTime);
            _animation.PlayerWalk(true);       
        }
        else
            _animation.PlayerWalk(false);
    }
    //Rotation   
    private void Rotate()
    {
        if (Rotvector2d != Vector2.zero)
        {
            float angle = Mathf.Atan2(Rotvector2d.x, Rotvector2d.y) * Mathf.Rad2Deg;
            //angle = Mathf.Lerp(transform.rotation.y, angle, 0.5f);
            Quaternion target = Quaternion.Euler(0, angle, 0);
            ShootRot.transform.rotation = Quaternion.RotateTowards(ShootRot.transform.rotation, target, 250f * Time.deltaTime);
            //Debug.Log(angle);
        }
        else
            BossLockOn();
    }
    //Dash
    private void DashOn()
    {
        if (isDashed && _DashNow > 0)
        {
            _animation.PlayerDash(true);
            //Debug.Log("P1 Dashed");
            StartCoroutine(Dash(vector3d));
            isDashed = false;
        }
    }
    IEnumerator Dash(Vector3 velocity)
    {
        //Debug.Log("Dashed");
        float startTime = Time.time;
        velocity = velocity.normalized;  
        int playerCount = 0;
        if (isPlayer1)
            playerCount = 1;
        if (isPlayer2)
            playerCount = 2;
        UIcontrol.EnergyBarChange(playerCount, _DashNow, true);
        _DashNow--;
        StartCoroutine(DashRestore());
        _animation.PlayerDash(false);

        if (velocity == Vector3.zero)
        {
            velocity = -transform.forward * 0.1f * tempSpeed;
        }

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(velocity * dashSpeed * Time.deltaTime);          
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
    //Shoot
    private void Shoot()
    {
        if (isShoot)
        {
            if (isPlayer1)
            {
                forceCast_TopDown.isShooted = true;
            }

            if (isPlayer2)
            {

            }
        }
    }
    private void BossLockOn()//Player will lock on the Best Target
    {
        if (GameObject.Find("Boss").GetComponent<BossAI_Wind>().isStandoMode)
        {
            Boss = GameObject.FindGameObjectWithTag("BossStando").gameObject;
        }
        else
            Boss = GameObject.Find("Boss");

        Quaternion targetRotation = Quaternion.LookRotation(Boss.transform.position - ShootRot.transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        ShootRot.transform.rotation = Quaternion.Slerp(ShootRot.transform.rotation, targetRotation, 400f * Time.deltaTime);
    }
    public IEnumerator KnockUp()
    {
        inCC = true;
        isKnockUp = true;
        Debug.Log("KnockUp!");

        yield return new WaitForSeconds(3);
        inCC = false;
    }
    ///Only For P1 While Getting New Cube
    public void SpeedSlow(float SpeedDec)
    {
        tempSpeed = tempSpeed * SpeedDec;
    }
    public void SpeedFast(float SpeedInc)
    {
        tempSpeed = tempSpeed / SpeedInc;
    }
    public void SpeedReset()
    {
        tempSpeed = moveSpeed;
    }
    #endregion
    private void Update()
    {
        if (isKnockUp)
        {
            characterController.Move(transform.up * 3 * Time.deltaTime);
            characterController.transform.rotation = new Quaternion(0, 90 * Time.deltaTime, 0, 0);
        }

        vector3d = new Vector3(vector2d.x, 0, vector2d.y);

        //if (DashBar < 100)
        //{       
        //    DashBar = (int)(DashBar + DashRestore * Time.deltaTime * 1.5);
        //    Debug.Log(DashBar);
        //    for (int i = 1; i <= _DashTotal; i++)
        //    {
        //        if (DashBar == DashUsed * i)
        //        {
        //            ///restore one Dash
        //            _DashNow++;
        //            int playerCount = 0;
        //            if (isPlayer1)
        //                playerCount = 1;
        //            if (isPlayer2)
        //                playerCount = 2;
        //            UIcontrol.EnergyBarChange(playerCount, _DashNow, false);
        //            Debug.Log("DashRestored!");
        //        }
        //    }
        //}

        if (!inCC)
        {
            DashOn();
            Move(vector3d);
            Rotate();
            //DashOn(vector3d);
            Shoot();          

            vSpeed -= gravity * Time.deltaTime;
            vector3d.y = vSpeed;

            characterController.Move(vector3d * Time.deltaTime * tempSpeed);
        }
        //Suck And Shoot
        //if (inputActions.GamePlay.Succ.WasPressedThisFrame())
        //{

        //}
        //if (inputActions.GamePlay.Succ.WasPerformedThisFrame())
        //{
        //    if (isPlayer1)  
        //    {
        //        if (!forceCast_TopDown.Shooted)
        //        {
        //            forceCast_TopDown.Accumulate();
        //        }
        //        forceCast_TopDown.FriendlyPushed();
        //    }
        //    if(isPlayer2)
        //    {

        //    }
        //}
        //if(inputActions.GamePlay.Succ.WasReleasedThisFrame())
        //{
        //    if (isPlayer1)
        //    {
        //        forceCast_TopDown.Shoot((int)forceCast_TopDown._force);
        //        forceCast_TopDown.ResetOldQue();
        //    }           
        //}
    }
}
