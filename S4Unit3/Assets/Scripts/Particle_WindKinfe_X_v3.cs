using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_WindKinfe_X_v3 : MonoBehaviour
{
    public Color windKinfe_Color;
    public Color windPoint_Color;
    public ParticleSystem WindKinfe_1, WindKinfe_2;
    public ParticleSystem Point_1, Point_2, Point_3, Point_4;
    [Range(-10f,10f)]
    public float intansty;

    bool flyIng = false;
    public Vector3 rotateRange;
    public float rotateSpeed = 1;    // Start is called before the first frame update

    void Start()
    {
        float hdr = Mathf.Pow(2, intansty);
        windKinfe_Color = new Color(windKinfe_Color.r*hdr, windKinfe_Color.g*hdr, windKinfe_Color.b*hdr);
        WindKinfe_1.startColor = windKinfe_Color;
        WindKinfe_2.startColor = windKinfe_Color;
        windKinfe_Color = new Color(windPoint_Color.r * hdr, windPoint_Color.g * hdr, windPoint_Color.b * hdr);
        Point_1.startColor = windPoint_Color;
        Point_2.startColor = windPoint_Color;
        Point_3.startColor = windPoint_Color;
        Point_4.startColor = windPoint_Color;

    }
    public string DestroyTag; 
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateRange * Time.deltaTime * rotateSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag=="Ground"|| other.transform.tag == DestroyTag)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }

}
