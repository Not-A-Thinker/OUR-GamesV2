using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1GetCube : MonoBehaviour
{
    //GameObject[] savedObject;

    public GameObject objectParent;

    public GameObject direction;

    public Transform SpawnPoint;

    float totalHight = 2f;

    Move move;

    [SerializeField] GameObject chip;


    public void PlayerGetCube(GameObject cube)
    {
        // Saveing Cube on the Top of dog head
        if (objectParent.transform.childCount<3)
        {
            move = GetComponent<Move>();
            move.SpeedSlow();

            cube.transform.parent = objectParent.transform;
            //totalHight = totalHight + 3;
            cube.transform.position = new Vector3(this.transform.position.x+2 , 3, this.transform.position.z);
            cube.transform.rotation = new Quaternion(0, 0, 0, 0);
            cube.GetComponent<Rigidbody>().useGravity = false;
           
            cube.AddComponent<ObjectRotation>();
            cube.GetComponent<ObjectRotation>().target = objectParent;
            cube.GetComponent<ObjectRotation>().inBox = true;
            cube.GetComponent<Bullet>().bossToSuck = false;
            //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //if (!cube.gameObject.GetComponent<ObjectDestroy>())
            //    Destroy(cube.GetComponent<ObjectDestroy>());
        }
       else
            cube.transform.position = transform.forward * Time.deltaTime ;
    }

    public void PlayerSpawnCube(float force)
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
            if (!cube.GetComponent<ObjectDestroy>())
            {
                if (cube.GetComponent<ObjectDestroy>().isSucked)
                    cube.GetComponent<ObjectDestroy>().isSucked = false;
            }
            
            Rb.constraints = RigidbodyConstraints.None;
            Rb.useGravity = true;
            totalHight = totalHight - 3;
            cube.transform.position = SpawnPoint.position;
            cube.transform.parent = null;
        }
    }

    void PlayerSetCube(int parentMax, float force)
    {
        move = GetComponent<Move>();
        move.SpeedFast();

        int caseNum = 1;

        totalHight = totalHight - 3;

        GameObject cube = objectParent.transform.GetChild(parentMax - 1).gameObject;
        if (force >= 2)
            caseNum = 2;     
        else
            caseNum = 1;

        //Debug.Log(force);

        Rigidbody Rb = cube.GetComponent<Rigidbody>();

        Rb.constraints = RigidbodyConstraints.None;
        Rb.useGravity = true;

        cube.transform.position = SpawnPoint.position;
        cube.transform.parent = null;

        cube.GetComponent<Bullet>().isAttacking = true;

        cube.GetComponent<ObjectRotation>().inBox = false;
        cube.AddComponent<ObjectDamage>();
        cube.GetComponent<ObjectDamage>().SetDamage(caseNum);
        cube.GetComponent<ObjectDamage>().chip = chip;
        cube.GetComponent<ObjectDamage>().Direction= direction.transform.forward;
        //Rb.AddForceAtPosition(direction.transform.forward * 3500f * 100 * Time.deltaTime, cube.transform.position, ForceMode.Impulse);
    }

    IEnumerator TheBigOne(int parentMax, float force)
    {      
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
