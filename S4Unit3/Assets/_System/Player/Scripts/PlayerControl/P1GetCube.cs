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
    public ParticleSystem _GetCubeEffect;

    Move move;

    private void Start()
    {
        move = GetComponent<Move>();
    }

    public void StartCarge(float Carge)
    {
        int parentMax = objectParent.transform.childCount;
        int newForce = (int)Carge;
        if (Carge >= 1.4f)
        {
            newForce = 3;
        }
        else
            newForce++;
        for (int i = 0; i< parentMax; i++)
        {
            GameObject cube = objectParent.transform.GetChild(i).gameObject;
            if (i <= newForce)
            {
                cube.transform.position = SpawnPoint.position;
                cube.GetComponent<ObjectRotation>()._isInCount = false;                
                if (!OneOnCarge)
                {
                    if(cube.GetComponentInChildren<Particle_PlamCharge>()!=null)
                    {
                        cube.GetComponentInChildren<Particle_PlamCharge>().IsCollecting = true;
                        cube.GetComponentInChildren<ParticleSystem>().Play();
                    }                
                    GetComponent<ForceCast_TopDown>().CargeObj = cube;
                    OneOnCarge = true;
                }            
            }
            else
            {
                if(cube.GetComponentInChildren<Particle_PlamCharge>() != null)
                {
                    cube.GetComponent<ObjectRotation>()._isInCount = true;
                    if (cube.transform.childCount != 0)
                    {
                        cube.GetComponentInChildren<Particle_PlamCharge>().IsCollecting = false;
                        cube.GetComponentInChildren<ParticleSystem>().Pause();
                    }            
                }              
            }
        }       
    }

    public void PlayerGetCube(GameObject cube)
    {
        // Saveing Cube on the Top of dog head              
       ///???????l???????????m
        cube.transform.parent = objectParent.transform;
        cube.transform.position = new Vector3(transform.position.x + 3, transform.position.y + 4, transform.position.z);
        cube.transform.rotation = new Quaternion(0, 0, 0, 0);
        cube.GetComponent<Rigidbody>().useGravity = false;

        ///???????l????????"????"
        ObjectRotation objectRotation = cube.AddComponent<ObjectRotation>();
        objectRotation.target = objectParent;
        objectRotation._isInCount = true;

        if (cube.GetComponent<Bullet>())
            cube.GetComponent<Bullet>().bossToSuck = false;

        move.CubeSpeedDown(objectParent.transform.childCount, SpeedToSlowDown);

        if (_GetCubeEffect != null)
            _GetCubeEffect.Play();
        //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //if (!cube.gameObject.GetComponent<ObjectDestroy>())
        //    Destroy(cube.GetComponent<ObjectDestroy>());
    }

    ///?g???????e?m?P?_
    public void PlayerSpawnCube(float force)
    {
        int parentMax = objectParent.transform.childCount;
        //Debug.Log(force);
        int newForce = (int)force;
        if (force >= 1.4f)
        {
            newForce = 3;
        }
        else
            newForce++;
        ///?????O???????g?????k
        if (newForce > 1 && parentMax>1)
            StartCoroutine(TheBigOne(parentMax, newForce)); 
        else
            PlayerSetCube(parentMax, newForce, GetComponent<ForceCast_TopDown>().CargeObj);
    }

    // ?????Q???????|???o
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
                    //???m???????????A
                    Rb.constraints = RigidbodyConstraints.None;
                    Rb.useGravity = true;
                    if (cube.GetComponentInChildren<Particle_PlamCharge>() != null)
                    {
                        cube.GetComponentInChildren<Particle_PlamCharge>().IsCollecting = false;
                        cube.GetComponentInChildren<ParticleSystem>().Stop();
                    }
                    cube.transform.position = SpawnPoint.position;
                    cube.transform.parent = null;              
                }
                OneOnCarge = false;
            }
            //Debug.Log(parentMax);
            // ???????W??????????                   
        }     
    }

    void PlayerSetCube(int parentMax,int force,GameObject cube)
    {
        //?C???????g???e???n?]?m?@??         
        int caseNum = 0;
        //Debug.Log(parentMax);

        cube.GetComponent<ObjectRotation>()._isInCount = false;

        if (force >= 3)
            caseNum = 2;     
        else
        {
            caseNum = 1;
        }
        //Debug.Log(force);

        Rigidbody Rb = cube.GetComponent<Rigidbody>();

        Rb.constraints = RigidbodyConstraints.None;
        Rb.useGravity = true;

        cube.transform.position = SpawnPoint.position;
        cube.transform.parent = null;

        if(objectParent.transform.childCount==0)
            move.SpeedReset();
        else if(objectParent.transform.childCount >=1)
            move.CubeSpeedDown(parentMax, SpeedToSlowDown);

        if (cube.GetComponent<Bullet>())
            cube.GetComponent<Bullet>().isAttacking = true;

        if (cube.GetComponent<ObjectDestroy>())
            cube.GetComponent<ObjectDestroy>().isSucked = false;


        ObjectDamage objectDamage = cube.AddComponent<ObjectDamage>();
        objectDamage.SetDamage(caseNum);
        objectDamage.Direction = direction.transform.forward;

   
        OneOnCarge = false;
        PlayerSoundEffect.PlaySound("Dog_Attack");

        for (int i = 0; i < objectParent.transform.childCount; i++)
            objectParent.transform.GetChild(i).GetComponent<ObjectRotation>()._isInCount = true;
        //Rb.AddForceAtPosition(direction.transform.forward * 3500f * 100 * Time.deltaTime, cube.transform.position, ForceMode.Impulse);
    }

    IEnumerator TheBigOne(int parentMax, int force)
    {
        //?W?O???\?T?s?o
        for (int i = 0; i < force; i++)
        {
            GameObject cube;
            if (GetComponent<ForceCast_TopDown>().CargeObj != null)
                cube = GetComponent<ForceCast_TopDown>().CargeObj;
            else if (objectParent.transform.childCount > 0)
                cube = objectParent.transform.GetChild(0).gameObject;
            else
                break;
            PlayerSetCube(parentMax, force, cube);
            yield return new WaitForSeconds(0.3f);
        }

        if (force == 2 && objectParent.transform.childCount > 0)
            objectParent.transform.GetChild(0).gameObject.GetComponent<ObjectRotation>()._isInCount = true;
    }
}
