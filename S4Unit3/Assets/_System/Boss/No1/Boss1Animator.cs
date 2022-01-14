using UnityEngine;
using Cinemachine;

//This is mainly for storging the boss skill animation which have to use the animation event.
//So is only for boss animaton.
//Or maybe the camera movement?
public class Boss1Animator : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    [Header("Boss Skill Holder")]
    public BossSkillDemo BossSkill;

    [Header("Virtual Camera Control")]
    public CinemachineVirtualCamera mainVC;
    public CinemachineVirtualCamera bossVC;
    public int orgPriority = 5;
    public int fadeInPriority = 15;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            impulseSource.GenerateImpulse();
        }
    }

    public void Animation_BossTailAttack()
    {
        BossSkill.BossTailAttackAnimation();
    }

    public void Animation_BossPinBallAttack()
    {
        BossSkill.TornadoSpecialAttackAnimation();
        //bossVC.m_Priority = orgPriority;
        impulseSource.GenerateImpulse();
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
