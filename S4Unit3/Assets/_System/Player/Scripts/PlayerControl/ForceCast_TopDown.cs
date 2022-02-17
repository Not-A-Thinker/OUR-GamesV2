using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast_TopDown : MonoBehaviour
{
    [Header("Component")]
    public GameObject objectParent;

    public GameObject rangeObj;
    [SerializeField] GameObject RangeBigObj;

     UIcontrol UIcontrol;
    [SerializeField] GameObject Charitor;
    [Header("P1 Push State")]
    public float _force = 500f;
    public float _range = 15f;
    public bool isfriendPushed, Charge, isShooted ;//���Ĳ�o�Ϊ�
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

    void Update()
    {
        ///�g���e�R��
        if (Charge)
        {
            if (!ShootInCD)
            {
                SetOldQue();
                Accumulate();
            }
            else
            {
                Charge = false;
            }
        }

        ///�g��
        if (isShooted)
        {
            if (!ShootInCD)
            {
                Shoot();
                rangeObj.SetActive(false);
                UIcontrol.PushingStop();
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
        if (Input.GetButtonDown("Fire1"))
        {
            Charge = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isShooted = true;
            ResetOldQue();
        }
        if(Input.GetButton("HelpFriendP1"))
        {
            isfriendPushed = true;
        }
        if (Input.GetButtonUp("HelpFriendP1"))
        {
            isfriendPushed = false;
        }

    }

    private void Shoot()
    {
        ///�ˬd��W���S�����
        //Debug.Log(force);
        if (objectParent.transform.childCount > 0)
        {
            //CD��W�O
            Timer = 0;      
            StartCoroutine(ShootCD(PushMaxCD));
            //�]�m���
            gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(countFloat);
        }

        Move move = GetComponent<Move>();
        move.SpeedFast(speedSlow);
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
        countFloat = 0;
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

        Move move = GetComponent<Move>();
        move.SpeedSlow(speedSlow);
    }

    private void FriendlyPushed()
    {
        ///�g�u
        Vector3 startPos = RangeBigObj.transform.position;
        Vector3 endPos = RangeBigObj.transform.up;
        RaycastHit isPlayerHit;
        if (Physics.Raycast(startPos, endPos, out isPlayerHit, _range))
        {
            //Debug.Log(isPlayerHit.transform.tag+"+"+isPlayerHit.transform.name);
            //Debug.DrawRay(startPos, endPos * _range);
            //Debug.DrawLine(transform.position, hit.point, Color.red,0.5f, false);

            if (isPlayerHit.transform.tag == "Player" && friendPushed == false)
            {
                rangeObj.SetActive(true);
                if (isPlayerHit.transform.gameObject != this.gameObject)
                {
                    ///����������
                    //Debug.Log(isPlayerHit.transform.gameObject.name);
                    Move move = isPlayerHit.transform.GetComponent<Move>();
                    StartCoroutine(move.GetFriendlyControl(RangeBigObj.transform.forward));
                    ///CD
                    StartCoroutine(FriendCD(8));
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
        Charitor.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        rangeObj.SetActive(false);
    }
}

