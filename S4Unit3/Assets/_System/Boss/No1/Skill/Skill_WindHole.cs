using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindHole : MonoBehaviour
{
    [SerializeField] float secondToDie = 12f;

    PlayerState playerState;

    void Start()
    {
        StartCoroutine(StartTimer());
    }
    

    void Update()
    {
        Destroy(gameObject, secondToDie);
    }

    IEnumerator StartTimer()
    {
        
        yield return new WaitForSeconds(3);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();
                //Debug.Log("Hit!");
            }
        }
    }
}
