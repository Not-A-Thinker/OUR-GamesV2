using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public int Damage = 15;
    BossHealthBar bossHealth;
    BasicState basicState;

    bool isSpcecialAttack;

    //¸H¤ù
    public GameObject chip;

    //private Rigidbody Rb;

    public Vector3 Direction;

    public float Speed = 50;

    private void Start()
    {
        bossHealth = GameObject.Find("Boss Health Bar").GetComponent<BossHealthBar>();
        if (this.gameObject.name.Contains("SpecialAttack"))
            isSpcecialAttack = true;

        StartCoroutine(DestroyTimer());

    
        //Rb = GetComponent<Rigidbody>();
        //Rigidbody Rb = GetComponent<Rigidbody>();
        //Rb.AddForceAtPosition(transform.forward * 2000f *5* Time.deltaTime, transform.position, ForceMode.Impulse);
    }

    private void Update()
    {
        //Rb.AddForceAtPosition(Direction * 2000 * Time.deltaTime, transform.position, ForceMode.Impulse);
        transform.position += Direction * Speed * Time.deltaTime;

       
    }

    //Do You Know YourDamage
    public void SetDamage(int DamageType)
    {
        if (isSpcecialAttack)
            Damage = Damage + 55;

        switch (DamageType)
        {
            case 2:
                Damage = Damage * 2;
                break;
            default:
                Damage = Damage * 1;
                break;
        }
        //Debug.Log(Damage);
    }

    //®g¤¤§A°ÕPK
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "BossStando")
        {
            //Help me Check if this is right or not.
            basicState = col.gameObject.GetComponentInParent<BasicState>();
            basicState._currentHealth -= Damage;
            //BossSpawnObject bossSpawn = col.gameObject.GetComponent<BossSpawnObject>();
            //bossSpawn.SpawnedCountDecrease();
            Destroy(this.gameObject);
        }

        if (col.gameObject.layer == 6)
        {
            //IfBoss
            if (col.transform.tag == "Boss")
            {
                bossHealth.TakeDamage(Damage);
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
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
