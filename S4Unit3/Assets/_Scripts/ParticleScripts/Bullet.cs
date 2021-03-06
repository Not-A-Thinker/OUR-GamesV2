using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    [Header("幫我在攻擊的時候打開")]
    public bool isAttacking = false;
    public ParticleSystem particle_palm;
    public ParticleSystem specialParticle;


    [Header("從boss身上吸取的時候打開")]
    public bool bossToSuck = false;

    //public GameObject tailParticle;
    private Quaternion dir; //face direction

    [Header("攻擊目標(Boss)")]
    public GameObject Traget;
    void Start()
    {
        dir = dir == null ? Quaternion.identity : dir;
        //tailParticle.transform.rotation = dir;
        //tailParticle.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            //tailParticle.SetActive(true);
            if(!particle_palm.isPlaying)
              particle_palm.Play();
            //  Destroy(particle_palm, 1);
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            if( particle_palm.GetComponent<Particle_LookAt>()!=null)
            {
                particle_palm.GetComponent<Particle_LookAt>().ParticleLookAt();
            }
           if( particle_palm.gameObject.GetComponent<Particle_LookAt>()!=null)
           {
                particle_palm.gameObject.GetComponent<Particle_LookAt>().ParticleLookAt();
           }
        }
        if(specialParticle!=null&& !specialParticle.isPlaying)
        {
            specialParticle.Play();
        }
        if(Level1GameData.b_isBossDeathCutScene)
        {
            Destroy(gameObject);
        }
    }

    public Transform explosionPrefab;
    public GameObject hitParticle;
    public GameObject CollisionParticle;

    private void OnCollisionEnter(Collision collision)
    {
        //print("collision with " + collision.gameObject.name);
        if (collision.transform.gameObject == Traget || collision.transform.gameObject.tag == "Boss" || collision.transform.gameObject.tag == "DummyBoss")
         {
            print("Collision");
            ContactPoint contact = collision.contacts[0]; //get hit point 
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // rotation
            Vector3 pos = contact.point;//position
            //if (!bossToSuck) 2022/4/18
            //{
            //    print("Collision // !toBoss");

            //    GameObject particleobj = Instantiate(CollisionParticle, pos, Quaternion.identity);
            //    Destroy(particleobj, 5);
            //}
            //Instantiate(explosionPrefab, pos, rot);

        }

        ///這兩個destroy我先關掉不然我吸不起來
         //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        //print(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        //Instantiate(explosionPrefab, pos, Quaternion.identity);

        if (other.gameObject.tag== "Boss" || other.gameObject== Traget || other.transform.gameObject.tag == "DummyBoss")
        {
            //print("trigger");
            if (!bossToSuck)// 2022 / 04 / 18
            {
                //print("trigger // !toBoss");

                GameObject particleobj = Instantiate(hitParticle, transform.position, Quaternion.identity);
                if(particle_palm.gameObject!=null)
                 particleobj.transform.localScale = particle_palm.transform.localScale;
                if (transform.GetChild(0).gameObject.GetComponent<Particle_PlamCharge>() != null)
                    particleobj.GetComponent<ParticleSystem>().startColor = transform.GetChild(0).gameObject.GetComponent<Particle_PlamCharge>().NowColor;
                Destroy(particleobj, 5);
            }
            Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        }
        if(other.gameObject.tag=="")
        {

        }

        //Destroy(gameObject);

    }


}



