using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    [Header("可以直接從ParticleSystem/collision/collidesWith 調整碰撞對象")]
  

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents=new List<ParticleCollisionEvent>();



    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);


        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {

            if (rb)
            {
                //Vector3 pos = collisionEvents[i].intersection;
                //Vector3 force = collisionEvents[i].velocity * 10;
                //rb.AddForce(force);
                //print(gameObject.name+ " On Collision " + other.gameObject.name);
            }
            i++;
        }
    }
}
