using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast_TopDown : MonoBehaviour
{
    [Header("Component")]
    public GameObject objectParent;
    Move move;
    public GameObject rangeObj;
    [SerializeField] GameObject RangeBigObj;
    //[SerializeField] private Renderer Renderer;

    UIcontrol UIcontrol;
    [SerializeField] GameObject Charitor;
    [Header("P1 Push State")]
    public float _force = 500f;
    public float _range = 20f;
    public bool isfriendPushed, Charge, isShooted,isAim ;//���Ĳ�o�Ϊ�
    bool friendPushed, ShootInCD;//�ˬd��

    public bool _attackTrigger = false;
    Quaternion OldQuate;

    [Header("P1 Carge State")]
    [SerializeField] float countFloat = 0;
    public float CountMax = 2f;

    [Header("P1 Attack CD")]
    [SerializeField] float Timer = 1;
 
    public int PushMaxCD = 1;
    public float speedSlow;

    [SerializeField] ParticleSystem DogCarge;

    void Start()
    {
        move = GetComponent<Move>();
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }
    /// <summary>
    /// �O�o�p�G�n��new input�n�b�]�w�@���s���s
    /// </summary>
    void Update()
    {
        //Vector3 startPos = transform.position;
        //Vector3 endPos = transform.forward;
        //RaycastHit hit;
        //if (Physics.Raycast(startPos, endPos, out hit, _range))
        //{
        //    if (hit.transform.tag == "Boss")
        //        Renderer.material.color = Color.green;
        //    else
        //        Renderer.material.color = Color.white;
        //}
        ///�g���e�R��
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

        ///�g��
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

        ///������
        if (isfriendPushed)
        {
            FriendlyPushed();
        }

        ///������CD
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

        ///�����W�O��UI
        UIcontrol.PushingCDBar(Timer / PushMaxCD);

        ///OldInput�Ʈ� �p�GNew Input��⤣��Ϊ��ɭԥ��}
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

            if(Input.GetButtonDown("Fire1"))
            {
                if (ShootInCD || objectParent.transform.childCount == 0)
                    UIcontrol.flyText(1, Color.red, "CD!!!");
            }

            if(Input.GetButton("Fire1"))
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

    public void P1_Aim_Slow()
    {    
        move.SpeedSlow(speedSlow);
        move.isDashClose = true;
    }

    private void Shoot()
    {
        ///�ˬd��W���S�����
        //Debug.Log(force);
        //CD��W�O
        Timer = 0;

        StartCoroutine(ShootCD(PushMaxCD));
        //�]�m���
         gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(countFloat);  

        ///���m���A
        isShooted = false;
        Charge = false;
        DogCarge.gameObject.SetActive(false);
    }

    ///�W�O
    private void Accumulate()
    {
        DogCarge.gameObject.SetActive(true);
        DogCarge.Play();
        //rangeObjRed = rangeObj.GetComponent<Renderer>();
        ////Call SetColor using the shader property name "_Color" and setting the color to red
        //rangeObjRed.material.SetColor("_Color", Color.green);
        ///�a��}��
        //rangeObj.SetActive(true);
        ///�W�O���W�O�p��
        countFloat += Time.deltaTime;
        if (countFloat > CountMax +1f)
            countFloat = 0;
        ///�W�O��UI
        //float BarValue = countFloat/CountMax;
        //UIcontrol.PushingBar(BarValue);
        
        if(countFloat<=1.5f)
        {
            int countInt = (int)(countFloat * 2);
            Debug.Log(countInt);
            int newScale = countInt + 1;
            DogCarge.gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        }        
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
    IEnumerator ShootCD(int time)
    {
        ShootInCD = true;
        yield return new WaitForSeconds(time);
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

