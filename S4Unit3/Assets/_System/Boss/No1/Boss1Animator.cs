using UnityEngine;
using Cinemachine;

//This is mainly for storging the boss skill animation which have to use the animation event.
//So is only for boss animaton.
//Or maybe the camera movement?
public class Boss1Animator : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    [Header("Boss Skill and AI Holder")]
    public BossSkillDemo BossSkill;
    public BossAI_Wind BossAI;

    [Header("Virtual Camera Control")]
    public CinemachineVirtualCamera mainVC;
    public CinemachineVirtualCamera bossVC;
    public int orgPriority = 5;
    public int fadeInPriority = 15;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (BossAI == null)
        {BossAI = GetComponentInParent<BossAI_Wind>();}
        if (BossSkill == null)
        {BossSkill = GetComponentInParent<BossSkillDemo>();}
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.X))
        {
            impulseSource.GenerateImpulse();
        }
    }

    public void Animation_BossWindBladeAttack()
    {
        BossSkill.StartCoroutine(BossSkill.WindBlade16(3));
    }

    public void Animation_BossTailAttack()
    {
        BossSkill.BossTailAttackAnimation();
    }

    public void Animation_BossHeadAttack()
    {
        BossSkill.BossHeadAttackAnimation();
    }

    public void Animation_BossPinBallAttack()
    {
        //BossSkill.TornadoSpecialAttackAnimation();
        //bossVC.m_Priority = orgPriority;
        impulseSource.GenerateImpulse();
        BossAI.IsAfter33 = true;
    }

    public void Camera_PinBallFadeIn()
    {
        bossVC.m_Priority = fadeInPriority;
    }

    public void Camera_PinBallFadeOut()
    {
        bossVC.m_Priority = orgPriority;
    }

    public void Hi()
    {
        Debug.Log("Say Hi.");
    }
}
