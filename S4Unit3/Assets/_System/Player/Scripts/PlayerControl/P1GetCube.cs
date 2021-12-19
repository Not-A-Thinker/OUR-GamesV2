using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1GetCube : MonoBehaviour
{
    //GameObject[] savedObject;

    public GameObject objectParent;

    public Transform SpawnPoint;

    float totalHight = 2f;

    Move move;

    [SerializeField] GameObject chip;

    public void PlayerGetCube(GameObject cube)
    {
        if (objectParent.transform.childCount<3)
        {
            move = GetComponent<Move>();
            move.SpeedSlow();

            cube.transform.parent = objectParent.transform;
            totalHight = totalHight + 3;
            cube.transform.position = new Vector3(this.transform.position.x, totalHight, this.transform.position.z);
            cube.transform.rotation = new Quaternion(0, 0, 0, 0);
            cube.GetComponent<ObjectDestroy>().isSucked = true;
            cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //if (!cube.gameObject.GetComponent<ObjectDestroy>())
            //    Destroy(cube.GetComponent<ObjectDestroy>());
        }
       else
        {
            cube.transform.position = transform.forward * Time.deltaTime ;
        }
    }

    public void PlayerSpawnCube(int force)
    {
        int parentMax = objectParent.transform.childCount;
        if (force>=2)
        {
            StartCoroutine(TheBigOne(parentMax, force));        
        }
        else
        {
            PlayerSetCube(parentMax, force);
        }    
    }

    public void PlayerGoneCube()
    {
        int parentMax = objectParent.transform.childCount;
        move = GetComponent<Move>();
        move.SpeedReset();
        //Debug.Log(parentMax);
        for (int i=0;i< parentMax;i++)
        {         
            GameObject cube = objectParent.transform.GetChild(objectParent.transform.childCount-1).gameObject;
            Rigidbody Rb = cube.GetComponent<Rigidbody>();
            if(cube.GetComponent<ObjectDestroy>().isSucked)
                cube.GetComponent<ObjectDestroy>().isSucked = false;
            Rb.constraints = RigidbodyConstraints.None;
            Rb.useGravity = true;
            totalHight = totalHight - 3;
            cube.transform.position = SpawnPoint.position;
            cube.transform.parent = null;
        }
    }

    void PlayerSetCube(int parentMax,int force)
    {
        move = GetComponent<Move>();
        move.SpeedFast();

        int caseNum = 0;

        totalHight = totalHight - 3;

        GameObject cube = objectParent.transform.GetChild(parentMax - 1).gameObject;
        if (force >= 3)
            caseNum = 2;     
        else
        {
            caseNum = 1;
            force = 2;
        }
        Debug.Log(force);


        Rigidbody Rb = cube.GetComponent<Rigidbody>();

        Rb.constraints = RigidbodyConstraints.None;
        Rb.useGravity = true;

        cube.transform.position = SpawnPoint.position;
        cube.transform.parent = null;
       
        cube.AddComponent<ObjectDamage>();
        cube.GetComponent<ObjectDamage>().SetDamage(caseNum);
        cube.GetComponent<ObjectDamage>().chip = chip;

        Rb.AddForceAtPosition(transform.forward * 2000f * 100 * Time.deltaTime, cube.transform.position, ForceMode.Impulse);
    }

    IEnumerator TheBigOne(int parentMax, int force)
    {      
        int Max = parentMax;
        for (int i = 0; i < Max; i++)
        {
            PlayerSetCube(parentMax, force);
            parentMax = objectParent.transform.childCount;
            yield return new WaitForSeconds(0.3f);
            Debug.Log(parentMax);
        }
    }
}