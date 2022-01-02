using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Animator : MonoBehaviour
{
    public BossSkillDemo BossSkill;

    void Start()
    {
        
    }

    public void Animation_BossTailAttack()
    {
        BossSkill.bossTailAttackAnimation();
    }
}
