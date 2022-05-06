using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast_TopDown : MonoBehaviour
{
    [Header("Player Component")]
    public GameObject objectParent;
    Move move;
    public GameObject rangeObj;
    [SerializeField] GameObject RangeBigObj;
    //[SerializeField] private Renderer Renderer;
    public bool IsDead;
    UIcontrol UIcontrol;
    [SerializeField] GameObject Charitor;

    [Header("P1 Push State")]
    public float _force = 500f;
    public float _range = 20f;
    public bool isfriendPushed, Charge, isShooted,isAim ;//���Ĳ�o�Ϊ�
    public bool friendPushed, ShootInCD;//�ˬd��
    [SerializeField] bool IsCargeEffectPlay;
    public bool _attackTrigger = false;
    Quaternion OldQuate;

    //[Header("P1 RangeColor State")]
    //public Color NewRangeObjColor;
    //Renderer rangeHeadRenderer;
    //Renderer rangeRenderer;
    //Color OldRangeObjColor;  

    [Header("P1 Carge State")]
    [SerializeField] float countFloat = 0;
    public float CountMax = 2f;

    [Header("P1 Attack CD")]
    [SerializeField] float Timer = 1;
 
    public int PushMaxCD = 1;
    public float speedSlow;

    public bool isAttackWithAim;
    public GameObject CargeObj;
    
    [SerializeField] ParticleSystem DogCarge;

    void Start()
    {
        move = GetComponent<Move>();
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
        //rangeRenderer = rangeObj.GetComponent<Renderer>();
        //OldRangeObjColor = rangeRenderer.material.color;
        //rangeHeadRenderer = rangeObj.GetComponentInChildren<Renderer>();
    }
    /// <summary>
    /// �O�o�p�G�n��new input�n�b�]�w�@���s���s
    /// </summary>
    void Update()
    {
        //�g���e�R��
        if (Charge)
        {
            if (!ShootInCD && objectParent.transform.childCount > 0)
            {       
                Accumulate();
            }
            else
            {
                Charge = false;
            }
        }

        if (!Charge)
        {
            if (countFloat > 0)
            {
                countFloat -= 0.5f * Time.deltaTime;
                float BarValue = countFloat / CountMax;
                //UIcontrol.PushingBar(BarValue);
            }
            else
            {
                countFloat = 0;
                //UIcontrol.PushingStop();
            }
        }

        //�g��
        if (isShooted)
        {
            if (!ShootInCD && objectParent.transform.childCount > 0)
            {
                //Renderer.material.color = Color.white;
                Shoot();
            }
            else
                isShooted = false;
        }

        //������
        if (isfriendPushed)
        {
            FriendlyPushed();
        }

        //������CD
        if (ShootInCD)
        {
            if (Timer < PushMaxCD)
            {
                Timer += Time.deltaTime;
            }
            else
                Timer = PushMaxCD;
        }
        else
            Timer = PushMaxCD;

        //������CD UI
        UIcontrol.PushingCDBar(Timer / PushMaxCD);

        //OldInput�Ʈ� �p�GNew Input��⤣��Ϊ��ɭԥ��}
        if(!IsDead)
        {
            if (isAttackWithAim)
                AttackWithAim();
            else
                AttackWithOutAim();
        }    
        //Attack Without Aim
    }

    //private void FixedUpdate()
    //{
    //    //rangeObj Color Change
    //    if (rangeObj.activeInHierarchy)
    //    {
    //        Vector3 startPos = RangeBigObj.transform.position;
    //        Vector3 endPos = RangeBigObj.transform.up;
    //        RaycastHit Hit;
    //        if (Physics.Raycast(startPos, endPos, out Hit, _range))
    //        {
    //            if (Hit.transform.tag == "Boss")
    //            {
    //                rangeHeadRenderer.material.color = NewRangeObjColor;
    //                rangeRenderer.material.color = NewRangeObjColor;
    //            }       
    //            else
    //            {
    //                rangeRenderer.material.color = OldRangeObjColor;
    //                rangeHeadRenderer.material.color = OldRangeObjColor;
    //            }
                    
    //        }
    //    }
    //}
    private void AttackWithAim()
    {
        if (Input.GetButtonDown("AimP1"))
        {
            SetOldQue();
            P1_Aim_Slow();
        }

        if (Input.GetButton("AimP1"))
        {
            isAim = true;
            rangeObj.SetActive(true);

            if (Input.GetButton("HelpFriendP1"))
            {
                isfriendPushed = true;
            }

            if (Input.GetButtonUp("HelpFriendP1"))
            {
                isfriendPushed = false;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (!ShootInCD && objectParent.transform.childCount > 0)
                    isShooted = true;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (ShootInCD || objectParent.transform.childCount == 0)
                    UIcontrol.flyText(1, Color.red, "CD!!!");
            }

            if (Input.GetButton("Fire1"))
            {
                if (!ShootInCD && objectParent.transform.childCount > 0)
                    Charge = true;
            }
        }
        if (Input.GetButtonUp("AimP1"))
        {
            isAim = false;
            ResetOldQue();
        }
    }

    private void AttackWithOutAim()
    {
        if (Input.GetButton("HelpFriendP1"))
        {
            isfriendPushed = true;
        }
        if (Input.GetButtonUp("HelpFriendP1"))
        {
            isfriendPushed = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (ShootInCD || objectParent.transform.childCount == 0)
            {
                if(ShootInCD)
                    UIcontrol.flyText(1, Color.red, "CD!!!");
                else if (objectParent.transform.childCount == 0)
                    UIcontrol.flyText(1, Color.red, "No Cube");
            }                          
            else
            {
                SetOldQue();
                P1_Aim_Slow();
                StartCoroutine(CargeEffectPlay(0.6f));
            }
        }
        if (Input.GetButton("Fire1"))
        {
            if (!ShootInCD && objectParent.transform.childCount > 0)
            {
                Charge = true;
                rangeObj.SetActive(true);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (!ShootInCD && objectParent.transform.childCount > 0)
            {
                Charge = false;
                isShooted = true;
                ResetOldQue();
                StopCoroutine(CargeEffectPlay(0.6f));
            }             
        }       
    }

    public void P1_Aim_Slow()
    {    
        move.SpeedSlow(speedSlow);
        move.isDashClose = true;
    }

    private void Shoot()
    {
        StartCoroutine(ShootCD());
        ///�ˬd��W���S�����
        //CD��W�O
        Timer = 0; 
        //�]�m���
         gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(countFloat);  
        ///���m���A   
        Charge = false;
        DogCarge.gameObject.SetActive(false);
        CargeObj = null;
        StopCoroutine(CargeEffectPlay(0.6f));
        isShooted = false;
    }

    ///�W�O
    private void Accumulate()
    {
        countFloat += Time.deltaTime;
        if (countFloat > CountMax+0.5f)
            countFloat = 0;
        int CountInt = (int)(countFloat * 2);

        gameObject.GetComponent<P1GetCube>().StartCarge(CountInt);
        //rangeObjRed = rangeObj.GetComponent<Renderer>();
        ////Call SetColor using the shader property name "_Color" and setting the color to red
        //rangeObjRed.material.SetColor("_Color", Color.green);
        ///�a��}��
        //rangeObj.SetActive(true);
        ///�W�O���W�O�p��
      
        ///�W�O��UI
        //float BarValue = countFloat/CountMax;
        //UIcontrol.PushingBar(BarValue);       
        if(!IsCargeEffectPlay)
        {
            if(CargeObj!=null)
            {
                if (CargeObj.GetComponentInChildren<Particle_PlamCharge>() != null)
                {
                    CargeObj.GetComponentInChildren<Particle_PlamCharge>().value = countFloat / 1.5f;
                    IsCargeEffectPlay = true;
                }
            }         
        }             
        //DogCarge.gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);      
    }

    private void FriendlyPushed()
    {
        Charitor.transform.rotation = Quaternion.Slerp(Charitor.transform.rotation, transform.rotation, 15f * Time.deltaTime);
        ///�g�u
        Vector3 startPos = RangeBigObj.transform.position;
        Vector3 endPos = RangeBigObj.transform.up;
        RaycastHit isPlayerHit;
        if (Physics.Raycast(startPos, endPos, out isPlayerHit, _range))
        {
            //Debug.Log(isPlayerHit.transform.tag + "+" + isPlayerHit.transform.name);
            //Debug.DrawRay(startPos, endPos * _range);
            //Debug.DrawLine(transform.position, hit.point, Color.red,0.5f, false);

            if (isPlayerHit.transform.tag == "Player" && friendPushed == false && isPlayerHit.transform.name != name)
            {
                rangeObj.SetActive(true);
                if (isPlayerHit.transform.gameObject != this.gameObject)
                {
                    ///����������
                    //Debug.Log(isPlayerHit.transform.gameObject.name);
                    Move P1move = isPlayerHit.transform.GetComponent<Move>();
                    StartCoroutine(P1move.GetFriendlyControl(RangeBigObj.transform.forward));
                    ///CD
                    StartCoroutine(FriendCD(4));
                    rangeObj.SetActive(false);
                }
            }
        }
    }

    ///�t�~�@�Ӯg��CD �@�˪�
    public IEnumerator ShootCD()
    {
        ShootInCD = true;
        yield return new WaitForSeconds(PushMaxCD);
        ShootInCD = false;
        //while (time > 0)
        //{
        //    yield return new WaitForSeconds(1);
        //    time -= 1;
        //    UIcontrol.PushingCDBar(time);
        //    // This leaves the coroutine at this point
        //    // so in the next frame the while loop continues
        //    yield return null;
        //}
    }

    ///������CD
    IEnumerator FriendCD(int time)
    {
        friendPushed = true;
        rangeObj.SetActive(false);
        yield return new WaitForSeconds(time);
        friendPushed = false;
    }

    IEnumerator CargeEffectPlay(float Time)
    {
        IsCargeEffectPlay = false;
        yield return new WaitForSeconds(Time);
        IsCargeEffectPlay = false;
        yield return new WaitForSeconds(Time);
        IsCargeEffectPlay = false;
        yield return new WaitForSeconds(Time);
        IsCargeEffectPlay = false;
        yield return new WaitForSeconds(Time);
        StartCoroutine(CargeEffectPlay(Time));
    }

    ///�O���g���e���
    public void SetOldQue()
    {
        OldQuate = Charitor.transform.rotation;
    }

    ///���m���g���e���
    public void ResetOldQue()
    {
        rangeObj.SetActive(false);
        isAim = false;
        Charitor.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        move.SpeedFast();
        move.isDashClose = false;
        Debug.Log("Reset");
    }
}

