using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAlighting_ani : MonoBehaviour
{

    public ParticleSystem CatSmoke;
    public void Catalighting()
    {
        CatSmoke.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
