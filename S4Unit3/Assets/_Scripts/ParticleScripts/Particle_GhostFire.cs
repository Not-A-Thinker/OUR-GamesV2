using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_GhostFire : MonoBehaviour
{
    //public Vector3 rotateRange;
    //public float rotateSpeed = 1;
    public float radian = 0;
    public float preRadian = 0.01f;
    public float radius = 0.8f;
    Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        radian += preRadian;
        float dy = Mathf.Cos(radian) * radius;
        transform.position =  new Vector3(transform.position.x, oldPos.y,transform.position.z) + new Vector3(0,dy,0);
    }
}
