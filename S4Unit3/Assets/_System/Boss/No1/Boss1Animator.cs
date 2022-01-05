using UnityEngine;

//This is mainly for storging the boss skill animation which have to use the animation event.
//So is only for boss animaton.
public class Boss1Animator : MonoBehaviour
{
    public BossSkillDemo BossSkill;

    public void Animation_BossTailAttack()
    {
        BossSkill.BossTailAttackAnimation();
    }

    public void Animation_BossPinBallAttack()
    {
        BossSkill.TornadoSpecialAttackAnimation();
    }


}
