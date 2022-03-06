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
    [SerializeField] GameObject S_Tonado;
    //public GameObject CubeCount;
    //int CubeConter=0;
    [SerializeField] GameObject Range;
    [SerializeField] GameObject clipParent;
    [SerializeField] Move move;
    [SerializeField] GameObject Mother;
    [SerializeField] private Renderer Renderer;
    //[SerializeField] private AnimationCurve curve;
    UIcontrol uIcontrol;

    [Header("State")]
    [SerializeField] bool FriendCD = false;
    BossSpawnObject BossSpwO;
    bool TextSpawning;

    Quaternion OldQuate;

    [Header("P2 Attack CD")]
    public int SuccMaxCD = 1;
    [SerializeField] float Timer = 1;
    public bool SuckInCD;
    public int SuckCount;
    public float _SpeedSlow;

    //clip

    private void Start()
    {
        uIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        BossSpwO = GameObject.Find("Boss").GetComponent<BossSpawnObject>();
        _OldForce = _force;
    }

    void Update()
    {
        if (SuckInCD)
        {
            if (Timer < SuccMaxCD)
            {
                Timer += Time.deltaTime;
            }
            else
            {
                SuckInCD = false;
                Timer = SuccMaxCD;
                SuckCount = 0;
            }
        }
        else
            Timer = SuccMaxCD;

        uIcontrol.SuckingCDBar(Timer/SuccMaxCD);

        //uIcontrol.SuckCount(SuckCount);
        ///open it while using move
        //if (Input.GetButton("HelpFriendP2"))        
        //    SuckFriend();

        //if (Input.GetButtonDown("Fire2"))
        //    ButtonDonwEvent();

        //if (Input.GetButton("Fire2"))
        //    ButtonHoldEvent();

        //if (Input.GetButtonUp("Fire2"))       
        //    ButtonUpEvent();
        
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

    public void ButtonDonwEvent()
    {
        if (!SuckInCD)
        {
            OldQuate = ChaRot.transform.rotation;
            move.SpeedSlow(_SpeedSlow);
        }
        else
            uIcontrol.flyText(2, Color.red, "Sucking CD!");
       
        //uIcontrol.SuckingCDBar(false);
    }
    public void ButtonHoldEvent()
    {
        if (!SuckInCD)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            Range.SetActive(true);
            Repel();
        }
    }
    public void ButtonUpEvent()
    {
        Renderer.material.color = Color.red;
        TextSpawning = false;
        ChaRot.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        GetComponent<BoxCollider>().isTrigger = false;
        Range.SetActive(false);
        move.SpeedReset();

        //uIcontrol.SuckingCDBar(canSucc);
        if (!SuckInCD)
        {
            SuckInCD = true;
            Timer = 0;
        }
        if (savedObject != null)
        {
            resetObject();
        }
        if (S_Tonado != null)
        {
            S_Tonado.transform.GetComponent<Skill_TornadoAttack_SForm>().CanMove = true;
            S_Tonado = null;
        }
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
            if (hit.transform.gameObject.layer == 6 && SuckCount < 3 && !SuckInCD)
            {
                //Debug.Log(hit.transform.tag);
                if (hit.transform.tag != "Boss" && hit.transform.tag != "Player")
                {
                    if (hit.transform.name.Contains("Tornado SForm"))
                    {
                        Renderer.material.color = Color.green;
                        hit.transform.GetComponent<Skill_TornadoAttack_SForm>().CanMove = false;
                        S_Tonado = hit.transform.gameObject;
                    }
                    else
                    {
                        Renderer.material.color = Color.green;
                        savedObject = hit.transform.gameObject;
                    }
                }
                //rb.useGravity = !rb.useGravity;       
               else if (hit.transform.tag == "Boss")
                {          
                    Renderer.material.color = Color.green;
                    if (BossSpwO.SpawnedCount <= BossSpwO.SpawnendMax)
                    {
                        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        //Debug.Log(hitpoint);
                        BossSpwO.ObjectSpawn(hit.point, spawnRotation);
                        if (BossSpwO.lastSpawned != null)
                        {
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
                            uIcontrol.flyText(2, Color.red, "Full!");
                            TextSpawning = true;
                        }                      
                    }                    
                }
                else if (hit.transform.tag == "Objcet")
                {
                    Renderer.material.color = Color.green;
                }
                //    hit.transform.rotation = new Quaternion(0, 0, 0, 0);
                //    hit.transform.position = Vector3.MoveTowards(hit.transform.position, transform.position, curve.Evaluate(0.15f));                

                else
                {
                    Renderer.material.color = Color.red;
                }
            }
            if (SuckCount == 3)
            {
                SuckInCD = true;
                Timer = 0;
            }

        }
    }
    public void SuckFriend()
    {
        ChaRot.transform.rotation = Quaternion.Slerp(ChaRot.transform.rotation, transform.parent.transform.rotation, 15f * Time.deltaTime);

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.forward;
        //Debug.DrawRay(startPos, endPos);

        RaycastHit hit;
        if (Physics.Raycast(startPos, endPos, out hit, _range))
        {
            //Debug.Log("Hit" + hit.transform.name);
            if (hit.transform.tag == "Player" && FriendCD == false && hit.transform.name != Mother.name)
            {
                //Debug.Log("Hit"+hit.transform.name);
                Move move = hit.transform.GetComponent<Move>();
                StartCoroutine(move.GetFriendlyControl(this.transform.position));
                StartCoroutine(FriendlyCD());
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
     
        yield return new WaitForSeconds(8);

        FriendCD = false;
    }

}
