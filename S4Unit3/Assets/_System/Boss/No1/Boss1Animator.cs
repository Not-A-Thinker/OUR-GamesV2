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
        if(Level1GameData.b_isBossDeathCutScene)
        {
            bossAngryEnd_ani();
            Boss_WingSpeedLineEnd_ani();
            Boss_EyesLightEnd_ani();
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
        if (BossAI.IsStage2)
        {
            int wBSpawnNum = Random.Range(6, 11);
            BossSkill.StartCoroutine(BossSkill.WindBalls(wBSpawnNum, 1));
            Boss1SoundManager.PlaySound("Boss_Noise01");
        }
        else
        {
            int wBSpawnNum = Random.Range(4, 7);
            BossSkill.StartCoroutine(BossSkill.WindBalls(wBSpawnNum, 1));
            Boss1SoundManager.PlaySound("Boss_Noise01");
        }
    }

    public void Animation_BossPinBallAttack()
    {
        //BossSkill.TornadoSpecialAttackAnimation();
        //bossVC.m_Priority = orgPriority;
        
        impulseSource.GenerateImpulse(2f);
        Boss1SoundManager.PlaySound("Boss_TransDown");
        BossAI.IsAfter33 = true;
        Level1GameData.b_isCutScene = false;
    }

    public void Camera_PinBallFadeIn()
    {
        bossVC.m_Priority = fadeInPriority;
        //Debug.Log(Level1GameData.b_isBoss1Defeated);
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

    [Header("Particl Stuff")]
    public ParticleSystem bossRoar;
    public ParticleSystem bossAngry;
    public ParticleSystem bossAngry_2;
    public ParticleSystem[] Particel_Boss_WingSpeedLine;
    public ParticleSystem[] Particle_BossEyesLight;


    public void bossRoarStart_ani()
    {
        if (bossRoar != null)
        {
            //bossRoar.gameObject.SetActive(true);
            bossRoar.Play();
            //print("isplaying ? " + bossRoar.isPlaying);
            //print("bossRoarStart_ani");
        }
    }
    public void bossRoarEnd_ani()
    {
        if (bossRoar != null && bossRoar.isPlaying)
        {
            //bossRoar.gameObject.SetActive(false);
            bossRoar.Stop();
            //print("bossRoarEnd_ani");

        }
    }
    public void bossAngryStart_ani()
    {
        if (bossAngry != null)
        {
            bossAngry.Play();
            //print("bossAngryStart_ani");
        }
        if (bossAngry_2 != null)
        {
            bossAngry_2.Play();
            //print("bossAngryStart_ani");
        }
    }
    public void bossAngryEnd_ani()
    {
        if (bossAngry != null && bossAngry.isPlaying)
        {
            bossAngry.Stop();
            //print("bossAngryEnd_ani");

        }
        if (bossAngry_2 != null)
        {
            bossAngry_2.Stop();
            //print("bossAngryStart_ani");
        }
    }

    public void Boss_WingSpeedLineStart_ani ()
    {
        foreach (ParticleSystem ps in Particel_Boss_WingSpeedLine)
        {
            if(ps!= null)
            {
                ps.Play();

                //print("WingSpeedLineStart");
            }
        }
    }
    public void Boss_WingSpeedLineEnd_ani()
    {
        foreach (ParticleSystem ps in Particel_Boss_WingSpeedLine)
        {
            if (ps != null && ps.isPlaying)
            {
                ps.Stop();
                //print("WingSpeedLineEnd");

            }
        }
    }

    public void Boss_EyesLightStart_ani ()
    {
        foreach (ParticleSystem ps in Particle_BossEyesLight)
        {
            if (ps != null)
            {
                ps.Play();
                //print("EyesLightStart");
            }
        }
    }
    public void Boss_EyesLightEnd_ani()
    {
        foreach (ParticleSystem ps in Particle_BossEyesLight)
        {
            if (ps != null && ps.isPlaying)
            {
                ps.Stop();
                //print("EyesLightEnd");

            }
        }
    }


}
