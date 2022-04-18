using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTraget : MonoBehaviour
{
    [Header("目標物件給他")]
    public Transform m_objectTraget = null;

    private Material m_materialRef= null;
    private Renderer m_renderer = null;

    public Renderer Renderer
    {
        get
            {
            if(m_renderer == null)
              m_renderer = this.GetComponent<Renderer>();

            return m_renderer; 
        }
    }
    public Material MaterialRef
    {
        get
        {
            if (m_materialRef == null)
                m_materialRef = Renderer.material;

            return m_materialRef;
        }
    }


    private void Awake()
    {
        m_renderer = this.GetComponent<Renderer>();
        m_materialRef = m_renderer.material;
    }


    private void Update()
    {
        if(m_objectTraget!= null)
        {
            m_materialRef.SetVector("_Position",m_objectTraget.position);
          //  print(m_objectTraget.position);
        }
    }

    private void OnDestroy()
    {
        m_renderer = null;
        if (m_materialRef != null)
            Destroy(m_materialRef);

        m_materialRef = null;
    }

    public void printTraget ()
    {
      //  print(m_objectTraget);
    }
}
