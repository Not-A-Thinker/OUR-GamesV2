using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isSucked=false;
    public float DestroyTime = 15;
    bool isDestroy;

    private void Update()
    {
        if (!isSucked)
            Invoke("DestroyThis", DestroyTime);
        else
            CancelInvoke("DestroyThis");

        if (Level1GameData.b_isBossDeathCutScene)
        {
            DestroyThis();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!isSucked)
        {
           if(col.gameObject.tag == "Wall" || col.gameObject.tag == "")
                DestroyThis();
        }        
    }

    //private void OnCollisionStay(Collision col)
    //{
    //    if (col.gameObject.name == "Floor")
    //        isGround = true;
    //    //else
    //    //    isGround = false;
    //}

    //private void OnCollisionExit(Collision col)
    //{
    //    if (col.gameObject.name == "Floor")
    //        isGround = false;
    //}

    private void DestroyThis()
    {
        if(!isDestroy)
        {
            BossSpawnObject bossSpawn = GameObject.Find("Boss").GetComponent<BossSpawnObject>();
            bossSpawn.SpawnedCountDecrease();
            isDestroy = true;
        }  
        Destroy(gameObject);
    }
}
