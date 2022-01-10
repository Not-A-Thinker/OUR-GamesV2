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

#if UNITY_EDITOR
    private void OnValidate()
    {
        characterController = GetComponent<CharacterController>();
    }
#endif
    private void Awake()
    {
        inputActions = new JoystickControl();
        inputActions.Enable();     
    }
    private void Update()
    {
        Vector2 vector2d = inputActions.GamePlay.Move.ReadValue<Vector2>();
        Vector2 Rotvector2d = inputActions.GamePlay.Rotate.ReadValue<Vector2>();
        Vector3 vector3d = new Vector3(vector2d.x, 0, vector2d.y);

        //Simple Move
        if (vector2d != Vector2.zero)
        {       
            Vector3 direction = vector3d * moveSpeed;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            Char.transform.rotation = Quaternion.RotateTowards(Char.transform.rotation, toRotation, rotationSpeed * 5 * Time.deltaTime);
            _animation.PlayerWalk(true);

            vSpeed -= gravity * Time.deltaTime;
            direction.y = vSpeed;

            characterController.Move(direction * Time.deltaTime);      
        }
        else 
            _animation.PlayerWalk(false);

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
        if(inputActions.GamePlay.Dash.WasPressedThisFrame() && DashBar >= DashUsed)
        {
            isDashed = true;
            _animation.PlayerDash(true);
            //Debug.Log("P1 Dashed");
            StartCoroutine(Dash(vector3d));
            DashBar = DashBar - DashUsed;
        }
        else if ((inputActions.GamePlay.Dash.WasReleasedThisFrame()))
        {
            isDashed = false;
            _animation.PlayerDash(false);
        }

        //LockBoss
        if (inputActions.GamePlay.LockBoss.WasPerformedThisFrame())
        {
            Debug.Log("locked Boss!");
            BossLockOn();
        }

        //Suck And Shoot
        if (inputActions.GamePlay.Succ.WasPerformedThisFrame())
        {
            if (isPlayer1)
            {
                //GameObject.Find<ForceCast_TopDown>().gameObject;
            }
            if(isPlayer2)
            {

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

    public void BossLockOn()//Player will lock on the Best Target
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
