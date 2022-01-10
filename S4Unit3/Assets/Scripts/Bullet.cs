using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject tailParticle;
    public Quaternion dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = dir == null ? Quaternion.identity : dir;
        tailParticle.transform.rotation = dir;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform explosionPrefab;
    public GameObject hitParticle;
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0]; //get hit point 
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // rotation
        Vector3 pos = contact.point;//position

      //Instantiate(explosionPrefab, pos, rot);
        GameObject obj = Instantiate(hitParticle, pos, Quaternion.identity);
        Destroy(obj, 5);
        Destroy(gameObject);
    }


}
