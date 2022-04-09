using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_PlamCharge : MonoBehaviour
{
    public ParticleSystem vfx;
    public ParticleSystem BiggerEffect; 
    [Range(0, 1)][SerializeField]
    private float value;
    float[] segment = { 0, 0.33f, 0.67f, 1 }; //1¤Á3
    public float LerpSpeed = 0.01f;


    private void Awake()
    {
        vfx = (vfx == null ? gameObject.GetComponent<ParticleSystem>() : vfx);
        OSize = transform.localScale; 
    }
    Vector3 OSize; 
    private void Update()
    {
        if(vfx!=null)
        {
            switch (Charge(value))
            {
                case 1:
                    {
                        transform.localScale = Vector3.Lerp(transform.localScale, OSize, LerpSpeed);
                    }
                    break;
                case 2:
                    {
                        if (transform.localScale.x < Vector3amplifier(OSize, 1.5f).x) // is bigger
                        {
                            if(BiggerEffect!=null)
                            {
                               ParticleSystem newvfx = Instantiate(BiggerEffect, transform.position, Quaternion.identity);
                                newvfx.gameObject.transform.localScale = Vector3amplifier(OSize, 1.5f);
                                Destroy(newvfx, 1);

                            }
                        }

                            transform.localScale = Vector3.Lerp(transform.localScale, Vector3amplifier(OSize,1.5f), LerpSpeed);
                    }
                    break;
                    
                case 3:
                    {
                        if (transform.localScale.x < Vector3amplifier(OSize, 2.5f).x) // is bigger
                        {
                            if (BiggerEffect != null)
                            {
                                ParticleSystem newvfx = Instantiate(BiggerEffect, transform.position, Quaternion.identity);
                                newvfx.gameObject.transform.localScale = Vector3amplifier(OSize, 2.5f);
                                Destroy(newvfx, 1);

                            }
                        }
                        transform.localScale = Vector3.Lerp(transform.localScale, Vector3amplifier(OSize, 2.5f), LerpSpeed);
                    }
                    break;
                    
                case 4:
                    {
                        if (transform.localScale.x < Vector3amplifier(OSize, 3.75f).x) // is bigger
                        {
                            if (BiggerEffect != null)
                            {
                                ParticleSystem newvfx = Instantiate(BiggerEffect, transform.position, Quaternion.identity);
                                newvfx.gameObject.transform.localScale = Vector3amplifier(OSize, 3.75f);
                                Destroy(newvfx, 1);

                            }
                        }
                        transform.localScale = Vector3.Lerp(transform.localScale, Vector3amplifier(OSize, 3.75f), LerpSpeed);
                    }
                    break;
                    
                case 0:
                    {
                        print("value Error");
                    }break;
                    
            }

        }
    }


    public int Charge(float value)
    {
        if (value >= segment[0] && value < segment[1])
        {
            return 1;
        }
        else if (value >= segment[1] && value < segment[2])
        {
            return 2;

        }
        else if (value >= segment[2] && value < segment[3])
        {
            return 3;
        }
        else if (value >= segment[3] && value <= 1)
        {
            return 4;
        }
        else
        {
            return 0;
        }

    }
    Vector3 Vector3amplifier( Vector3 O , float magnification)
    {
        return new Vector3(O.x * magnification, O.y * magnification, O.z * magnification);
    }

}
