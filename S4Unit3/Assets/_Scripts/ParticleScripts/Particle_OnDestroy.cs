using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_OnDestroy : MonoBehaviour
{
    public ParticleSystem OnDestroy_Particle;
    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void onParticleDestory (Vector3 pos)
    {
        if(OnDestroy_Particle!=null)
        {
            var opd = Instantiate(OnDestroy_Particle, pos, Quaternion.identity);
            Destroy(opd.gameObject, 2);
        }
    }
}
