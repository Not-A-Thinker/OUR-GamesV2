using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isSucked=false;
    public float DestroyTime=15;

    private void Update()
    {
        if (!isSucked)
            Invoke("DestroyThis", DestroyTime);
        else
            CancelInvoke("DestroyThis");
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall" && !isSucked)           
            DestroyThis();
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
        BossSpawnObject bossSpawn = GameObject.Find("Boss").GetComponent<BossSpawnObject>();
        bossSpawn.SpawnedCountDecrease();
        Destroy(gameObject);
    }
}
