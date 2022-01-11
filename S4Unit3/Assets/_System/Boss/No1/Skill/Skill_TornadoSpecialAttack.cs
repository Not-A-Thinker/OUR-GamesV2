using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TornadoSpecialAttack : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 12f;

    PlayerState playerState;
    BossCameraControl cameraControl;

    [SerializeField] bool b_CanBounce = true;
    public int TimesCanBounce = 3;
    private int bounceCount = 0;

    private Vector3 lastFrameVelocity;

    void Start()
    {
        b_CanBounce = true;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();
    }

    void Update()
    {
        lastFrameVelocity = rb.velocity;
        Destroy(gameObject, 25f);
    }

    private void OnDestroy()
    {
        LastDetect();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && b_CanBounce)
        {
            Bounce(collision.contacts[0].normal);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject, 0.1f);
        }

        if (collision.gameObject.tag == "Breakable Wall" && b_CanBounce)
        {
            Bounce(collision.contacts[0].normal);
        }
        else if (collision.gameObject.tag == "Breakable Wall")
        {
            Destroy(gameObject, 0.1f);
        }


        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = collision.gameObject.GetComponent<PlayerState>();
                playerState.hp_decrease();

                Destroy(gameObject);
            }

        }

    }
    private void Bounce(Vector3 collisionNormal)
    {
        var lastSpeed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        bounceCount++;
        if (bounceCount == TimesCanBounce)
        {
            Debug.Log("No more Bounce!");
            b_CanBounce = false;
        }
        //Debug.Log("Out Direction: " + direction);
        rb.velocity = direction * Mathf.Max(lastSpeed, speed);
    }


    private void LastDetect()
    {
        if (GameObject.FindGameObjectsWithTag("STA").Length <= 1)
            cameraControl.ChangeTargetWeight(3, 1);
    }
}