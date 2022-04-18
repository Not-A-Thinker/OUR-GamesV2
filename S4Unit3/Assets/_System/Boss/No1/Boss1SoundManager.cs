using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Boss1SoundManager : MonoBehaviour
{
    static AudioSource audioSrc;

    public static AudioClip Boss_Idle01, Boss_Idle02;
    public static AudioClip Boss_Wing01, Boss_Wing02, Boss_Wing03;
    public static AudioClip Boss_Noise01, Boss_Noise02;

    public static AudioClip Boss_TailAttack, Boss_TailAttack_Alt;
    public static AudioClip Boss_EightTornado;

    public static AudioClip Boss_TransUp, Boss_TransDown;
    public static AudioClip Boss_DeadNoise01, Boss_DeadNoise02, Boss_DeadNoise03, Boss_DeadNoise04, Boss_DeadNoise05;
    public static AudioClip Boss_Dead;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();

        Boss_Idle01 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Idle_01");
        Boss_Idle02 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Idle_02");

        Boss_Wing01 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Wing_Swing_01");
        Boss_Wing02 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Wing_Swing_02");
        Boss_Wing03 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Wing_Swing_03");

        Boss_Noise01 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Noise_01");
        Boss_Noise02 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Noise_02");

        Boss_TailAttack = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Skill_TailAttack");
        Boss_TailAttack_Alt = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Skill_TailAttack_Alter");

        Boss_EightTornado = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Skill_EightTornado");

        Boss_TransUp = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Transition_Up");
        Boss_TransDown = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Transition_Down");

        Boss_DeadNoise01 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Dead_Noise_01");
        Boss_DeadNoise02 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Dead_Noise_02");
        Boss_DeadNoise03 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Dead_Noise_03");
        Boss_DeadNoise04 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Dead_Noise_04");
        Boss_DeadNoise05 = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Dead_Noise_05");

        Boss_Dead = Resources.Load<AudioClip>("SoundEffect/BOSS/01/Sfx_Boss_Dead");
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Boss_Idle01":
                audioSrc.PlayOneShot(Boss_Idle01, 0.2f);
                break;
            case "Boss_Idle02":
                audioSrc.PlayOneShot(Boss_Idle02);
                break;

            case "Boss_Wing01":
                audioSrc.PlayOneShot(Boss_Wing01);
                break;
            case "Boss_Wing02":
                audioSrc.PlayOneShot(Boss_Wing02);
                break;
            case "Boss_Wing03":
                audioSrc.PlayOneShot(Boss_Wing03);
                break;

            case "Boss_Noise01":
                audioSrc.PlayOneShot(Boss_Noise01);
                break;
            case "Boss_Noise02":
                audioSrc.PlayOneShot(Boss_Noise02);
                break;

            case "Boss_TailAttack":
                audioSrc.PlayOneShot(Boss_TailAttack);
                break;
            case "Boss_TailAttack_Alt":
                audioSrc.PlayOneShot(Boss_TailAttack_Alt);
                break;
            case "Boss_EightTornado":
                audioSrc.PlayOneShot(Boss_EightTornado);
                break;

            case "Boss_TransUp":
                audioSrc.PlayOneShot(Boss_TransUp);
                break;
            case "Boss_TransDown":
                audioSrc.PlayOneShot(Boss_TransDown);
                break;

            case "Boss_DeadNoise01":
                audioSrc.PlayOneShot(Boss_DeadNoise01, 0.8f);
                break;
            case "Boss_DeadNoise02":
                audioSrc.PlayOneShot(Boss_DeadNoise02, 0.8f);
                break;
            case "Boss_DeadNoise03":
                audioSrc.PlayOneShot(Boss_DeadNoise03, 0.8f);
                break;
            case "Boss_DeadNoise04":
                audioSrc.PlayOneShot(Boss_DeadNoise04, 0.8f);
                break;
            case "Boss_DeadNoise05":
                audioSrc.PlayOneShot(Boss_DeadNoise05);
                break;
            case "Boss_Dead":
                audioSrc.PlayOneShot(Boss_Dead);
                break;
            default:
                Debug.Log("No Audio Clip Found!");
                break;
        }
    }
}
