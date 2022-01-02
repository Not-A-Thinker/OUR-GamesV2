using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is mainly for storging the boss skill animation which have to use the animation event.

public class Boss1Animator : MonoBehaviour
{
    public BossSkillDemo BossSkill;

    void Start()
    {
        
    }

    public void Animation_BossTailAttack()
    {
        BossSkill.BossTailAttackAnimation();
    }

    public void Animation_BossPinBallAttack()
    {

    }


}
