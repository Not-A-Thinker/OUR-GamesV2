using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Particle_Animator : MonoBehaviour
{
    [SerializeField]
    private Animator bossAni;
    private void Awake()
    {
        bossAni = gameObject.GetComponent<Animator>();
        if (bossAni == null)
            return;
        foreach (ParticleSystem eyesPar in bossEyesLight)
        {
            eyesPar.playOnAwake=false;
            eyesPar.gameObject.SetActive(false);
        }
        foreach (ParticleSystem linePar in bossWingSpeedLine)
        {
            linePar.playOnAwake=false;
            linePar.gameObject.SetActive(false);
        }
    }


    public ParticleSystem[] bossEyesLight;
    public void Eyes_Light_On ()
    {
        foreach (ParticleSystem eyesPar in bossEyesLight)
        {
            eyesPar.gameObject.SetActive(true);
            eyesPar.Play();
        }
    }

    public void Eyes_Light_Off()
    {
        foreach (ParticleSystem eyesPar in bossEyesLight)
        {
            eyesPar.Stop();
            eyesPar.gameObject.SetActive(false);
        }
    }

    public ParticleSystem[] bossWingSpeedLine;
    public void Wing_Line_On()
    {
        foreach (ParticleSystem linePar in bossWingSpeedLine)
        {
            linePar.gameObject.SetActive(true);
            linePar.Play();
        }
    }

    public void Wing_Line_Off()
    {
        foreach (ParticleSystem linePar in bossWingSpeedLine)
        {
            linePar.Stop();
            linePar.gameObject.SetActive(false);
        }
    }


}
