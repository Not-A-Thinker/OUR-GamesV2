using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnObject : MonoBehaviour
{
    [SerializeField]
    GameObject Object;

    [SerializeField] BossHealthBar bossHealth;

    public GameObject lastSpawned;

    [SerializeField] int SpawnedCount;

    private void FixedUpdate()
    {
        SpawnedCount = GameObject.FindGameObjectsWithTag("Object").Length;
    }

    public void ObjectSpawn(Vector3 P2Pos)
    {
        if (SpawnedCount < 5)
        {
            lastSpawned = Instantiate(Object, P2Pos, Quaternion.identity);
            bossHealth.TakeDamage(5);
        }
        //Debug.Log(P2Pos);
    }

    public void SpawnedCountDecrease()
    {
        //SpawnedCount--;
    }
    
    // Start is called before the first frame update
  
}
