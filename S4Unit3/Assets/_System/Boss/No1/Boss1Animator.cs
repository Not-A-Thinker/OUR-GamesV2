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

    public void Animation_Death()
    {
        Level1GameData.b_isBoss1Defeated = true;
        Debug.Log("The Boss is dead, long live the big Boss!" + Level1GameData.b_isBoss1Defeated);
    }

    public void Animation_BossWindBladeAttack()
    {
        int wBNum = Random.Range(3, 6); 
        BossSkill.StartCoroutine(BossSkill.WindBlade16(wBNum));
        Boss1SoundManager.PlaySound("Boss_Wing03");
    }

    public void Animation_BossTailAttack()
    {
        BossSkill.BossTailAttackAnimation();
        Boss1SoundManager.PlaySound("Boss_TailAttack");
    }

    public void Animation_BossHeadAttack()
    {
        BossSkill.BossHeadAttackAnimation();
    }

    public void Animation_BossWindBall()
    {
        int wBSpawnNum = Random.Range(4, 7);
        BossSkill.StartCoroutine(BossSkill.WindBalls(wBSpawnNum, 1));
        Boss1SoundManager.PlaySound("Boss_Noise01");
    }

    public void Animation_BossPinBallAttack()
    {
        //BossSkill.TornadoSpecialAttackAnimation();
        //bossVC.m_Priority = orgPriority;
        impulseSource.GenerateImpulse();
        Boss1SoundManager.PlaySound("Boss_TransDown");
        BossAI.IsAfter33 = true;
        Level1GameData.b_isCutScene = false;
    }

    public void Camera_PinBallFadeIn()
    {
        bossVC.m_Priority = fadeInPriority;
    }

    public void Camera_PinBallFadeOut()
    {
        bossVC.m_Priority = orgPriority;
    }

    public void PlaySound(string clip)
    {
        Boss1SoundManager.PlaySound(clip);
    }

    public void Hi()
    {
        Debug.Log("Say Hi.");
    }
}
