using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPortal : MonoBehaviour
{
    public DistanceTraget distanceTraget;
    public Transform m_traget;
    private void Awake()
    {
        if (distanceTraget == null)
        {
            foreach (Transform child in transform)
            {
                print(child.gameObject.name);
                if (child.GetComponent<DistanceTraget>() != null)
                    distanceTraget = child.GetComponent<DistanceTraget>();
            }
        }
        else
        {
            if (distanceTraget.m_objectTraget == null)
            {
                distanceTraget.m_objectTraget = m_traget;
            }
        }
    }

    public void setTraget(Transform traget)
    {
        if (m_traget != null&& m_traget!= traget)
        {
            m_traget.gameObject.SetActive(false);
            Destroy(m_traget);
            Debug.Log("reSetTraget");
        }
        else
        {
            m_traget = traget;
            distanceTraget.m_objectTraget = m_traget;
        }
        Debug.Log("setTraget");
    }
    private void Update()
    {
        distanceTraget.m_objectTraget = m_traget;
    }

}
