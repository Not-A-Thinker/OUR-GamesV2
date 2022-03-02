using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    BossHealthBar bossHealth;
    BasicState basicState;
    BossSpawnObject bossSpawn;

    bool isSpcecialAttack;

    //碎片
    public GameObject chip;

    //private Rigidbody Rb;

    [Header("Cube Attact State")]
    public Vector3 Direction;
    public int Damage = 15;
    public float Speed = 50;

    private void Start()
    {
        chip = Resources.Load("Prefabs/Clip") as GameObject;
        bossHealth = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();
      
        if (this.gameObject.name.Contains("SpecialAttack"))
            isSpcecialAttack = true;
        ///到指定時間會自己消失
        StartCoroutine(DestroyTimer());

        //Rb = GetComponent<Rigidbody>();
        //Rigidbody Rb = GetComponent<Rigidbody>();
        //Rb.AddForceAtPosition(transform.forward * 2000f *5* Time.deltaTime, transform.position, ForceMode.Impulse);
    }

    private void Update()
    {
        ///方塊移動
        //Rb.AddForceAtPosition(Direction * 2000 * Time.deltaTime, transform.position, ForceMode.Impulse);
        transform.position += Direction * Speed * Time.deltaTime;
        Speed = Speed * 0.8f * Time.deltaTime;
    }

    //Do You Know YourDamage
    public void SetDamage(int DamageType)
    {
        ///特殊攻擊基本傷害70
        if (isSpcecialAttack)
            Damage = Damage + 55;

        switch (DamageType)
        {
            ///蓄力成功傷害*2
            case 2:
                Damage = Damage * 2;
                break;
            default:
                Damage = Damage * 1;
                break;
        }
        //Debug.Log(Damage);
    }

    //射中你啦PK
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "BossStando")
        {
            ///如果擊中分身
            //Help me Check if this is right or not.
            basicState = col.gameObject.GetComponentInParent<BasicState>();
            basicState._currentHealth -= Damage;
            //BossSpawnObject bossSpawn = col.gameObject.GetComponent<BossSpawnObject>();
            //bossSpawn.SpawnedCountDecrease();
            Destroy(this.gameObject);
        }

        if (col.gameObject.layer == 6)
        {
            ///如果擊中分身
            if (col.transform.tag == "Boss")
            {
                ///扣血
                bossHealth.TakeDamage(Damage);
                ///Boss Count -1
                ///
                bossSpawn = col.transform.parent.GetComponent<BossSpawnObject>();
                bossSpawn.SpawnedCountDecrease();
                //BossSpawnObject bossSpawn = col.gameObject.GetComponent<BossSpawnObject>();
                //bossSpawn.SpawnedCountDecrease();
                Destroy(this.gameObject);
            }
            //If Skill
            else
            {
                int i = Random.Range(1, 3);
                //Debug.Log(i);
                for (int j = 0; j < i; j++)
                {
                    Instantiate(chip, col.transform.position, Quaternion.identity);
                }

                if (!isSpcecialAttack)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Damage = Damage - 10;
                }
                Destroy(col.gameObject);
            }
        }     
    }

   IEnumerator DestroyTimer()
    {
        ///射擊三秒後一定消失
        yield return new WaitForSeconds(3);
        bossSpawn = GameObject.Find("Boss").GetComponent<BossSpawnObject>();
        bossSpawn.SpawnedCountDecrease();
        Destroy(this.gameObject);
    }
}
