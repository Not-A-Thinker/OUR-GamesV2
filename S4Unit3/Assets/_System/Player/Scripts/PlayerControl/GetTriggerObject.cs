using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTriggerObject : MonoBehaviour
{
    public float _Hight = 1;
    //public float _RotationSpeed = 20;

    //getChip
    public GameObject ChipParent;
    float totalHight = 2f;
    [SerializeField] GameObject SpcAttack;
    GameObject chip;
    P1GetCube getCube;

    public int ClipMax;

    public bool SpawnDone;

    private void Start()
    {
        getCube = GameObject.Find("Player1").GetComponent<P1GetCube>();
        SpcAttack = Resources.Load("Prefabs/SpecialAttack") as GameObject;
        chip = Resources.Load("Prefabs/Clip") as GameObject;
    }

    private void Update()
    {
        ///�p�G�H���j�L���p3�N�|�۰ʥͦ��S����������
        if (ChipParent.transform.childCount >= ClipMax && SpawnDone == false)
        {
            //if(Input.GetButtonDown("Create"))
                SpawnSpecialAttack();
        }
    }

    private void OnTriggerEnter(Collider Obj)
    {
        //Debug.Log(Obj.tag);
        if (Obj.transform.gameObject.layer == 6 && Obj.transform.tag!="Player")
        {
            ///�B�z�H��
            if (Obj.transform.tag == "Clip")
            {
                ///��l�ƸH��
                Obj.transform.tag = "Object";
                GameObject getedObject = Obj.gameObject;
                getedObject.GetComponent<Rigidbody>();
                getedObject.GetComponent<Collider>().isTrigger = false;
                //getedObject.transform.position = new Vector3(0, 0, 0);
                ForceRepel_TopDown forceRepel_TopDown = transform.gameObject.GetComponent<ForceRepel_TopDown>();
                forceRepel_TopDown.resetObject();
                //Obj_rb.useGravity = false;       
                getedObject.transform.parent = ChipParent.transform;
                //getedObject.transform.position = new Vector3(ChipParent.transform.position.x, totalHight, ChipParent.transform.position.z );
                //totalHight = totalHight + 2;
                getedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                getedObject.AddComponent<ObjectRotation>();
                getedObject.GetComponent<ObjectRotation>().target = ChipParent;
                getedObject.GetComponent<ObjectRotation>()._isInCount = true;
            }
            //�B�z���
            else if (Obj.transform.tag == "Object")
            {
                ///��l�Ƥ��
                GameObject getedObject = Obj.gameObject;
                Rigidbody Obj_rb = getedObject.GetComponent<Rigidbody>();
                Obj.transform.GetComponent<Collider>().isTrigger = false;
                ForceRepel_TopDown forceRepel_TopDown = GetComponent<ForceRepel_TopDown>();
                forceRepel_TopDown.resetObject();
                forceRepel_TopDown.SuckCount--;

                //Obj_rb.useGravity = false;
                //Debug.Log(Obj.name + "Trigger");             
                if (getCube.objectParent.transform.childCount < 3)
                {
                    ///�p�GCount������ֹL���w�ƥ�
                    getCube.PlayerGetCube(Obj.gameObject);
                    forceRepel_TopDown.resetObject();
                    getedObject.GetComponent<ObjectDestroy>().isSucked = true;
                }
                else
                {
                    ///�p�GCount�����F
                    Obj_rb.useGravity = true;
                }

                //gameObject.GetComponent<ForceRepel_TopDown>().CantSucc();
            }
            ///�����ӧl��boss
            else if (Obj.transform.tag == "Boss")
            {

            }
            ///�l��Boss�ޯ�
            else
            {
                int i = Random.Range(1, 3);
                ///�ͦ�clip
                for (int j = 0; j < i; j++)
                {
                    GameObject getedObject = Instantiate(chip, Obj.transform.position, Quaternion.identity);
                    ///���mclip
                    getedObject.tag = "Object";
                    Rigidbody Obj_rb = transform.parent.parent.GetComponent<Rigidbody>();
                    getedObject.GetComponent<Collider>().isTrigger = false;
                    ForceRepel_TopDown forceRepel_TopDown = transform.gameObject.GetComponent<ForceRepel_TopDown>();
                    forceRepel_TopDown.resetObject();
                    forceRepel_TopDown.SuckCount--;

                    Obj_rb.useGravity = false;

                    ///�]�mclip��Count�W
                    getedObject.transform.parent = ChipParent.transform;
                    totalHight = totalHight + 2;
                    //getedObject.transform.position = new Vector3(this.transform.position.x, totalHight, this.transform.position.z);
                    getedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    getedObject.AddComponent<ObjectRotation>();
                    getedObject.GetComponent<ObjectRotation>().target = ChipParent;
                    getedObject.GetComponent<ObjectRotation>()._isInCount = true;
                    
                    //getedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }

                Destroy(Obj.transform.gameObject);
            }
        }     
    }

    ///�ͦ��S�����
    void SpawnSpecialAttack()
    {
        SpawnDone = true;
        for (int k = ClipMax; k > 0; k--)
        {
            ///clip����
            Destroy(ChipParent.transform.GetChild(ChipParent.transform.childCount - k).gameObject);
            totalHight = totalHight - 2;
            if (k == 1)
                SpawnDone = false;
            //Debug.Log(SpawnDone);
        }
        ///�ͦ��S�����
        GameObject NewSpcAtk = Instantiate(SpcAttack);
        Debug.Log(NewSpcAtk.name);
        getCube.PlayerGetCube(NewSpcAtk);
    }
}
