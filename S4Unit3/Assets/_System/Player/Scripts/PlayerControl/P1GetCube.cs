using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1GetCube : MonoBehaviour
{
    public GameObject objectParent;

    public GameObject direction;

    public Transform SpawnPoint;

    public float SpeedToSlowDown;

    public bool OneOnCarge;

    Move move;

    private void Start()
    {
        move = GetComponent<Move>();
    }

    public void StartCarge(int Carge)
    {
        int parentMax = objectParent.transform.childCount;
        for (int i = parentMax; i>0 ; i--)
        {
            GameObject cube = objectParent.transform.GetChild(i - 1).gameObject;
            if (i <= Carge)
            {
                cube.transform.position = SpawnPoint.position;
                cube.GetComponent<ObjectRotation>()._isInCount = false;                
                if (!OneOnCarge)
                {
                    cube.GetComponentInChildren<Particle_PlamCharge>().IsCollecting = true;
                    cube.GetComponentInChildren<ParticleSystem>().Play();
                    GetComponent<ForceCast_TopDown>().CargeObj = cube;
                    OneOnCarge = true;
                }            
            }
            else
            {
                cube.GetComponent<ObjectRotation>()._isInCount = true;
                cube.GetComponentInChildren<Particle_PlamCharge>().IsCollecting = false;
                cube.GetComponentInChildren<ParticleSystem>().Pause();
            }
        }       
    }

    public void PlayerGetCube(GameObject cube)
    {
        // Saveing Cube on the Top of dog head
        if (objectParent.transform.childCount < 3)
        {                 
            ///�Ω��l�Ƥ������m
            cube.transform.parent = objectParent.transform;
            cube.transform.position = new Vector3(transform.position.x + 3, transform.position.y + 4, transform.position.z);
            cube.transform.rotation = new Quaternion(0, 0, 0, 0);
            cube.GetComponent<Rigidbody>().useGravity = false;

            ///�Ω��l�Ƥ����"����"
            ObjectRotation objectRotation = cube.AddComponent<ObjectRotation>();
            objectRotation.target = objectParent;
            objectRotation._isInCount = true;

            if (cube.GetComponent<Bullet>())
                cube.GetComponent<Bullet>().bossToSuck = false;

            move.CubeSpeedDown(objectParent.transform.childCount, SpeedToSlowDown);
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
            StartCoroutine(TheBigOne(parentMax, newForce));        
        else
            PlayerSetCube(parentMax, newForce);   
    }

    // �����Q�����ɷ|Ĳ�o
    public void PlayerGoneCube()
    {        
        int parentMax = objectParent.transform.childCount;
        //Debug.Log(parentMax);
        if (parentMax > 0)
        {
            if (!GetComponent<PlayerState>().isDead)
            {
                move = GetComponent<Move>();
                move.SpeedReset();
                for (int i = 0; i < parentMax; i++)
                {
                    GameObject cube = objectParent.transform.GetChild(objectParent.transform.childCount - 1).gameObject;
                    Rigidbody Rb = cube.GetComponent<Rigidbody>();
                    if (cube.GetComponent<ObjectRotation>())
                        cube.GetComponent<ObjectRotation>()._isInCount = false;

                    if (cube.GetComponent<ObjectDestroy>())
                        cube.GetComponent<ObjectDestroy>().isSucked = false;
                    //���m����������A
                    Rb.constraints = RigidbodyConstraints.None;
                    Rb.useGravity = true;
                    cube.transform.position = SpawnPoint.position;
                    cube.transform.parent = null;
                }
                OneOnCarge = false;
            }
            //Debug.Log(parentMax);
            // �������W���������                   
        }     
    }

    void PlayerSetCube(int parentMax,int force)
    {
        //�C�Ӥ���g���e���n�]�m�@��
            
        int caseNum = 0;

        GameObject cube;
        if (GetComponent<ForceCast_TopDown>().CargeObj!=null)
        {
            cube = GetComponent<ForceCast_TopDown>().CargeObj;
        } 
        else
            cube = objectParent.transform.GetChild(parentMax - 1).gameObject;

        //Debug.Log(parentMax);
        cube.GetComponent<ObjectRotation>()._isInCount = false;

        if (parentMax == 1)
            move.SpeedReset();
        else
            move.CubeSpeedDown(parentMax, SpeedToSlowDown);
        

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
            cube.GetComponent<Bullet>().isAttacking = true;

        if (cube.GetComponent<ObjectDestroy>())
            cube.GetComponent<ObjectDestroy>().isSucked = false;


        ObjectDamage objectDamage = cube.AddComponent<ObjectDamage>();
        objectDamage.SetDamage(caseNum);
        objectDamage.Direction = direction.transform.forward;

   
        OneOnCarge = false;
        PlayerSoundEffect.PlaySound("Dog_Attack");
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
