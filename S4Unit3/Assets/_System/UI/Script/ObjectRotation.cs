using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{

    [Header("自轉公轉")]
    [Tooltip("This is only for debug testing, don't touch it.")]
    public bool _isInCount;

    public GameObject target;

    private float step = 180;
    private float own = 20;

    private float distance;

    Vector3 dir;

    void Start()
    {
        if(target)
           dir = transform.position - target.transform.position;

        distance = Vector3.Distance(transform.position, target.transform.position);
    }

    void Update()
    {
        if (_isInCount)
        {
            transform.localScale = new Vector3(1f,1f,1f);
            //Debug.Log(distance);

            transform.position = target.transform.position + dir.normalized * distance;

            Vector3 targetPos = target.transform.position; 

            transform.RotateAround(targetPos, Vector3.up, step * Time.deltaTime);

            dir = transform.position - target.transform.position;

            //自轉
            transform.Rotate(new Vector3(0, -own * Time.deltaTime, 0));
        }
        else
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}
