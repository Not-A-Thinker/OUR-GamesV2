using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRepel_TopDown : MonoBehaviour
{

    [SerializeField] GameObject ChaRot;

    [Header("Suck Force")]
    public float _force = 5f;
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
    [SerializeField] private AnimationCurve curve;
    UIcontrol uIcontrol;

    [Header("State")]
    [SerializeField] bool SuccFromBoss = false;
    [SerializeField] bool canSucc = true;
    [SerializeField] bool FriendCD = false;
    Quaternion OldQuate;
    public int SuckCount;

    //clip

    private void Start()
    {
        uIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            ButtonDonwEvent();
        }         
        if (Input.GetButton("Fire2"))
        {
            GetComponent<BoxCollider>().isTrigger = true;
            Range.SetActive(true);
            Repel();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            ChaRot.transform.rotation = OldQuate;
            OldQuate = new Quaternion(0,0,0,0);
            GetComponent<BoxCollider>().isTrigger = false;
            SuccFromBoss = false;
            Range.SetActive(false);
            move.inCC = false;        
            canSucc = true;
            uIcontrol.SuckingCDBar(canSucc);

            if (savedObject != null)
            {
                resetObject();  
            }    
            if( S_Tonado!=null)
            {
                S_Tonado.transform.GetComponent<Skill_TornadoAttack_SForm>().CanMove = true;
                S_Tonado = null;
            }
        }
        if (savedObject)
        {
            savedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            savedObject.transform.position = Vector3.Lerp(savedObject.transform.position, transform.position, curve.Evaluate(0.25f));
        }
    }

    public void ButtonDonwEvent()
    {
        OldQuate = ChaRot.transform.rotation;
        move.inCC = true;
        uIcontrol.SuckingCDBar(false);
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
            if (hit.transform.gameObject.layer == 6)
            {
                if (savedObject == null)
                {
                    if(hit.transform.tag != "Boss" && hit.transform.tag != "Player")
                    {
                        if (hit.transform.name.Contains("Tornado SForm"))
                        {
                            hit.transform.GetComponent<Skill_TornadoAttack_SForm>().CanMove = false;
                            S_Tonado = hit.transform.gameObject;
                        }
                        savedObject = hit.transform.gameObject;
                    }                                     
                    //rb.useGravity = !rb.useGravity;       
                }
                if (hit.transform.tag == "Boss" && SuccFromBoss == false)
                {
                    BossSpawnObject BossSpwO = hit.transform.gameObject.GetComponent<BossSpawnObject>();
                    Vector3 hitpoint = hit.point - hit.normal;
                    //Debug.Log(hitpoint);
                    BossSpwO.ObjectSpawn(hitpoint);
                    if (BossSpwO.lastSpawned != null)
                    {
                        savedObject = BossSpwO.lastSpawned;
                        //Rigidbody rb = savedObject.GetComponent<Rigidbody>();
                        //rb.useGravity = false;
                        BossSpwO.lastSpawned = null;
                    }
                    SuccFromBoss = true;
                }
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

                else
                { }
            }  
        }
    }

    public void CantSucc()
    {
        canSucc = false;
        uIcontrol.SuckingCDBar(canSucc);
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
        }                          
    }   
    IEnumerator FriendlyCD()
    {
        FriendCD = true;
     
        yield return new WaitForSeconds(8);

        FriendCD = false;
    }

}
