using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_WindBall : MonoBehaviour
{
    public GameObject particlePer;
    public Color particleColor;
    [Range(-10f,10f)]
    public float intansty;
    public float poolRang;
    public float DestroyTime = 1;
    public Transform poolMaster;
    private int poolObj;
    public Vector3 RandomRotRange;  
    // Start is called before the first frame update
    void Start()
    {
        var ps =  particlePer.GetComponent<ParticleSystem>().main;
        float hdr = Mathf.Pow(2, intansty);
        ps.startColor = new Color (particleColor.r* hdr,particleColor.g* hdr,particleColor.b* hdr,particleColor.a);
        StartCoroutine(StartCor());
    }
    private float RandomFloat(float inN)
    {
        return (Random.Range(0, 100)) * inN / 100;
    }
    private bool RandomBool()
    {
        int ii = Random.Range(0, 1);
        return ii != 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        poolObj = poolMaster.childCount;
        if (poolObj < poolRang)
        {
            //print(RandomFloat(360));
            GameObject obj = Instantiate(particlePer, transform.position, Quaternion.Euler(new Vector3(RandomFloat(RandomRotRange.x), RandomFloat(RandomRotRange.y), RandomFloat(RandomRotRange.z))), poolMaster);
            if (obj.GetComponent<rotate>() == null) obj.AddComponent<rotate>();

            Destroy(obj, DestroyTime + (RandomFloat(1) * (RandomBool() ? 1 : -1f)));
        }
    }

    IEnumerator StartCor()
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
