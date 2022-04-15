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
    public bool isfriendPushed, Charge, isShooted,isAim ;//控制器觸發用的
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

    [SerializeField] ParticleSystem DogCarge;

    void Start()
    {
        move = GetComponent<Move>();
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }
    /// <summary>
    /// 記得如果要用new input要在設定一次新按鈕
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
        ///射擊前充能
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

        ///射擊
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
        ///檢查手上有沒有方塊
        //Debug.Log(force);
        //CD跟蓄力
        Timer = 0;

        StartCoroutine(ShootCD(PushMaxCD));
        //設置方塊
         gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(countFloat);  

        ///重置狀態
        isShooted = false;
        Charge = false;
        DogCarge.gameObject.SetActive(false);
    }

    ///蓄力
    private void Accumulate()
    {
        DogCarge.gameObject.SetActive(true);
        DogCarge.Play();
        //rangeObjRed = rangeObj.GetComponent<Renderer>();
        ////Call SetColor using the shader property name "_Color" and setting the color to red
        //rangeObjRed.material.SetColor("_Color", Color.green);
        ///地毯開啟
        //rangeObj.SetActive(true);
        ///蓄力條蓄力計算
        countFloat += Time.deltaTime;
        if (countFloat > CountMax +1f)
            countFloat = 0;
        ///蓄力條UI
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
        ///射線
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
                    ///不能讓對方動
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
        rangeObj.SetActive(false);
        isAim = false;
        Charitor.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        move.SpeedFast();
        move.isDashClose = false;
        Debug.Log("Reset");
    }
}

