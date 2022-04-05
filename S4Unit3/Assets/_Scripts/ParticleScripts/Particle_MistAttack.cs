using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_MistAttack : MonoBehaviour
{
    public ParticleSystem particle_mistAttack;
    float timeAlive;
    // Start is called before the first frame update
    void Start()
    {
        particle_mistAttack = particle_mistAttack == null ? gameObject.GetComponent<ParticleSystem>(): particle_mistAttack;
        // Calculate how long the particle has been alive.
        timeAlive = particle_mistAttack.duration * particle_mistAttack.startLifetime;
        print(timeAlive);
        Destroy(gameObject, timeAlive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
