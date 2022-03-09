using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTotalDead : MonoBehaviour
{
    [SerializeField] UIcontrol UIcontrol;
    public int TotalLife = 3 ;
    public bool isTotalLifeOn ;
    // Start is called before the first frame update

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Tab))
        {          
            isTotalLifeOn = !isTotalLifeOn;
            UIcontrol.TotalDeadCounterControl(isTotalLifeOn);
        }        
    }

    public void TotalLifeDecrease()
    {
        if(isTotalLifeOn)
        {
            TotalLife--;
            UIcontrol.TotalDead(TotalLife);
            //Debug.Log("TotalLife:"+TotalLife);
            //UIcontrol.TotalDead(TotalLife);
            if (TotalLife < 0)
                UIcontrol.GameOver();
        }       
    }
    public void TotalLifeIncrease()
    {
        TotalLife++;
        UIcontrol.TotalDead(TotalLife);
    }
}
