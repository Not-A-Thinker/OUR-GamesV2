using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will need to add a S-Shape rountine to player
public class Skill_TornadoAttack : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float secondToDie = 15f;

    PlayerState playerState;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Level1GameData.b_isBossDeathCutScene || Level1GameData.b_isCutScene) { Destroy(gameObject, 0.5f); }

        transform.position += transform.forward * speed * Time.deltaTime;
        Destroy(gameObject, secondToDie);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            //Debug.Log("Hit the Wall!");
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Breakable Wall")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            playerState = other.GetComponent<PlayerState>();
            playerState.hp_decrease();
            //Debug.Log("Hit!");
            Destroy(gameObject);
        }

    }
    private void OnDestroy()
    {
        if (Level1GameData.b_isBossDeathCutScene || Level1GameData.b_isCutScene) return;
        if (!this.gameObject.scene.isLoaded) return;
        var odp = Instantiate(Resources.Load("Prefabs/Particle_OnDestroy"), transform.position, Quaternion.identity);

    }

}
