using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindBall : MonoBehaviour
{
    //Rigidbody rb;
    //[SerializeField] float _fallingSpeed = 3f;

    PlayerState playerState;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //rb.velocity = new Vector3(transform.position.x, -1, transform.position.z) * _fallingSpeed;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Ground")
        {
            Destroy(gameObject, .3f);
        }
        if (col.transform.tag == "Wall")
        {
            Destroy(gameObject, .1f);
        }
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = col.gameObject.GetComponent<PlayerState>();
                playerState.hp_decrease();

                Destroy(gameObject);
            }

        }
    }

    private void OnDestroy()
    {
        
    }
}
