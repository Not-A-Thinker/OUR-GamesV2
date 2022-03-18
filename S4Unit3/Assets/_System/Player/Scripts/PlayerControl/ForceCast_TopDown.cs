using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast_TopDown : MonoBehaviour
{
    [Header("Component")]
    public GameObject objectParent;

    public GameObject rangeObj;
    [SerializeField] GameObject RangeBigObj;
    [SerializeField] private Renderer Renderer;

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

    void Start()
    {
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }
    /// <summary>
    /// �O�o�p�G�n��new input�n�b�]�w�@���s���s
    /// </summary>
    void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(startPos, endPos, out hit, _range))
        {
            if (hit.transform.tag == "Boss")
                Renderer.material.color = Color.green;
            else
                Renderer.material.color = Color.red;
        }
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
                UIcontrol.PushingBar(BarValue);
            }
            else
            {
                countFloat = 0;
                UIcontrol.PushingStop();
            }
        }

        ///�g��
        if (isShooted)
        {
            if (!ShootInCD && objectParent.transform.childCount > 0)
            {
                Renderer.material.color = Color.red;
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
            if (!ShootInCD && objectParent.transform.childCount > 0)
                Charge = true;

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
                isShooted = true;

                if (!ShootInCD && objectParent.transform.childCount > 0)
                {
                    
                }
                else
                    UIcontrol.flyText(1, Color.red, "Cant Attack!");
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
        Move move = GetComponent<Move>();
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

        //else
        //{
        //    _attackTrigger = true;
        //    Vector3 startPos = transform.position;
        //    Vector3 endPos = transform.forward * 10;
        //    //Debug.DrawRay(startPos, endPos);

        //    RaycastHit hit;
        //    if (Physics.Raycast(startPos, endPos, out hit, _range))
        //    {
        //        //Debug.Log(hit.transform.name + "." + hit.transform.tag);

        //        if (hit.transform.tag == "Object")
        //        {
        //            hit.rigidbody.AddForceAtPosition(transform.forward * force* Time.deltaTime, hit.transform.position, ForceMode.Impulse);
        //        }

        //    }
        //    _attackTrigger = false;
        //}

        ///���m���A
        isShooted = false;
        Charge = false;
    }

    ///�W�O
    private void Accumulate()
    {
        //rangeObjRed = rangeObj.GetComponent<Renderer>();
        ////Call SetColor using the shader property name "_Color" and setting the color to red
        //rangeObjRed.material.SetColor("_Color", Color.green);
        
        ///�a��}��
        rangeObj.SetActive(true);

        ///�W�O���W�O�p��
        countFloat += Time.deltaTime;
        if (countFloat > CountMax +1f)
            countFloat = 0;
        ///�W�O��UI
        float BarValue = countFloat/CountMax;
        UIcontrol.PushingBar(BarValue);    
    }

    private void FriendlyPushed()
    {
        Charitor.transform.rotation = Quaternion.Slerp(Charitor.transform.rotation, transform.rotation, 15f * Time.deltaTime);
        ///�g�u
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.up;
        RaycastHit isPlayerHit;
        if (Physics.Raycast(startPos, endPos, out isPlayerHit, _range))
        {
            Debug.Log(isPlayerHit.transform.tag + "+" + isPlayerHit.transform.name);
            //Debug.DrawRay(startPos, endPos * _range);
            //Debug.DrawLine(transform.position, hit.point, Color.red,0.5f, false);

            if (isPlayerHit.transform.tag == "Player" && friendPushed == false && isPlayerHit.transform.name != name)
            {
                rangeObj.SetActive(true);
                if (isPlayerHit.transform.gameObject != this.gameObject)
                {
                    ///����������
                    //Debug.Log(isPlayerHit.transform.gameObject.name);
                    Move move = isPlayerHit.transform.GetComponent<Move>();
                    StartCoroutine(move.GetFriendlyControl(transform.forward));
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
        isAim = false;
        Charitor.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        rangeObj.SetActive(false);
        Move move = GetComponent<Move>();
        move.SpeedFast();
        move.isDashClose = false;
    }
}

