using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindHole : MonoBehaviour
{
    [SerializeField] float secondToDie = 12f;
    [SerializeField] float ActiveTime = 1f;

    PlayerState playerState;
    Move playerMove;

    void Start()
    {
        StartCoroutine(StartTimer());
    }
    

    void Update()
    {
        if (Level1GameData.b_isBossDeathCutScene) { Destroy(gameObject, 0.5f);}
        Destroy(gameObject, secondToDie);
    }

    IEnumerator StartTimer()
    {
        //Debug.Log("Wind Hole!");
        yield return new WaitForSeconds(ActiveTime);
        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();
                //While isKnockUp is true,Player will start getting knockup
                playerMove = other.GetComponent<Move>();
                playerMove.isKnockUp = true;
                //Debug.Log(other.gameObject.name);
            }
        }
    }
}
