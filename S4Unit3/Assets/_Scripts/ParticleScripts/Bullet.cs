using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    [Header("���ڦb�������ɭԥ��}")]
    public bool isAttacking = false;

    [Header("�qboss���W�l�����ɭԥ��}")]
    public bool bossToSuck = false;

    public GameObject tailParticle;
    private Quaternion dir; //face direction

    [Header("�����ؼ�(Boss)")]
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
        //print("collision with " + collision.gameObject.name);
        if (collision.transform.gameObject == Traget || collision.transform.gameObject.tag == "Boss")
         {

            ContactPoint contact = collision.contacts[0]; //get hit point 
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // rotation
            Vector3 pos = contact.point;//position
            if (!bossToSuck)
            {
                GameObject particleobj = Instantiate(CollisionParticle, pos, Quaternion.identity);
                Destroy(particleobj, 5);
            }
            //Instantiate(explosionPrefab, pos, rot);

        }

        ///�o���destroy�ڥ��������M�ڧl���_��
         //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        //print(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        //Instantiate(explosionPrefab, pos, Quaternion.identity);

        if (other.gameObject.tag== "Boss" || other.gameObject==Traget)
        {
            if(!bossToSuck)
            {
                GameObject particleobj = Instantiate(hitParticle, transform.position, Quaternion.identity);
                Destroy(particleobj, 5);
            }
            //Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        }

        //Destroy(gameObject);

    }


}


