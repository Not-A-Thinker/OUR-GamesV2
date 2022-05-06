using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_LookAt : MonoBehaviour
{
    ParticleSystem par;
    [Header("面向目標物")]
    public Transform target;
    [Header("面向目標物")]
    public string targetStr;
    public bool anyTime = false;
    // Start is called before the first frame update
    void Start()
    {
        par = gameObject.GetComponent<ParticleSystem>();
        if (target == null)
        {
            target = GameObject.Find(targetStr).gameObject.transform;
        }
        gameObject.transform.LookAt(target);

    }

    // Update is called once per frame
    void Update()
    {
        if(anyTime)
        {
            ParticleLookAt();
        }
    }
    //private void Awake()
    //{
    //    if (target == null)
    //    {
    //        target = GameObject.Find(targetStr).gameObject.transform;
    //    }
    //    gameObject.transform.LookAt(target);
    //}

    public void ParticleLookAt()
    {
        if (target == null)
        {
            target = GameObject.Find(targetStr).gameObject.transform;

        }
        gameObject.transform.LookAt(target);

    }
}
