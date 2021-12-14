using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_VacuumArea : MonoBehaviour
{
    Move P1;
    Move P2;

    [SerializeField] float secondToDie = 5f;

    void Start()
    {
        P1 = GameObject.Find("Player1").GetComponent<Move>();
        P2 = GameObject.Find("Player2").GetComponent<Move>();
    }

    void Update()
    {
        Destroy(gameObject, secondToDie);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player1")
        {
            if (!P1.isSlowed)
            {
                P1.StartCoroutine("SlowEffectTimer");
            }
        }
        if (other.gameObject.name == "Player2")
        {
            if (!P2.isSlowed)
            {
                P2.StartCoroutine("SlowEffectTimer");
            }
        }
    }

}
