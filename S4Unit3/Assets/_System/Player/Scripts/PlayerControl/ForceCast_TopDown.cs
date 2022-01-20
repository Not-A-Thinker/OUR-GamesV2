using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast_TopDown : MonoBehaviour
{
    [Header("Component")]
    public GameObject objectParent;

    public GameObject rangeObj;
    [SerializeField] GameObject RangeBigObj;

    [SerializeField] UIcontrol UIcontrol;
    [SerializeField] GameObject Charitor;
    [Header("P1 Push State")]
    public float _force = 500f;
    public float _range = 15f;
    public bool friendPushed, Charge, Shooted, ShootInCD;
  

    public bool _attackTrigger = false;
    Quaternion OldQuate;

    [Header("P1 Carge State")]
    [SerializeField] float countFloat = 0;
    public float CountMax = 2f;

    [Header("P1 Attack CD")]
    [SerializeField] float Timer = 1;
    public int PushMaxCD = 1;

    void Start()
    {
        Shooted = false;
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }

    void Update()
    {
        //射擊前充能
        if (Charge)
        {
            if (Shooted == false)
            {
                //transform.rotation = Quaternion.Slerp(Charitor.transform.rotation, ShootRot.transform.rotation, 15f * Time.deltaTime);
                Accumulate();
            }
            else
            {
                Charge = false;          
            }            
        }

        //射擊
        if (Shooted)
        {
            if (Shooted == false)
            {
                Shoot((int)_force);
                rangeObj.SetActive(false);
                UIcontrol.PushingStop();
            }
            else
                Shooted = false;
        }

        //推隊友
        if (friendPushed)
        {
            FriendlyPushed();
        }

        //射擊CD
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

        UIcontrol.PushingCDBar(Timer/PushMaxCD);

        //if (Input.GetButton("Fire1"))
        //{
        //    rangeObj.SetActive(true);
        //    StartCoroutine("PushFriendCD");
        //}
        //if (Input.GetButtonUp("HelpFriendP1"))
        //{
        //    rangeObj.SetActive(false);
        //}
    }

    public void Shoot(int force)
    {
        //Debug.Log(force);
        if (objectParent.transform.childCount > 0)
        {
            gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(countFloat);
            StartCoroutine(ShootCD(1));
        }    
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
        countFloat = 0;
    }

    public void Accumulate()
    {
        //rangeObjRed = rangeObj.GetComponent<Renderer>();

        ////Call SetColor using the shader property name "_Color" and setting the color to red

        //rangeObjRed.material.SetColor("_Color", Color.green);
        rangeObj.SetActive(true);

        countFloat += (1 * Time.deltaTime);

        if(countFloat > 1f)
            //rangeObjRed.material.SetColor("_Color", Color.yellow);
        if (countFloat > 3f)
            //rangeObjRed.material.SetColor("_Color", Color.red);
        if (countFloat > 3.2f)
        {
            countFloat = 0;
        }
            

        UIcontrol.PushingBar(countFloat);
    }

    public void FriendlyPushed()
    {
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
                    //Debug.Log(isPlayerHit.transform.gameObject.name);
                    Move move = isPlayerHit.transform.GetComponent<Move>();
                    StartCoroutine(move.GetFriendlyControl(RangeBigObj.transform.forward));
                    StartCoroutine(FriendCD(8));
                    rangeObj.SetActive(false);
                }
            }
        }
    }

   //射擊CD 2
   IEnumerator ShootCD(int time)
    {
        Shooted = true;
        UIcontrol.PushingCDBar(time);
        yield return new WaitForSeconds(time);
        Shooted = false;
        UIcontrol.PushingCDBar(time);
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

    IEnumerator FriendCD(int time)
    {
        friendPushed = true;
        rangeObj.SetActive(false);
        yield return new WaitForSeconds(time);
        friendPushed = false;
    }

    //重置成射擊前方為
    public void SetOldQue()
    {
        OldQuate = Charitor.transform.rotation;
    }

    public void ResetOldQue()
    {
        Charitor.transform.rotation = OldQuate;
        OldQuate = new Quaternion(0, 0, 0, 0);
        rangeObj.SetActive(false);
    }
}
