using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindBall : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Ground")
        {
            Destroy(gameObject, 1f);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Hi.");
    }
}
