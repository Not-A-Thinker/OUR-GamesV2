using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindBladeBoomerang : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float returnSpeed = 12f;

    [SerializeField] float rotateSpeed = 3;

    PlayerState playerState;
    BossCameraControl cameraControl;

    Vector3 orgPos;
    public Vector3 tarPos;
    public Vector3 velocity = Vector3.zero;

    [SerializeField] bool b_ShouldReturn = false;
    [SerializeField] bool b_Enabled = false;

    void Start()
    {
        orgPos = transform.position;
        cameraControl = GameObject.Find("TargetGroup1").GetComponent<BossCameraControl>();
    }


    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0) * -1 * rotateSpeed * Time.deltaTime);


        if (b_ShouldReturn)//May need to change due to whenever the boss if move
        {
            transform.position = Vector3.Lerp(transform.position, orgPos, returnSpeed * Time.deltaTime);
        }
        else if (!b_ShouldReturn)
        {
            transform.position = Vector3.SmoothDamp(transform.position, tarPos, ref velocity, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, tarPos) <= 0.1f)
        {
            if (!b_Enabled)
            {
                StartCoroutine(WaitingTimer());
            }
        }
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
        if (other.gameObject.tag == "Boss" || other.gameObject.tag == "BossStando")
        {
            if (b_ShouldReturn == true)
            {
                if (GameObject.FindGameObjectsWithTag("Boomerang").Length <= 5)
                    cameraControl.ChangeTargetWeight(3, 2);

                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitingTimer()
    {
        b_Enabled = true;
        yield return new WaitForSeconds(3);
        yield return b_ShouldReturn = true;
    }

}
