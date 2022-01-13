using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_WindKinfe_X_v3 : MonoBehaviour
{
    public Color main_Color; 
    public ParticleSystem WindKinfe_1, WindKinfe_2;
    public ParticleSystem Point_1, Point_2, Point_3, Point_4;

    bool flyIng = false;
    public Vector3 rotateRange;
    public float rotateSpeed = 1;    // Start is called before the first frame update

    void Start()
    {
        WindKinfe_1.startColor = main_Color;
        WindKinfe_2.startColor = main_Color;
        Point_1.startColor = main_Color;
        Point_2.startColor = main_Color;
        Point_3.startColor = main_Color;
        Point_4.startColor = main_Color;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateRange * Time.deltaTime * rotateSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag=="Ground")
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
    }
}
