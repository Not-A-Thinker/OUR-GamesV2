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
        if (Level1GameData.b_isBossDeathCutScene||Level1GameData.b_isCutScene) { Destroy(gameObject, 0.5f); }

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
        if (!this.gameObject.scene.isLoaded) return;
        if (Level1GameData.b_isBossDeathCutScene || Level1GameData.b_isCutScene) return;
        var odp = Instantiate(Resources.Load("Prefabs/Particle_OnDestroy"), transform.position, Quaternion.identity);
    }
}
