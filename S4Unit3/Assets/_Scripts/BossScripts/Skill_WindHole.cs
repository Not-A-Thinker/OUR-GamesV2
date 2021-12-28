using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindHole : MonoBehaviour
{
    [SerializeField] float secondToDie = 12f;

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


}
