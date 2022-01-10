using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    [Header("幫我在攻擊的時候打開")]
    public bool isAttacking = false;
    public GameObject tailParticle;
    public Quaternion dir;
    [Header("攻擊目標(Boss)")]
    public GameObject Traget;
    void Start()
    {
        dir = dir == null ? Quaternion.identity : dir;
        tailParticle.transform.rotation = dir;
        tailParticle.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            tailParticle.SetActive(true);
        }
    }

    public Transform explosionPrefab;
    public GameObject hitParticle;
    public GameObject CollisionParticle;

    private void OnCollisionEnter(Collision collision)
    {
        print("collision with " + collision.gameObject.name);
        if (collision.transform.gameObject == Traget || collision.transform.gameObject.tag == "Boss")
         {
            ContactPoint contact = collision.contacts[0]; //get hit point 
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // rotation
            Vector3 pos = contact.point;//position

            //Instantiate(explosionPrefab, pos, rot);
            GameObject particleobj = Instantiate(CollisionParticle, pos, Quaternion.identity);
            Destroy(particleobj, 5);
        }
          Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        //print(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        //Instantiate(explosionPrefab, pos, Quaternion.identity);

        if (other.gameObject.tag== "Boss" || other.gameObject==Traget)
        {
            //Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            GameObject particleobj = Instantiate(hitParticle, transform.position,Quaternion.identity);

            Destroy(particleobj, 5);
        }
    }


}
