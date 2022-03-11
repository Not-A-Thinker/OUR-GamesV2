using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Hit_WithPlayer : MonoBehaviour
{
    public GameObject hitParticle;

    bool Staying = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player" || collision.gameObject== Traget)
        {
            GameObject particleobj = Instantiate(hitParticle, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(particleobj, 5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject == Traget)
        {
            //print("ddddd");
            GameObject particleobj = Instantiate(hitParticle, other.gameObject.transform.position, Quaternion.identity);
            Destroy(particleobj, 5);
        }
    }
}
