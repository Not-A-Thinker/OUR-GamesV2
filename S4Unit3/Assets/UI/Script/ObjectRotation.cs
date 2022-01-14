using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{

    [Header("自轉公轉")]
    public bool inBox;

    public GameObject target;

    private float step = 50;
    private float own = 20;

    private float distance;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = transform.position - target.transform.position;

        distance = Vector3.Distance(transform.position, target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (inBox)
        {
            transform.position = target.transform.position + dir.normalized * distance;

            Vector3 targetPos = new Vector3(target.transform.position.x, -2, target.transform.position.z); 

            transform.RotateAround(targetPos, Vector3.up, step * Time.deltaTime);

            dir = transform.position - target.transform.position;

            //自轉
            transform.Rotate(new Vector3(0, -own, 0));
        }
    }
}
