using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindBall : MonoBehaviour
{
    //Rigidbody rb;
    //[SerializeField] float _fallingSpeed = 3f;

    PlayerState playerState;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Level1GameData.b_isBossDeathCutScene) { Destroy(gameObject); }
    }

    void FixedUpdate()
    {

        //rb.velocity = new Vector3(transform.position.x, -1, transform.position.z) * _fallingSpeed;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Ground")
        {
            Destroy(gameObject, .3f);
        }
        if (col.transform.tag == "Wall")
        {
            Destroy(gameObject, .1f);
        }
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                playerState = col.gameObject.GetComponent<PlayerState>();
                playerState.hp_decrease();

                Destroy(gameObject);
            }

        }
    }
    private void LateUpdate()
    {
        RayToGround();

    }
    private void OnDestroy()
    {
        //Destroy(hitPointObj);
    }

    private Vector3 collsion = Vector3.zero;
    public Vector3 offsetPos;
    private GameObject lastHit;
    public LayerMask m_layer;
    public GameObject hitPointObj;
    public GameObject hitGameObj = null;
    public void RayToGround()
    {
        var ray = new Ray(this.transform.position, -this.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_layer)&& hit.transform.gameObject.tag=="Ground")
        {
            lastHit = hit.transform.gameObject;
            collsion = hit.point;
            if(hitGameObj==null)
            {
                hitGameObj = Instantiate(hitPointObj, collsion, Quaternion.identity);
            }
            else
            {
                hitGameObj.transform.position = new Vector3(collsion.x+offsetPos.x,collsion.y+offsetPos.y,collsion.z+offsetPos.z);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(collsion, 0.2f);
    }
}
