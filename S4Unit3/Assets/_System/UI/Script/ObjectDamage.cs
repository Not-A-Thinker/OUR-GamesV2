using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    BossHealthBar bossHealth;
    BasicState basicState;
    BossSpawnObject bossSpawn;

    bool isSpcecialAttack;

    //�H��
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
        ///����w�ɶ��|�ۤv����
        StartCoroutine(DestroyTimer());

        //Rb = GetComponent<Rigidbody>();
        //Rigidbody Rb = GetComponent<Rigidbody>();
        //Rb.AddForceAtPosition(transform.forward * 2000f *5* Time.deltaTime, transform.position, ForceMode.Impulse);
    }

    private void Update()
    {
        ///�������
        //Rb.AddForceAtPosition(Direction * 2000 * Time.deltaTime, transform.position, ForceMode.Impulse);
        transform.position += Direction * Speed * Time.deltaTime;
        Speed = Speed * 0.8f * Time.deltaTime;
    }

    //Do You Know YourDamage
    public void SetDamage(int DamageType)
    {
        ///�S������򥻶ˮ`70
        if (isSpcecialAttack)
            Damage = Damage + 55;

        switch (DamageType)
        {
            ///�W�O���\�ˮ`*2
            case 2:
                Damage = Damage * 2;
                break;
            default:
                Damage = Damage * 1;
                break;
        }
        //Debug.Log(Damage);
    }

    //�g���A��PK
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "BossStando")
        {
            ///�p�G��������
            //Help me Check if this is right or not.
            basicState = col.gameObject.GetComponentInParent<BasicState>();
            basicState._currentHealth -= Damage;
            //BossSpawnObject bossSpawn = col.gameObject.GetComponent<BossSpawnObject>();
            //bossSpawn.SpawnedCountDecrease();
            Destroy(this.gameObject);
        }

        if (col.gameObject.layer == 6)
        {
            ///�p�G��������
            if (col.transform.tag == "Boss")
            {
                ///����
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
        ///�g���T���@�w����
        yield return new WaitForSeconds(3);
        bossSpawn = GameObject.Find("Boss").GetComponent<BossSpawnObject>();
        bossSpawn.SpawnedCountDecrease();
        Destroy(this.gameObject);
    }
}
