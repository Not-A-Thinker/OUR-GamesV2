using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningLevelScripts : MonoBehaviour
{
    [Header("Learning Scence Component")]
    [SerializeField] GameObject _LarningText;
    [SerializeField] GameObject _Portal;
    Animation _LarningTextAnim, _LarningTextRotateAnim;

    [Header("Learning Scence State")]
    public bool isMove, isDash, isAttack, isCD, isRespawn;
    bool isP1Move, isP1Dash, isP1Attack, isP1CD;
    bool isP2Move, isP2Dash, isP2Attack, isP2CD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
