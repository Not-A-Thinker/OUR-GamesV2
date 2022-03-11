using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1GetCube : MonoBehaviour
{
    public GameObject objectParent;

    public GameObject direction;

    public Transform SpawnPoint;

    public float SpeedToSlowDown;

    Move move;

    public void PlayerGetCube(GameObject cube)
    {
        // Saveing Cube on the Top of dog head
        if (objectParent.transform.childCount < 3)
        {
            move = GetComponent<Move>();
            move.SpeedSlow(SpeedToSlowDown);

            ///�Ω��l�Ƥ������m
            cube.transform.parent = objectParent.transform;
            cube.transform.position = new Vector3(transform.position.x + 3, transform.position.y + 4, transform.position.z);
            cube.transform.rotation = new Quaternion(0, 0, 0, 0);
            cube.GetComponent<Rigidbody>().useGravity = false;

            ///�Ω��l�Ƥ����"����"
            cube.AddComponent<ObjectRotation>();
            cube.GetComponent<ObjectRotation>().target = objectParent;
            cube.GetComponent<ObjectRotation>()._isInCount = true;
            if (cube.GetComponent<Bullet>())
            {
                cube.GetComponent<Bullet>().bossToSuck = false;
            }       
            //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //if (!cube.gameObject.GetComponent<ObjectDestroy>())
            //    Destroy(cube.GetComponent<ObjectDestroy>());
        }
       else
            cube.transform.position = transform.forward * Time.deltaTime ;
    }

    ///�g������e�m�]�m
    public void PlayerSpawnCube(float force)
    {
        int parentMax = objectParent.transform.childCount;
        //Debug.Log(force);
        int newForce = (int)force;
        ///�ھڤO�׽վ�g����k�]�b�W�O����w�g���̤j�ȩҥH�̤j�O�@�Ϊ̤j��@�^
        if (force>=1)
        {
            StartCoroutine(TheBigOne(parentMax, newForce));        
        }
        else
        {
            PlayerSetCube(parentMax, newForce);
        }    
    }

    // �����Q�����ɷ|Ĳ�o
    public void PlayerGoneCube()
    {
        int parentMax = objectParent.transform.childCount;
        move = GetComponent<Move>();
        move.SpeedReset();
        //Debug.Log(parentMax);
        // �������W���������
        for (int i=0;i< parentMax;i++)
        {         
            GameObject cube = objectParent.transform.GetChild(objectParent.transform.childCount-1).gameObject;
            Rigidbody Rb = cube.GetComponent<Rigidbody>();
            cube.GetComponent<ObjectRotation>()._isInCount = false;
            if (cube.GetComponent<ObjectDestroy>())
            {
                    cube.GetComponent<ObjectDestroy>().isSucked = false;
            }
            //���m����������A
            Rb.constraints = RigidbodyConstraints.None;
            Rb.useGravity = true;
            cube.transform.position = SpawnPoint.position;
            cube.transform.parent = null;
        }
    }

    void PlayerSetCube(int parentMax,int force)
    {
        //�C�Ӥ���g���e���n�]�m�@��
        move = GetComponent<Move>();
       
       

        int caseNum = 0;

        GameObject cube = objectParent.transform.GetChild(parentMax - 1).gameObject;

        if (parentMax==1)    
            move.SpeedReset();       
        else       
            move.SpeedFast();
        

        if (force >= 3)
            caseNum = 2;     
        else
        {
            caseNum = 1;
            force = 2;
        }
        //Debug.Log(force);

        Rigidbody Rb = cube.GetComponent<Rigidbody>();

        Rb.constraints = RigidbodyConstraints.None;
        Rb.useGravity = true;

        cube.transform.position = SpawnPoint.position;
        cube.transform.parent = null;

        if (cube.GetComponent<Bullet>())
        {
            cube.GetComponent<Bullet>().isAttacking = true;
        }

        if (cube.GetComponent<ObjectDestroy>())
        {
            cube.GetComponent<ObjectDestroy>().isSucked = false;
        }
        cube.AddComponent<ObjectDamage>();
        cube.GetComponent<ObjectDamage>().SetDamage(caseNum);
        cube.GetComponent<ObjectDamage>().Direction = direction.transform.forward;

        cube.GetComponent<ObjectRotation>()._isInCount = false;
        //Rb.AddForceAtPosition(direction.transform.forward * 3500f * 100 * Time.deltaTime, cube.transform.position, ForceMode.Impulse);
    }

    IEnumerator TheBigOne(int parentMax, int force)
    {      
        //�W�O���\�T�s�o
        int Max = parentMax;
        for (int i = 0; i < Max; i++)
        {
            PlayerSetCube(parentMax, force);
            parentMax = objectParent.transform.childCount;
            yield return new WaitForSeconds(0.3f);
            //Debug.Log(parentMax);
        }
    }
}
