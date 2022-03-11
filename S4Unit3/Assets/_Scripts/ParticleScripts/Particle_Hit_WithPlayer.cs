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
        if(collision.gameObject.tag=="Player" )
        {
            GameObject particleobj = Instantiate(hitParticle, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(particleobj, 3);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            Staying = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject particleobj = Instantiate(hitParticle, other.gameObject.transform.position, Quaternion.identity);
            Destroy(particleobj, 3);
            StartCoroutine(attackIsStaing(other.gameObject.transform.position));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            Staying = true;
        }
    }

    IEnumerator attackIsStaing(Vector3 pos )
    {
        GameObject particleobj = Instantiate(hitParticle, pos, Quaternion.identity);
        Destroy(particleobj, 3);
        yield return new WaitForSeconds(.1f);
        if(Staying)
        {
            StartCoroutine(attackIsStaing(pos));
        }
    }

}
