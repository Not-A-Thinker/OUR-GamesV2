using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Move : MonoBehaviour
{
   
    [SerializeField] UIcontrol UIcontrol;


    [Header("Player Move")]
    [SerializeField]
    private float maximumSpeed;
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

    public bool isSlowed;
    public bool isImMobilized;
    public bool isDashed;

    public bool inCC = false;

    //demo1 used
    private GameObject Boss;

    float gravity = 9.8f;
    float vSpeed = 0f;

    [Header("Player Dash")]
    //dash
    public float dashSpeed;
    public float dashTime;

    public float DashBar=100f;
    public float DashUsed ;
    public float DashRestore;

    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        tempSpeed = maximumSpeed;
        Boss = GameObject.Find("Boss");
        
    }


    void Update()
    {
        
        if (characterController.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
        }

        if (DashBar < 100)
        {
            DashBar = DashBar + DashRestore * Time.deltaTime;
        }
        if (isPlayer1)//wasd
        {

            if(inCC==false)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                //Move
                float horizontalInput = Input.GetAxis("HorizontalP1");
                float verticalInput = Input.GetAxis("VerticalP1");

                //if (horizontalInput < 0.3f && horizontalInput > -0.3f)
                //    horizontalInput = 0;

                //if (verticalInput < 0.3f && verticalInput > -0.3f)
                //    verticalInput = 0;

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
                }

                if (movementDirection != Vector3.zero)
                {
                    _animation.PlayerWalk(true);
                }
                if (movementDirection == Vector3.zero)
                {
                    _animation.PlayerWalk(false);
                }

                if (Input.GetButtonDown("JumpP1") && DashBar >= DashUsed)
                {
                    isDashed = true;
                    _animation.PlayerDash(true);
                    //Debug.Log("Dashed");
                    StartCoroutine(Dash(movementDirection, horizontalInput, -verticalInput));
                    DashBar = DashBar - DashUsed;
                }
                else if (Input.GetButtonUp("JumpP1"))
                {
                    isDashed = false;
                    _animation.PlayerDash(false);
                }
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            //Rotate Trash
            float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP1");
            float RotverticalInput = Input.GetAxisRaw("RotVerticalP1");
            Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);

            //if (RothorizontalInput < 0.3f && RothorizontalInput > -0.3f)
            //    RothorizontalInput = 0;
            //if (RotverticalInput < 0.3f && RotverticalInput > -0.3f)
            //    RotverticalInput = 0;

            //Vector2 angle = new Vector2(RothorizontalInput, RotverticalInput);

            //if (angle != Vector2.zero)
            //{
            //    characterController.transform.Rotate(transform.up * angle * (rotationSpeed * Time.deltaTime));
            //}

            //2
            //Vector2 angle = new Vector2(RothorizontalInput, RotverticalInput);


            //if (angle != Vector2.zero)
            //{
            //    Quaternion toRotation = Quaternion.LookRotation(Vector3.up, angle);
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //}

            //3
            //Vector3 relative = transform.InverseTransformPoint(target.position);
            float angle = Mathf.Atan2(RothorizontalInput, -RotverticalInput) * Mathf.Rad2Deg;
            Debug.Log(angle);

            ShootRot.transform.rotation = new Quaternion(0, angle, 0, 90);

          

            //BossLockOn();

            ////Aim
            //Plane plane = new Plane(Vector3.up, transform.position);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //float hitDist = 0;

            //if (plane.Raycast(ray, out hitDist))
            //{
            //    Vector3 targetPoint = ray.GetPoint(hitDist);
            //    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            //    targetRotation.x = 0;
            //    targetRotation.z = 0;
            //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
            //}
        }

        if (isPlayer2)//arrows
        {
            if(inCC==false)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                float horizontalInput = Input.GetAxis("HorizontalP2");
                float verticalInput = Input.GetAxis("VerticalP2");

                Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

                Vector3 velocity = movementDirection * maximumSpeed;

                // apply gravity acceleration to vertical speed:
                vSpeed -= gravity * Time.deltaTime;
                velocity.y = vSpeed;
                // include vertical speed in vel
                // convert vel to displacement and Move the character:
                characterController.Move(velocity * Time.deltaTime);

               if(movementDirection!=Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    Cha.transform.rotation = Quaternion.RotateTowards(Cha.transform.rotation, toRotation, rotationSpeed*100f * Time.deltaTime);
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

                //else
                //{

                //}
                //Vector3 v_movement = characterController.transform.forward * verticalInput;
                //characterController.transform.Rotate(Vector3.up * horizontalInput * (100f * Time.deltaTime));
                //characterController.Move(v_movement * maximumSpeed * Time.deltaTime);

                if (Input.GetButtonDown("JumpP2") && DashBar >= DashUsed)
                {
                    isDashed = true;
                    //Debug.Log("Dashed");
                    StartCoroutine(Dash(movementDirection, horizontalInput, verticalInput));
                    DashBar = DashBar - DashUsed;
                }

                else if (Input.GetButtonUp("JumpP2"))
                    isDashed = false;

                //float RothorizontalInput = Input.GetAxisRaw("RotHorizontalP1");
                //float RotverticalInput = Input.GetAxisRaw("RotVerticalP1");
                //Debug.Log(RothorizontalInput.ToString("0.00000") + "+" + RotverticalInput);
            }

            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            //BossLockOn();

            //Aim
            Plane plane = new Plane(Vector3.up, ShootRot.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0;

            if (plane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - ShootRot.transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                ShootRot.transform.rotation = Quaternion.Slerp(ShootRot.transform.rotation, targetRotation, 7f * Time.deltaTime);
            }
        }
    }

    IEnumerator SlowEffectTimer()
    {
        //Debug.Log(transform.name + " is Slow!");
        isSlowed = true;
        maximumSpeed = maximumSpeed / 2;

        yield return new WaitForSeconds(3);
        isSlowed = false;
        maximumSpeed = tempSpeed;
    }

    IEnumerator ImMobilzer(int sec)
    {
        //Debug.Log(transform.name + " is ImMobilze!");
        isImMobilized = true;
        maximumSpeed = 0;
        yield return new WaitForSeconds(sec);
        isImMobilized = false;
        maximumSpeed = tempSpeed;
    }
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
            characterController.Move(velocity * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator GetFriendlyControl(Vector3 velocity)
    {
        if (isPlayer1)
        {      
            inCC = true;
            velocity.y = 0;
            float startTime = Time.time;

            velocity = velocity - this.transform.position;
            velocity = velocity.normalized * 2f;
      
            Debug.Log(velocity);
            //velocity = velocity.normalized;

            while (Time.time < startTime + dashTime)
            {
                characterController.Move(velocity * 30 *Time.deltaTime);

                yield return null;

                if (GetComponent<PlayerState>().isDead == false)
                {
                    inCC = false;
                }
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

            Debug.Log(velocity);
            //velocity = velocity.normalized;

            while (Time.time < startTime + dashTime)
            {
                characterController.Move(velocity * 30 * Time.deltaTime);

                yield return null;

                if(GetComponent<PlayerState>().isDead==false)
                {
                    inCC = false;
                }            
            }
            //yield return new WaitForSeconds(10);
        }
    }

    public IEnumerator KnockUp()
    {
        inCC = true;
        characterController.Move(new Vector3(0, 5, 0) * Time.deltaTime);
        Debug.Log("KnockUp!");

        yield return null;
        inCC = false;
    }
    public void SpeedSlow()
    {
        maximumSpeed = (float)maximumSpeed * 0.85f;
    }
    public void SpeedFast()
    {
        maximumSpeed = (float)maximumSpeed / 0.85f;
    }

    public void SpeedReset()
    {
        maximumSpeed = tempSpeed;
    }

    //demo1Used look at boss

    public void BossLockOn()//Boss will always lock on the closest player
    {
        Quaternion targetRotation = Quaternion.LookRotation(Boss.transform.position - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
    }
}
