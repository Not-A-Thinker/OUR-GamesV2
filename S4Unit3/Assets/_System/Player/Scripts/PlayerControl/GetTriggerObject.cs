using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTriggerObject : MonoBehaviour
{
    //public float _RotationSpeed = 20;

    //getChip
    public GameObject ChipParent;
    float totalHight = 6f;
    [SerializeField] GameObject SpcAttack;
    GameObject chip;
    P1GetCube getCube;
    [SerializeField] ParticleSystem _GetCubeEffect;

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
        ///如果碎片大過等如3就會自動生成特殊方塊給隊友
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
            ///處理碎片
            if (Obj.transform.tag == "Clip")
            {
                ///初始化碎片             
                GameObject getedObject = Obj.gameObject;
                getedObject.GetComponent<Rigidbody>();
                getedObject.GetComponent<Collider>().isTrigger = false;
                //getedObject.transform.position = new Vector3(0, 0, 0);
                ForceRepel_TopDown forceRepel_TopDown = transform.gameObject.GetComponent<ForceRepel_TopDown>();
                forceRepel_TopDown.resetObject();
                //Obj_rb.useGravity = false;       
                getedObject.transform.parent = ChipParent.transform;
                getedObject.transform.position = new Vector3(ChipParent.transform.position.x, totalHight, ChipParent.transform.position.z);
                //totalHight = totalHight + 2;
                getedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                getedObject.AddComponent<ObjectRotation>();
                getedObject.GetComponent<ObjectRotation>().target = ChipParent;
                getedObject.GetComponent<ObjectRotation>()._isInCount = true;
                getedObject.GetComponent<ObjectRotation>()._isClip = true;
                PlayerSoundEffect.PlaySound("Cat_Attack");
                if(_GetCubeEffect!=null)
                    _GetCubeEffect.Play();
                //getedObject.tag = "Object";

            }
            //處理方塊
            else if (Obj.transform.tag == "Object")
            {
                ///初始化方塊
                GameObject getedObject = Obj.gameObject;
                Rigidbody Obj_rb = getedObject.GetComponent<Rigidbody>();
                Obj.transform.GetComponent<Collider>().isTrigger = false;
                ForceRepel_TopDown forceRepel_TopDown = GetComponent<ForceRepel_TopDown>();
                forceRepel_TopDown.resetObject();
                forceRepel_TopDown.SuckCount--;
                forceRepel_TopDown.ClearBossSpawnObj();

                //Obj_rb.useGravity = false;
                //Debug.Log(Obj.name + "Trigger");             
                if (getCube.objectParent.transform.childCount < 3)
                {
                    ///如果Count的方塊少過指定數目
                    getCube.PlayerGetCube(Obj.gameObject);
                    forceRepel_TopDown.resetObject();
                    if (getedObject.GetComponent<ObjectDestroy>()) 
                        getedObject.GetComponent<ObjectDestroy>().isSucked = true;
                }
                else
                {
                    ///如果Count的滿了
                    Obj_rb.useGravity = true;
                }
                PlayerSoundEffect.PlaySound("Cat_Attack");
                if (_GetCubeEffect != null)
                    _GetCubeEffect.Play();
                //gameObject.GetComponent<ForceRepel_TopDown>().CantSucc();
            }
            ///不應該吸到boss
            else if (Obj.transform.tag == "Boss" || Obj.transform.tag == "DummyBoss")
            {

            }
            ///吸到Boss技能
            else
            {
                int i = Random.Range(1, 3);
                ///生成clip
                for (int j = 0; j < i; j++)
                {
                    GameObject getedObject = Instantiate(chip, Obj.transform.position, Quaternion.identity);
                    ///重置clip                
                    Rigidbody Obj_rb = transform.parent.parent.GetComponent<Rigidbody>();
                    getedObject.GetComponent<Collider>().isTrigger = false;
                    ForceRepel_TopDown forceRepel_TopDown = transform.gameObject.GetComponent<ForceRepel_TopDown>();
                    forceRepel_TopDown.resetObject();
                    forceRepel_TopDown.SuckCount--;
                    Obj_rb.useGravity = false;
                    ///設置clip到Count上
                    getedObject.transform.parent = ChipParent.transform;
                    //totalHight = totalHight + 2;
                    getedObject.transform.position = new Vector3(this.transform.position.x, totalHight, this.transform.position.z);
                    getedObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    getedObject.AddComponent<ObjectRotation>();
                    getedObject.GetComponent<ObjectRotation>().target = ChipParent;
                    getedObject.GetComponent<ObjectRotation>()._isInCount = true;
                    getedObject.GetComponent<ObjectRotation>()._isClip = true;                  
                    //getedObject.tag = "Object";
                    //getedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
                PlayerSoundEffect.PlaySound("Cat_Attack");
                if (_GetCubeEffect != null)
                    _GetCubeEffect.Play();
                Destroy(Obj.transform.gameObject);
            }
        }     
    }

    ///生成特殊攻擊
    void SpawnSpecialAttack()
    {
        SpawnDone = true;
        for (int k = ClipMax; k > 0; k--)
        {
            ///clip消失
            Destroy(ChipParent.transform.GetChild(ChipParent.transform.childCount - k).gameObject);
            totalHight = totalHight - 2;
            if (k == 1)
                SpawnDone = false;
            //Debug.Log(SpawnDone);
        }
        ///生成特殊攻擊
        GameObject NewSpcAtk = Instantiate(SpcAttack);
        NewSpcAtk.GetComponent<ObjectDestroy>().isSucked = true;
        getCube.PlayerGetCube(NewSpcAtk);
    }
}
