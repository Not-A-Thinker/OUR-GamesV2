using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TornadoAttack_SForm : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float secondToDie = 15f;

    [SerializeField] float Leftx = -5f;
    [SerializeField] float Rightx = 5f;
    Vector3 selfPos;
    Vector3 dirPos;

    public bool CanMove = true;

    [Header("Frequency: How Fast it finish 1 turn")]
    [SerializeField] float frequency = 20f;
    [Header("Magnitude: Control the x position")]
    [SerializeField] float magnitude = 0.5f;

    PlayerState playerState;

    void Start()
    {
        //StartCoroutine(TornadoAttack_SStart());
        selfPos = transform.position;

        dirPos.x = Leftx;
    }


    void Update()
    {
        if (Level1GameData.b_isBossDeathCutScene || Level1GameData.b_isCutScene) { Destroy(gameObject, 0.5f); }
        Destroy(gameObject, secondToDie);

        if(CanMove)
            LerpMovement();
    }

    void SimpleMovement()
    {
        transform.position += new Vector3(dirPos.x * Time.deltaTime, 0, speed * Time.deltaTime);

        if (transform.position.x <= selfPos.x + Leftx)
        {
            dirPos.x = Rightx;
        }
        else if (transform.position.x >= selfPos.x + Rightx)
        {
            dirPos.x = Leftx;
        }
    }

    void LerpMovement()
    {
        selfPos += transform.forward * Time.deltaTime * speed;
        transform.position = selfPos + transform.right * Mathf.Sin(Time.time * frequency) * magnitude;

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
            if (other.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = other.GetComponent<PlayerState>();
                playerState.hp_decrease();
                //Debug.Log("Hit!");
                Destroy(gameObject);
            }
        }

    }
    private void OnDestroy()
    {
        var odp = Instantiate(Resources.Load("Prefabs/Particle_OnDestroy"), transform.position, Quaternion.identity);

    }


}
