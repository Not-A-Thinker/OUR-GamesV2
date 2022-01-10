using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField][Header("幫我在攻擊的時候打開")]
    public bool isAttacking = false;
    public GameObject tailParticle;
    public Quaternion dir;
    void Start()
    {
        dir = dir == null ? Quaternion.identity : dir;
        tailParticle.transform.rotation = dir;
        tailParticle.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking)
        {
            tailParticle.SetActive(true);
        }
    }

   // public Transform explosionPrefab;
    public GameObject hitParticle;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("collision with " + collision.gameObject.name);
    //    if(collision.transform.gameObject.tag=="Boss")
    //    {
    //        ContactPoint contact = collision.contacts[0]; //get hit point 
    //        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // rotation
    //        Vector3 pos = contact.point;//position

    //        //Instantiate(explosionPrefab, pos, rot);
    //        GameObject particleobj = Instantiate(hitParticle, pos, Quaternion.identity);
    //        Destroy(particleobj, 5);
    //    }
    //  //  Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            if(isAttacking)
            {
                GameObject particleobj = Instantiate(hitParticle, transform.position, Quaternion.identity);

                Destroy(particleobj, 5);
            }
        }
    }


}
