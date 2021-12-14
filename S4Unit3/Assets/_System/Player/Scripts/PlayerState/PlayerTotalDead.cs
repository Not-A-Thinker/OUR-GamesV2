using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTotalDead : MonoBehaviour
{
    [SerializeField] UIcontrol UIcontrol;
    public int TotalLife =3 ;
    // Start is called before the first frame update


    public void TotalLifeDecrease()
    {
        TotalLife--;
        UIcontrol.TotalDead(TotalLife);
        //Debug.Log("TotalLife:"+TotalLife);
        //UIcontrol.TotalDead(TotalLife);
        if (TotalLife < 0)
            UIcontrol.GameOver();
    }
    public void TotalLifeIncrease()
    {
        TotalLife++;
        UIcontrol.TotalDead(TotalLife);
    }
}
