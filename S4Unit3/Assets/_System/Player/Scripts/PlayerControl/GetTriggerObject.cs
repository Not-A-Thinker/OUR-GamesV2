using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTriggerObject : MonoBehaviour
{
    public float _Hight = 1;
    //public float _RotationSpeed = 20;

    //getChip
    public GameObject ChipParent,SpcAttack;
    float totalHight = 2f;

    [SerializeField] GameObject chip;
    P1GetCube getCube;

    public bool SpawnDone;

    private void Start()
    {
        getCube = GameObject.Find("Player1").GetComponent<P1GetCube>();
    }

    private void Update()
    {
        if (ChipParent.transform.childCount >= 5 && SpawnDone == false)
        {
            SpawnSpecialAttack();
          
        }
    }

    private void OnTriggerEnter(Collider Obj)
    {
        Debug.Log(Obj.tag);
        if (Obj.transform.gameObject.layer == 6&&Obj.transform.tag!="Player")
        {
            if (Obj.transform.tag == "Clip")
            {
                GameObject getedObject = Obj.gameObject;
                Rigidbody Obj_rb = getedObject.GetComponent<Rigidbody>();
                getedObject.GetComponent<Collider>().isTrigger = false;
                //getedObject.transform.position = new Vector3(0, 0, 0);
                ForceRepel_TopDown forceRepel_TopDown = transform.gameObject.GetComponent<ForceRepel_TopDown>();
                forceRepel_TopDown.resetObject();

                Obj_rb.useGravity = false;       

                getedObject.transform.parent = ChipParent.transform;

                getedObject.transform.position = new Vector3(ChipParent.transform.position.x*Time.deltaTime, totalHight * Time.deltaTime, ChipParent.transform.position.y * Time.deltaTime);
                totalHight = totalHight + 2;
                getedObject.transform.rotation = new Quaternion(0, 0, 0, 0);

                getedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else if (Obj.transform.tag == "Object")
            {
                GameObject getedObject = Obj.gameObject;
                Rigidbody Obj_rb = getedObject.GetComponent<Rigidbody>();
                Obj.transform.GetComponent<Collider>().isTrigger = false;
                ForceRepel_TopDown forceRepel_TopDown = GetComponent<ForceRepel_TopDown>();
                forceRepel_TopDown.resetObject();

                Obj_rb.useGravity = false;
                //Debug.Log(Obj.name + "Trigger");             
                if (getCube.objectParent.transform.childCount < 3)
                {
                    getCube.PlayerGetCube(Obj.transform.gameObject);
                }
                else
                {
                    //Obj_rb.useGravity = true;
                }

                gameObject.GetComponent<ForceRepel_TopDown>().CantSucc();
            }
            else if (Obj.transform.tag == "Boss")
            {

            }
            else
            {
                int i = Random.Range(1, 3);
                for (int j = 0; j < i; j++)
                {
                    GameObject getedObject = Instantiate(chip, Obj.transform.position, Quaternion.identity);
                    Rigidbody Obj_rb = transform.parent.parent.GetComponent<Rigidbody>();
                    getedObject.GetComponent<Collider>().isTrigger = false;
                    ForceRepel_TopDown forceRepel_TopDown = transform.gameObject.GetComponent<ForceRepel_TopDown>();
                    forceRepel_TopDown.resetObject();

                    Obj_rb.useGravity = false;

                    getedObject.transform.parent = ChipParent.transform;
                    totalHight = totalHight + 2;
                    getedObject.transform.position = new Vector3(this.transform.position.x, totalHight, this.transform.position.z);
                    getedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    getedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }

                Destroy(Obj.transform.gameObject);
            }
        }     
    }

    void SpawnSpecialAttack()
    {
        SpawnDone = true;
        for (int k = 5; k > 0; k--)
        {
            Destroy(ChipParent.transform.GetChild(ChipParent.transform.childCount - k).gameObject);
            totalHight = totalHight - 2;
            if (k == 1)
                SpawnDone = false;
            Debug.Log(SpawnDone);
        }
        GameObject NewSpcAtk = Instantiate(SpcAttack);
        getCube.PlayerGetCube(NewSpcAtk);
    }
}
