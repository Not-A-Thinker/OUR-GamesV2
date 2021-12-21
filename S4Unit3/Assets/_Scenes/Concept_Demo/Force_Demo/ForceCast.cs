using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCast : MonoBehaviour
{
    public float _force = 100f;
    public float _range = 100f;

    public Camera Cam;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, _range))
        {
            Debug.Log(hit.transform.name + "." + hit.transform.tag);

            if (hit.transform.tag == "Object")
            {
                hit.rigidbody.AddForceAtPosition(Cam.transform.forward * _force , hit.transform.position, ForceMode.Impulse);
            }
        }
        
    }
}
