using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_PlamCharge : MonoBehaviour
{
    [Header("集氣時開啟")]
    public bool IsCollecting = false;

    public ParticleSystem vfx;
    public ParticleSystem BiggerEffect;
    [Range(0, 1)][Header("集氣程度0~1")]
    public float value;
    float[] segment = { 0, 0.33f, 0.67f, 1 }; //1切3
    [Range(0, 1)]
    public float LerpSpeed = 0.1f;



    [Header("紀錄數值用")]
    [SerializeField]
    Vector3 OSize;
    [SerializeField]
    Vector3 ExSize;
    [SerializeField]
    int ExState;


    private void Awake()
    {
        vfx = (vfx == null ? gameObject.GetComponent<ParticleSystem>() : vfx);
        OSize = transform.localScale;
    }





    private void Update()
    {
        if (vfx != null&& IsCollecting)
        {
            if (!vfx.isPlaying)
                vfx.Play();

            switch (Charge(value))
            {
                case 1:
                    {
                        ExSize = transform.localScale;
                        if (ExState < Charge(value))//is bigger
                        {
                            for (int i = 0; i <= Mathf.FloorToInt(1 / LerpSpeed); i++)
                            {
                                transform.localScale = Vector3.Lerp(transform.localScale, OSize, LerpSpeed);
                            }
                            ExSize = transform.localScale;
                            StartCoroutine(biggerEffect(3.75f));
                        }
                        else if (ExState >= Charge(value))//is smaller
                        {
                            transform.localScale = Vector3.Lerp(transform.localScale, OSize, LerpSpeed);
                            ExSize = transform.localScale;
                        }
                        else //not change
                        {

                        }
                        ExState = Charge(value);
                    }
                    break;
                case 2:
                    {
                        if (ExState < Charge(value))//is bigger
                        {
                            for (int i = 0; i <= Mathf.FloorToInt(1 / LerpSpeed); i++)
                            {
                                transform.localScale = Vector3.Lerp(transform.localScale, Vector3amplifier(OSize, 1.5f), LerpSpeed);
                            }
                            ExSize = transform.localScale;
                            StartCoroutine(biggerEffect(3.75f));
                        }
                        else if (ExState >= Charge(value))//is smaller
                        {
                            transform.localScale = Vector3.Lerp(transform.localScale, ExSize, LerpSpeed);
                            ExSize = transform.localScale;
                        }
                        else //not change
                        {

                        }
                        ExState = Charge(value);
                    }
                    break;

                case 3:
                    {
                        if (ExState < Charge(value))//is bigger
                        {
                            for (int i = 0; i <= Mathf.FloorToInt(1 / LerpSpeed); i++)
                            {
                                transform.localScale = Vector3.Lerp(transform.localScale, Vector3amplifier(OSize, 2.5f), LerpSpeed);
                            }
                            ExSize = transform.localScale;
                            StartCoroutine(biggerEffect(3.75f));
                        }
                        else if (ExState >= Charge(value))//is smaller
                        {
                            transform.localScale = Vector3.Lerp(transform.localScale, ExSize, LerpSpeed);
                            ExSize = transform.localScale;
                        }
                        else //not change
                        {

                        }
                        ExState = Charge(value);
                    }
                    break;

                case 4:
                    {

                        if (ExState < Charge(value))//is bigger
                        {
                            for (int i = 0; i <= Mathf.FloorToInt(1 / LerpSpeed); i++)
                            {
                                transform.localScale = Vector3.Lerp(transform.localScale, Vector3amplifier(OSize, 3.75f), LerpSpeed);

                            }
                            ExSize = transform.localScale;
                            StartCoroutine(biggerEffect(3.75f));
                        }
                        else if (ExState > Charge(value))//is smaller
                        {
                            transform.localScale = Vector3.Lerp(transform.localScale, ExSize, LerpSpeed);
                        }
                        else //not change
                        {

                        }
                        ExState = Charge(value);
                    }
                    break;

                case 0:
                    {
                        print("value Error");
                    }
                    break;

            }


        }
    }


    public int Charge(float value)
    {
        if (value >= segment[0] && value < segment[1])  //0~0.32999...
        {
            return 1;
        }
        else if (value >= segment[1] && value < segment[2])//0.33~0.66999...
        {
            return 2;

        }
        else if (value >= segment[2] && value < segment[3])//0.67~0.999
        {
            return 3;
        }
        else if (value >= segment[3])//1
        {
            return 4;
        }
        else
        {
            return 0;
        }

    }
    Vector3 Vector3amplifier(Vector3 O, float magnification)
    {
        return new Vector3(O.x * magnification, O.y * magnification, O.z * magnification);
    }

    IEnumerator biggerEffect(float size)
    {
        if (BiggerEffect != null)
        {
            yield return new WaitForSeconds(.3f);
            ParticleSystem newvfx = Instantiate(BiggerEffect, transform.position,transform.rotation, transform);
            newvfx.gameObject.transform.localScale = Vector3amplifier(OSize, size);
            var vfxLookAt =  newvfx.gameObject.AddComponent<Particle_LookAt>();
            vfxLookAt.GetComponent<Particle_LookAt>().targetStr = "Boss";

            yield return new WaitForSeconds(3f);
            Destroy(newvfx.gameObject);
        }
    }

}
