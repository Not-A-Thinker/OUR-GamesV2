using UnityEngine;

public class BossSpawnObject : MonoBehaviour
{
    GameObject Object;

    public BossHealthBar bossHealth;

    public GameObject lastSpawned;

    public int SpawnedCount;

    public int SpawnendMax;

    private void Start()
    {
        if(bossHealth==null)
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
        lastSpawned = Instantiate(Object, P2Pos, SpawnQuat);
        //Debug.Log(lastSpawned.gameObject.name);
        bossHealth.TakeDamage(5);
        SpawnedCount++;
        //Debug.Log(P2Pos);
    }

    public void SpawnedCountDecrease()
    {
        SpawnedCount--;
        //Debug.Log("SpawnGone");
    }

    // Start is called before the first frame update

}
