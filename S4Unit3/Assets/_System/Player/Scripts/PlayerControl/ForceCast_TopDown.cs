using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast_TopDown : MonoBehaviour
{
    public float _force = 500f;
    public float _range = 15f;

    public bool _attackTrigger = false;

    public int maxItem = 5;

    public GameObject objectParent;

    public GameObject rangeObj;
    Renderer rangeObjRed;
   [SerializeField] GameObject RangeBigObj;

   [SerializeField] UIcontrol UIcontrol;
   [SerializeField] GameObject Charitor;

    float countFloat = 0;

    int count = 0;

    public bool friendPushed,Shooted;

    Quaternion OldQuate;


    void Start()
    {
        Shooted = false;
        UIcontrol = GameObject.Find("GUI").GetComponent<UIcontrol>();
    }

    void Update()
    {
        //Vector3 selfPos = new Vector3(transform.position.x, 1, transform.position.z);
        //Debug.DrawRay(selfPos, transform.forward * _range, Color.red);
        if (Input.GetButtonDown("Fire1"))
            OldQuate = Charitor.transform.rotation;

        if (Input.GetButton("HelpFriendP1"))
        {       
            //Vector3 startPos = transform.position;
            //Vector3 endPos = RangeBigObj.transform.forward;
            //RaycastHit isPlayerHit;

            //if (Physics.Raycast(startPos, endPos, out isPlayerHit, _range))
            //{
            //    //Debug.Log(isPlayerHit.transform.tag);
            //    //Debug.DrawRay(startPos, endPos * _range);
            //    //Debug.DrawLine(transform.position, hit.point, Color.red,0.5f, false);
            //    if (isPlayerHit.transform.tag == "Player" && friendPushed == false)
            //    {
            //        rangeObj.SetActive(true);
            //        if (isPlayerHit.transform.gameObject != gameObject)
            //        {
            //            //Debug.Log(isPlayerHit.transform.gameObject.name);
            //            Move move = isPlayerHit.transform.GetComponent<Move>();
            //            StartCoroutine(move.GetFriendlyControl(RangeBigObj.transform.forward));
            //            StartCoroutine(FriendCD(8));
            //            rangeObj.SetActive(false);
            //        }
            //    }
            //}     
        }

        if (Input.GetButtonUp("HelpFriendP1"))
        {
            rangeObj.SetActive(false);
        }

        if (Input.GetButton("Fire1"))
        {
            if(Shooted==false)
            {
                //transform.rotation = Quaternion.Slerp(Charitor.transform.rotation, ShootRot.transform.rotation, 15f * Time.deltaTime);
                Accumulate();

                rangeObj.SetActive(true);
            }     
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (Shooted == false)
            {
                Shoot((int)_force);
                rangeObj.SetActive(false);
                UIcontrol.PushingStop();
            }

            Charitor.transform.rotation = OldQuate;
            OldQuate = new Quaternion(0, 0, 0, 0);

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
        //if (Input.GetButton("HelpFriendP1"))
        //{
        //    rangeObj.SetActive(true);
        //    StartCoroutine("PushFriendCD");
        //}
        //if (Input.GetButtonUp("HelpFriendP1"))
        //{
        //    rangeObj.SetActive(false);
        //}
    }

    void Shoot(int force)
    {
        //Debug.Log(force);
        if (objectParent.transform.childCount > 0)
        {
            gameObject.GetComponent<P1GetCube>().PlayerSpawnCube(count);
            StartCoroutine(ShootCD(3));
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
        count = 0;
    }

    void Accumulate()
    {
        //rangeObjRed = rangeObj.GetComponent<Renderer>();

        ////Call SetColor using the shader property name "_Color" and setting the color to red

        //rangeObjRed.material.SetColor("_Color", Color.green);

        countFloat += (1 * Time.deltaTime);

        if(countFloat > 1f)
            //rangeObjRed.material.SetColor("_Color", Color.yellow);
        if (countFloat > 3f)
            //rangeObjRed.material.SetColor("_Color", Color.red);
        if (countFloat > 3.2f)
        {
            countFloat = 0;
        }
            
        count = (int)countFloat;

        UIcontrol.PushingBar(count);
    }

   IEnumerator ShootCD(int time)
    {
        Shooted = true;
        yield return new WaitForSeconds(time);
        Shooted = false;
    }

    IEnumerator FriendCD(int time)
    {
        friendPushed = true;
        rangeObj.SetActive(false);
        yield return new WaitForSeconds(time);
        friendPushed = false;
    }
}
