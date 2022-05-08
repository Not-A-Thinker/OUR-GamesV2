using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindBlade : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float secondToDie = 3f;

    PlayerState playerState;

    void Update()
    {
        transform.position +=  transform.forward * speed * Time.deltaTime;
        Destroy(gameObject, secondToDie);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();

                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        var odp = Instantiate(Resources.Load("Prefabs/Particle_OnDestroy"), transform.position, Quaternion.identity);
    }
}
