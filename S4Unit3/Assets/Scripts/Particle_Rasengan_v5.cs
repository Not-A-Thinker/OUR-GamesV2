using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Rasengan_v5 : MonoBehaviour
{
    public GameObject particlePer;
    public float poolRang;
    public float DestroyTime = 1;
    public Transform poolMaster;
    private int poolObj;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCor());
    }
    private float RandomFloat (float inN)
    {     
        return (Random.Range(0,100))*inN/100;
    }
    private bool RandomBool()
    {
      int ii=  Random.Range(0, 1);
        return ii != 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        poolObj = poolMaster.childCount;
       // print(poolObj);
        if(poolObj<poolRang)
        {
            //print(RandomFloat(360));
            GameObject obj = Instantiate(particlePer, transform.position, Quaternion.Euler(new Vector3(RandomFloat(360), RandomFloat(360), RandomFloat(360))), poolMaster);
            if (obj.GetComponent<rotate>() == null) obj.AddComponent<rotate>();

            Destroy(obj, DestroyTime + (RandomFloat(1) * (RandomBool() ? 1 : -1f)));
        }
    }

    IEnumerator StartCor ()
    {
        for (int ii = 0; ii <= poolRang; ii++)
        {

            GameObject obj = Instantiate(particlePer, transform.position, Quaternion.Euler(new Vector3(RandomFloat(360), RandomFloat(360), RandomFloat(360))), poolMaster);
            if (obj.GetComponent<rotate>() == null) obj.AddComponent<rotate>();

            Destroy(obj, DestroyTime + (RandomFloat(1) * (RandomBool() ? 1 : -1f)));
            yield return new WaitForSeconds(RandomFloat(1));

        }
    }
}
