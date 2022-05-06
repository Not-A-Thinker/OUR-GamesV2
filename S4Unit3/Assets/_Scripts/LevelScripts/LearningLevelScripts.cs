using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningLevelScripts : MonoBehaviour
{
    [Header("Learning Scence Component")]
    [SerializeField] GameObject _LarningText;
    [SerializeField] GameObject _Portal;
    [SerializeField] Animator _LarningTextAnim, _LarningTextRotateAnim;
    [SerializeField] GameObject RespawnFakeSheep;

    [Header("Learning Scence State")]
    public bool isMove;
    public bool isDash, isAttack, isCD, isRespawn;
    bool isP1Move, isP1Dash, isP1Attack;
    bool isP2Move, isP2Dash, isP2Attack;
   
    [SerializeField] Move P1Move, P2Move;
    [SerializeField] PlayerState P1State, P2State;
    [SerializeField] ForceCast_TopDown P1AttackState;
    [SerializeField] ForceRepel_TopDown P2AttackState;
    // Start is called before the first frame update
    void Start()
    {
        isMove = isDash = isAttack = isCD = isRespawn =false;
        isP1Move = isP1Dash = isP1Attack = false;
        isP2Move = isP2Dash = isP2Attack = false;
        RespawnFakeSheep.SetActive(false);
        _Portal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isRespawn = true;
            }
        }     
        //Player State Check
        if (P1Move.isMove == true)
            isP1Move = true;
        if (P2Move.isMove == true)
            isP2Move = true;
        if (isMove)
        {
            if (P1Move.isDashed == true)
                isP1Dash = true;
            if (P2Move.isDashed == true)
                isP2Dash = true;
        }
        if (isDash)
        {
            if (P1AttackState.isShooted == true)
                isP1Attack = true;
            if (P2AttackState.onSucking == true)
                isP2Attack = true;
        }
        if (isCD)
        {
            SpawnRespawnObjcet();
        }
    }
    private void LateUpdate()
    {
        //StageControl
        if (isP1Move == isP2Move && isP1Move == true && isMove == false)
        {
            isMove = true;
            _LarningTextAnim.SetBool("isMoveDone", true);
            StartCoroutine(RotateCounter());
        }
        if (isP1Dash == isP2Dash && isP1Dash == true && isDash == false)
        {
            isDash = true;
            _LarningTextAnim.SetBool("isDashDone", true);
            StartCoroutine(RotateCounter());
        }
        if (isP1Attack == isP2Attack && isP1Attack == true && isAttack == false)
        {
            isAttack = true;
            StartCoroutine(CDbar());
        }
    }

    public void isRespawnDone()
    {
        isRespawn = true;
        RespawnFakeSheep.SetActive(false);
        _LarningTextAnim.SetBool("isRespawnDone", true);
        _Portal.SetActive(true);
        Debug.Log("Respawned!");
    }
    void SpawnRespawnObjcet()
    {
        RespawnFakeSheep.SetActive(true);
    }
    IEnumerator RotateCounter()
    {
        _LarningTextRotateAnim.SetBool("isChangeText", true);
        yield return new WaitForSeconds(0.1f);
        _LarningTextRotateAnim.SetBool("isChangeText", false);
    }
    IEnumerator CDbar()
    {
        _LarningTextAnim.SetBool("isAttackDone", isMove);     
        StartCoroutine(RotateCounter());
        yield return new WaitForSeconds(3f);
        _LarningTextAnim.SetBool("isCDDone", isMove);
        SpawnRespawnObjcet();
    }
}
