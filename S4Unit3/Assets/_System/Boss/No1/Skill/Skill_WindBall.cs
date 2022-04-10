using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindBall : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float _fallingSpeed = 3f;

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
    }

    private void OnDestroy()
    {
        Debug.Log("Hi.");
    }
}
