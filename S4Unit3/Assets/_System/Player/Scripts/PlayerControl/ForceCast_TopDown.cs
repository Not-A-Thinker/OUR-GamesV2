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
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }

    void Update()
    {
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
        if (Shooted)
        {
            if (!ShootInCD)
            {
                Shoot();               
                rangeObj.SetActive(false);
                UIcontrol.PushingStop();
            }
            else
                Shooted = false;
        }
           
        if (friendPushed)
        {
            FriendlyPushed();
        }

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

        UIcontrol.PushingCDBar(Timer);

        //if (Input.GetButton("Fire1"))
        //{
        //    if (ShootInCD == false)
        //    {
        //        Accumulate();
        //    }
        //    FriendlyPushed();
        //}

        //if (Input.GetButtonUp("Fire1"))
        //{
        //    if (ShootInCD == false)
        //    {
        //        Shoot();
        //        rangeObj.SetActive(false);
        //        UIcontrol.PushingStop();
        //    }
        //    ResetOldQue();
        //}
    }

    private void Shoot()
    {
        //Debug.Log(force);
        if (objectParent.transform.childCount > 0)
        {
            Timer = 0;
            gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(countFloat);
            StartCoroutine(ShootCD(PushMaxCD));
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
        Shooted = false;
        Charge = false;
    }

    private void Accumulate()
    {
        //rangeObjRed = rangeObj.GetComponent<Renderer>();

        ////Call SetColor using the shader property name "_Color" and setting the color to red

        //rangeObjRed.material.SetColor("_Color", Color.green);
        rangeObj.SetActive(true);

        countFloat += Time.deltaTime;

        if (countFloat > CountMax + 0.5f)
        {
            countFloat = 0;
        }

        float BarValue = countFloat / CountMax;


        UIcontrol.PushingBar(BarValue);
    }

    private void FriendlyPushed()
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

    IEnumerator FriendCD(int time)
    {
        friendPushed = true;
        rangeObj.SetActive(false);
        yield return new WaitForSeconds(time);
        friendPushed = false;
    }

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
