using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRepel_TopDown : MonoBehaviour
{

    [SerializeField] GameObject ChaRot;

    [Header("Suck Force")]
    public float _force = 5f;
    float _OldForce = 5f;
    public float _range = 15f;

    [Header("Suck OBJ")]
    [SerializeField] GameObject savedObject;
    //[SerializeField] GameObject S_Tonado;
    //public GameObject CubeCount;
    //int CubeConter=0;
    [SerializeField] GameObject Range;
    [SerializeField] GameObject clipParent;
    [SerializeField] Move move;
    [SerializeField] GameObject Mother;
    //[SerializeField] private Renderer Renderer;
    [SerializeField] ParticleSystem _Suck_Effect;
    //[SerializeField] private AnimationCurve curve;
    UIcontrol uIcontrol;
    bool IsAiming;

    [Header("State")]
    [SerializeField] bool FriendCD = false;
    BossSpawnObject BossSpwO;
    bool TextSpawning;

    Quaternion OldQuate;

    [Header("P2 Attack CD")]
    //public int SuccMaxCD = 1;
    //float Timer = 0;
    public bool SuckInCD;
    public float SuckCount;
    int SuckTotal;
    public float _SpeedSlow;
    bool onSucking;

    public bool isAttackWithAim;
    //clip

    private void Start()
    {
        SuckTotal = (int)SuckCount;
        uIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        BossSpwO = GameObject.Find("Boss").GetComponent<BossSpawnObject>();
        _OldForce = _force;
    }

    void Update()
    {
        if(!onSucking)
        {
            if (SuckCount < SuckTotal)
            {
                SuckCount += 1 * Time.deltaTime;
                if (SuckCount >= 3)
                {
                    SuckCount = 3;
                    if (SuckInCD)
                        SuckInCD = false;
                }                   
            }            
        }
        uIcontrol.SuckingCDBar(SuckCount);

        //if (SuckInCD)
        //{
        //    if (Timer < SuckCount)
        //    {
        //        Timer += Time.deltaTime;
        //    }
        //    else
        //    {
        //        SuckInCD = false;             
        //    }
        //    Timer = SuckCount;
        //}
        //else
        //    Timer = SuckCount;  
        //uIcontrol.SuckCount(SuckCount);
        
        if (isAttackWithAim)
            AttackWithAim();
        else
            AttackWithOutAim();

        if (savedObject)
        {       
            Vector3 NowPos = Vector3.Lerp(savedObject.transform.position, transform.position, 0.2f);
            Vector3 toTarget = transform.position - savedObject.transform.position;
            savedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            savedObject.transform.position = Vector3.MoveTowards(savedObject.transform.position, transform.position, _force * Time.deltaTime);
            _force = _force + _force* 5 *Time.deltaTime;
            // curve.Evaluate(0.6f)
        }
    }
    private void AttackWithAim()
    {
        if (Input.GetButtonDown("AimP2"))
        {
            ButtonDonwEvent();
        }

        if (Input.GetButton("AimP2"))
        {
            Range.SetActive(true);
            IsAiming = true;
        }

        if (IsAiming)
        {
            if (Input.GetButton("HelpFriendP2"))
                SuckFriend();

            if (Input.GetButtonDown("Fire2"))
            {
                if (!SuckInCD)
                {
                    onSucking = true;
                }
                else
                    uIcontrol.flyText(2, Color.red, "CD!!!!");
            }
            if (Input.GetButton("Fire2"))
            {
                if (!SuckInCD)
                {
                    GetComponent<BoxCollider>().isTrigger = true;
                    Repel();
                }
            }
            if (Input.GetButtonUp("Fire2"))
            {
                if (savedObject != null)
                {
                    resetObject();
                    GetComponent<BoxCollider>().isTrigger = false;
                }
                onSucking = false;
            }
        }

        if (Input.GetButtonUp("AimP2"))
        {
            onSucking = false;
            //Renderer.material.color = Color.white;
            TextSpawning = false;
            ChaRot.transform.rotation = OldQuate;
            OldQuate = new Quaternion(0, 0, 0, 0);
            Range.SetActive(false);
            move.SpeedReset();
            move.isDashClose = false;
            IsAiming = false;

            //uIcontrol.SuckingCDBar(canSucc);
            //if (!SuckInCD)
            //{
            //    SuckInCD = true;
            //    Timer = 0;              
            //}              

            //if(S_Tonado!=null)
            //{
            //    S_Tonado.transform.GetComponent<Skill_TornadoAttack_SForm>().CanMove = true;
            //    S_Tonado = null;
            //}          
        }
    }

    private void AttackWithOutAim()
    {
        if(Input.GetButton("HelpFriendP2"))
             SuckFriend();

        if (Input.GetButtonDown("Fire2"))
        {
            if (!SuckInCD)
            {
                ButtonDonwEvent();
                onSucking = true;
            }
            else
                uIcontrol.flyText(2, Color.red, "CD!!!!");
        }
        if (Input.GetButton("Fire2"))
        {
            if (!SuckInCD)
            {
                Range.SetActive(true);
                GetComponent<BoxCollider>().isTrigger = true;
                Repel();
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if (savedObject != null)
            {
                resetObject();
                GetComponent<BoxCollider>().isTrigger = false;
            }
            onSucking = false;
            //Renderer.material.color = Color.white;
            TextSpawning = false;
            ChaRot.transform.rotation = OldQuate;
            OldQuate = new Quaternion(0, 0, 0, 0);
            Range.SetActive(false);
            move.SpeedReset();
            move.isDashClose = false;
            IsAiming = false;
        }
    }
    public void ButtonDonwEvent()
    {
        OldQuate = ChaRot.transform.rotation;
        move.SpeedSlow(_SpeedSlow);
        move.isDashClose = true;
        
        //uIcontrol.SuckingCDBar(false);
    }
    public void Repel()
    {
        ChaRot.transform.rotation = Quaternion.Slerp(ChaRot.transform.rotation, transform.parent.transform.rotation , 15f * Time.deltaTime);

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.forward;
        //Debug.DrawRay(startPos, endPos);
        
        RaycastHit hit;
    
        if (Physics.Raycast(startPos, endPos, out hit, _range))
        {
            //Debug.Log(hit.transform.name + "." + hit.transform.tag);
            if (hit.transform.gameObject.layer == 6 &&  SuckCount <= 3 && (int)SuckCount > 0 && !SuckInCD)
            {
                //Debug.Log(hit.transform.tag);
                if (hit.transform.tag != "Boss" && hit.transform.tag != "Player")
                {
                    //if (hit.transform.name.Contains("Tornado SForm"))
                    //{
                    //    if (_Suck_Effect != null)
                    //        _Suck_Effect.Play();
                    //    soundEffect.OnAttackPlay();
                    //    //Renderer.material.color = Color.green;
                    //    hit.transform.GetComponent<Skill_TornadoAttack_SForm>().CanMove = false;
                    //    S_Tonado = hit.transform.gameObject;
                    //}
                    //else
                    //{
                        if (_Suck_Effect != null)
                            _Suck_Effect.Play();
                        //soundEffect.OnAttackPlay();
                        //Renderer.material.color = Color.green;
                        savedObject = hit.transform.gameObject;
                    //}
                }
                //rb.useGravity = !rb.useGravity;       
               else if (hit.transform.tag == "Boss")
                {
                    if (_Suck_Effect != null)
                        _Suck_Effect.Play();

                    //Renderer.material.color = Color.green;
                    if (BossSpwO.SpawnedCount < BossSpwO.SpawnendMax)
                    {

                        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        //Debug.Log(hitpoint);
                        BossSpwO.ObjectSpawn(hit.point, spawnRotation);
                        if (BossSpwO.lastSpawned != null)
                        {
                            //soundEffect.OnAttackPlay();
                            savedObject = BossSpwO.lastSpawned;
                            savedObject.GetComponent<ObjectDestroy>().isSucked = true;
                            savedObject.GetComponent<Bullet>().bossToSuck = true;
                            BossSpwO.lastSpawned = null;
                            //Rigidbody rb = savedObject.GetComponent<Rigidbody>();
                            //rb.useGravity = false;
                        }
                    }
                    else
                    {
                        if(!TextSpawning)
                        {
                            uIcontrol.flyText(2, Color.red, "Full!!!!");
                            TextSpawning = true;
                        }                      
                    }                    
                }
                else if (hit.transform.tag == "Objcet")
                {
                    //Renderer.material.color = Color.green;
                }
                //    hit.transform.rotation = new Quaternion(0, 0, 0, 0);
                //    hit.transform.position = Vector3.MoveTowards(hit.transform.position, transform.position, curve.Evaluate(0.15f));                

                else
                {
                    //Renderer.material.color = Color.white;
                }
            }
            if (SuckCount <= 0)
            {
                onSucking = false;
                //Renderer.material.color = Color.white;
                //soundEffect.OnResetSound();
                SuckInCD = true;
                GetComponent<BoxCollider>().isTrigger = false;
                //Range.SetActive(false);
                //Timer = 0;
            }

        }
    }
    public void SuckFriend()
    {
        ChaRot.transform.rotation = Quaternion.Slerp(ChaRot.transform.rotation, transform.parent.transform.rotation, 15f * Time.deltaTime);

        Range.SetActive(true);

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.forward;
        //Debug.DrawRay(startPos, endPos);

        RaycastHit hit;

        if (Physics.Raycast(startPos, endPos, out hit, _range))
        {
            //Debug.Log("Hit" + hit.transform.name);
            if (hit.transform.tag == "Player" && FriendCD == false && hit.transform.name != Mother.name)
            {
                //Debug.Log("Hit" + hit.transform.name);
                Move move = hit.transform.GetComponent<Move>();
                StartCoroutine(move.GetFriendlyControl(this.transform.position));
                StartCoroutine(FriendlyCD());
                Range.SetActive(false);
                //Vector3 NowPos = Vector3.Lerp(hit.transform.position, transform.position, 0.2f);
                //Vector3 toTarget = this.transform.position - hit.transform.position;      
                //hit.transform.Translate(toTarget * _force * Time.deltaTime);
            }
        }
    }

    public void resetObject()
    {
        if (savedObject==true)
        {
            if(savedObject.transform.gameObject.layer != 6)
            {
                savedObject.GetComponent<Rigidbody>().useGravity = true;
            }    

            if (savedObject.GetComponent<ObjectDestroy>())
            {
                savedObject.GetComponent<ObjectDestroy>().isSucked = false;
            }         
            savedObject = null;
            _force = _OldForce;
        }
           
        //if (savedObject.tag=="Object")
        //{
        //    savedObject.AddComponent<ObjectDestroy>();
        //}    
           //savedObject.transform.parent = null;
           //savedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;                       
    }   
    //void ObjectTransform(GameObject obj)
    //{
    //    direction = savedObject.transform.position - transform.position;
    //    direction.Normalize();
        
    //    rb.AddForceAtPosition(transform.forward * _force * -1 * Time.deltaTime, transform.position, ForceMode.Impulse);
    //}
    IEnumerator FriendlyCD()
    {
        FriendCD = true;
     
        yield return new WaitForSeconds(4);

        FriendCD = false;
    }

}
