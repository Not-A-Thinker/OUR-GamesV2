using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BubbleAttack : MonoBehaviour
{
    Move P1;
    Move P2;

    [SerializeField] float secondToDie = 10f;

    //bool b_shouldDestory = false;

    public string targetName;

    bool CCed;

    void Start()
    {
        P1 = GameObject.Find("Player1").GetComponent<Move>();
        P2 = GameObject.Find("Player2").GetComponent<Move>();

        StartCoroutine("DespawnTimer", secondToDie);
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player1")
        {
            if (!P1.isImMobilized)
            {
                P1.StartCoroutine("ImMobilzer", 1);
            }
            if(targetName != other.gameObject.name)
                Destroy(this.gameObject);
        }
        if (other.gameObject.name == "Player2")
        {
            if (!P2.isImMobilized)
            {
                P2.StartCoroutine("ImMobilzer", 1);
            }
            if (targetName != other.gameObject.name)
                Destroy(this.gameObject);
        }
    }

    IEnumerator DespawnTimer(int sec)//To despawn the obj
    {
        
        yield return new WaitForSeconds(sec);
        if (P1.isImMobilized)
        {
            P1.isImMobilized = false;
            P1.SpeedReset();
        }
        if (P2.isImMobilized)
        {
            P2.isImMobilized = false;
            P2.SpeedReset();
        }

        Destroy(gameObject);
    }

}
