using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

public class JoyStickMovement : MonoBehaviour
{

    [Header("Player Components")]
    [SerializeField] JoystickControl inputActions;
    [SerializeField] CharacterController characterController;

    [SerializeField] GameObject Char;
    [SerializeField] PlayerAnimator _animation;
    [SerializeField] GameObject ShootRot;
    GameObject Boss;
    ForceCast_TopDown forceCast_TopDown;
    ForceRepel_TopDown ForceRepel_TopDown;

    [Header("Player State")]
    public bool isPlayer1;
    public bool isPlayer2;

    public bool isSlowed;
    public bool isImMobilized;
    public bool isDashed;

    public bool inCC = false;

    [Header("Player Move Settings")]
    public float moveSpeed = 10;
    public float rotationSpeed = 100;

    float gravity = 9.8f;
    float vSpeed = 0f;

    [Header("Player Dash")]
    public float dashSpeed;
    public float dashTime;

    public float DashBar = 100f;
    public float DashUsed;
    public float DashRestore;

    [Header("Player Vectors")]
    Vector2 vector2d = Vector2.zero;
    Vector2 Rotvector2d = Vector2.zero;

#if UNITY_EDITOR
    private void OnValidate()
    {
        characterController = GetComponent<CharacterController>();
    }
#endif
    private void Awake()
    {
        if (isPlayer1)
            forceCast_TopDown = GetComponent<ForceCast_TopDown>();
        else
        {

        }

        inputActions = new JoystickControl();
        inputActions.Enable();     
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        isDashed = context.action.triggered;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        vector2d = context.ReadValue<Vector2>();
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

    }
    public void OnFriendlyHelp(InputAction.CallbackContext context)
    {

    }

    private void Move(Vector3 vector3d)
    {
        if (vector2d != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(vector3d, Vector3.up);
            Char.transform.rotation = Quaternion.RotateTowards(Char.transform.rotation, toRotation, rotationSpeed * 5 * Time.deltaTime);
            _animation.PlayerWalk(true);

            vSpeed -= gravity * Time.deltaTime;
            vector3d.y = vSpeed;

            characterController.Move(vector3d * Time.deltaTime * moveSpeed);
        }
        else
            _animation.PlayerWalk(false);
    }

    private void Update()
    {
        Vector3 vector3d = new Vector3(vector2d.x, 0, vector2d.y);
        //Rotation
        if(Rotvector2d!=Vector2.zero)
        {
            float angle = Mathf.Atan2(Rotvector2d.x, Rotvector2d.y) * Mathf.Rad2Deg;
            //angle = Mathf.Lerp(transform.rotation.y, angle, 0.5f);
            Quaternion target = Quaternion.Euler(0, angle, 0);
            ShootRot.transform.rotation = Quaternion.RotateTowards(ShootRot.transform.rotation, target, 250f * Time.deltaTime);
            //Debug.Log(angle);
        }

        //Dash
        if(isDashed && DashBar >= DashUsed)
        {
            _animation.PlayerDash(true);
            //Debug.Log("P1 Dashed");
            StartCoroutine(Dash(vector3d));
            DashBar = DashBar - DashUsed;
        }
        else if (!isDashed)
        {    
            _animation.PlayerDash(false);
        }

        //Suck And Shoot
        if (inputActions.GamePlay.Succ.WasPressedThisFrame())
        {
            if (isPlayer1)
            {
                forceCast_TopDown.SetOldQue();
            }

            if (isPlayer2)
            {

            }
        }
        if (inputActions.GamePlay.Succ.WasPerformedThisFrame())
        {
            if (isPlayer1)  
            {
                if (!forceCast_TopDown.Shooted)
                {
                    forceCast_TopDown.Accumulate();
                }
                forceCast_TopDown.FriendlyPushed();
            }
            if(isPlayer2)
            {

            }
        }
        if(inputActions.GamePlay.Succ.WasReleasedThisFrame())
        {
            if (isPlayer1)
            {
                forceCast_TopDown.Shoot((int)forceCast_TopDown._force);
                forceCast_TopDown.ResetOldQue();
            }           
        }
    }

    IEnumerator Dash(Vector3 velocity)
    {
        //Debug.Log("Dashed");
        float startTime = Time.time;
        velocity = velocity.normalized;

        if (velocity==Vector3.zero)
        {
            velocity = -transform.forward * 0.1f * moveSpeed;
        }

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(velocity * dashSpeed * Time.deltaTime);
            yield return null;
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
}
