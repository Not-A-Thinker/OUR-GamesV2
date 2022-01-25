using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VacuumCleaner : MonoBehaviour
{
    /*public GameObject core_G;
    public ParticleSystem core_P;*/
    public float radius = 10;
    public float maxDistance;
    public float suckPower = -60f;

    public LayerMask layerMask;
    private Vector3 origin;
    private Vector3 direction;
    public WindZone windzone;   // wind zone 粒子  // script  粒子本身位置
    public SphereCollider sphereCollider;

    public VisualEffect Vfx;

    // Start is called before the first frame update
    void Start()
    {
        if(windzone==null)
            windzone = gameObject.GetComponent<WindZone>();
        if (sphereCollider == null)
            sphereCollider = gameObject.GetComponent<SphereCollider>();

        sphereCollider.isTrigger = true ;
        sphereCollider.radius = radius;
        windzone.radius = radius;
        windzone.windMain = suckPower;

    }
    // Update is called once per frame
    void Update()
    {
        sphereCollider.isTrigger = true;
        sphereCollider.radius = radius;
        windzone.radius = radius;
        windzone.windMain = suckPower;


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<VisualEffect>())
        {
            print("Vfx");
        }
        if (other.transform.tag == "suckObj"||other.transform.gameObject.layer==layerMask)
        {
            print("getObj");
            if(other.GetComponent<ParticleSystem>())
            {
                print("getParticle");
                GameObject ps = other.transform.gameObject;
                if(ps.GetComponent<Rigidbody>())
                  ps.GetComponent<Rigidbody>().AddForce(Vector3.forward*10);
            }
        }
        print(other.transform.name);
    }




    void OnDrawGizmosSelected() //宣告範圍
    {
        Gizmos.color = Color.red; //範圍顏色
        Gizmos.DrawWireSphere(transform.position, radius); //範圍pos,大小 
    }
}
