using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnObject : MonoBehaviour
{
    GameObject Object;

    BossHealthBar bossHealth;

    public GameObject lastSpawned;

    [SerializeField] int SpawnedCount;

    private void Start()
    {
        bossHealth = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();
        Object = Resources.Load("Prefabs/Feather Prefab") as GameObject;
    }

    //private void FixedUpdate()
    //{
    //    ///這個可能要做更改, 因為這樣搜索挺用效能的.
    //    SpawnedCount = GameObject.FindGameObjectsWithTag("Object").Length;     
    //}

    public void ObjectSpawn(Vector3 P2Pos,Quaternion SpawnQuat)
    {
        if (SpawnedCount<6)
        {
            lastSpawned = Instantiate(Object, P2Pos, SpawnQuat);
            //Debug.Log(lastSpawned.gameObject.name);
            bossHealth.TakeDamage(5);
            SpawnedCount++;
            //Debug.Log(P2Pos);
        }
    }

    public void SpawnedCountDecrease()
    {
        SpawnedCount--;
    }

    // Start is called before the first frame update

}
