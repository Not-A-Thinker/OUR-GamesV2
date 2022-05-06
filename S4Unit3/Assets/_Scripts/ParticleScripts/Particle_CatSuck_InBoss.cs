using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_CatSuck_InBoss : MonoBehaviour
{
    public GameObject AimLine;
    public bool aa;
    public ParticleSystem m_Particle_CatSuck_InBoss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (AimLine!=null&& m_Particle_CatSuck_InBoss!=null)
        {
            aa = AimLine.activeInHierarchy;
            if (AimLine.activeInHierarchy)
                m_Particle_CatSuck_InBoss.Play();
            else
                m_Particle_CatSuck_InBoss.Stop();

        }
    }
}
