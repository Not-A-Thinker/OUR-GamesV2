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
    public bool isfriendPushed, Charge, isShooted ;//控制器觸發用的
    bool friendPushed, ShootInCD;//檢查用

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
        ///射擊前充能
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

        ///射擊
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

        ///推隊友
        if (isfriendPushed)
        {
            FriendlyPushed();
        }

        ///攻擊的CD
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

        ///攻擊蓄力的UI
        UIcontrol.PushingCDBar(Timer / PushMaxCD);

        ///OldInput備案 如果New Input手把不能用的時候打開
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
        ///檢查手上有沒有方塊
        //Debug.Log(force);
        if (objectParent.transform.childCount > 0)
        {
            //CD跟蓄力
            Timer = 0;      
            StartCoroutine(ShootCD(PushMaxCD));
            //設置方塊
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

        ///重置狀態
        countFloat = 0;
        isShooted = false;
        Charge = false;
    }

    ///蓄力
    private void Accumulate()
    {
        //rangeObjRed = rangeObj.GetComponent<Renderer>();
        ////Call SetColor using the shader property name "_Color" and setting the color to red
        //rangeObjRed.material.SetColor("_Color", Color.green);
        
        ///地毯開啟
        rangeObj.SetActive(true);

        ///蓄力條蓄力計算
        countFloat += Time.deltaTime;
        if (countFloat > CountMax +1f)
            countFloat = 0;
        ///蓄力條UI
        float BarValue = countFloat/CountMax;
        UIcontrol.PushingBar(BarValue);

        Move move = GetComponent<Move>();
        move.SpeedSlow(speedSlow);
    }

    private void FriendlyPushed()
    {
        ///射線
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
                    ///不能讓對方動
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

    ///另外一個射擊CD 一樣的
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

    ///推隊友CD
    IEnumerator FriendCD(int time)
    {
        friendPushed = true;
        rangeObj.SetActive(false);
        yield return new WaitForSeconds(time);
        friendPushed = false;
    }

    ///記錄射擊前方位
    public void SetOldQue()
    {
        OldQuate = Charitor.transform.rotation;
    }

    ///重置成射擊前方位
    public void ResetOldQue()
    {
        Charitor.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        rangeObj.SetActive(false);
    }
}

