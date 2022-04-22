using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_LookAt : MonoBehaviour
{
    ParticleSystem par;
    [Header("���V�ؼЪ�")]
    public Transform target;
    [Header("���V�ؼЪ�")]
    public string targetStr;
    // Start is called before the first frame update
    void Start()
    {
        par = gameObject.GetComponent<ParticleSystem>();
        if (target == null)
        {
            target = GameObject.Find(targetStr).gameObject.transform;
        }
        // gameObject.transform.LookAt(target);

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void Awake()
    {
        if (target == null)
        {
            target = GameObject.Find(targetStr).gameObject.transform;
        }
        gameObject.transform.LookAt(target);

    }

    public void ParticleLookAt()
    {
        if (target == null)
        {
            target = GameObject.Find(targetStr).gameObject.transform;

        }
        gameObject.transform.LookAt(target);

    }
}
