using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPortal : MonoBehaviour
{
    public DistanceTraget distanceTraget;
    public GameObject distanceTragetObj;

    private void Update()
    {
        if(distanceTragetObj == null)
        {
            if (distanceTraget == null)
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject.GetComponent<DistanceTraget>() != null)
                        distanceTraget = child.gameObject.GetComponent<DistanceTraget>();
                }
            }
            distanceTragetObj = distanceTraget.gameObject;
        }
    }



}
