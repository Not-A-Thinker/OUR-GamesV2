using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    public static AudioClip Dog_Move, Dog_Attack,Dog_Dead;
    public static AudioClip Cat_Dead,Cat_Attack;
    public static AudioClip Player_Damage, Player_Dash, Player_GetDamage,Player_Respawn;
    public static AudioClip UI_ButtonClick,UI_Select;
    static AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();

        Dog_Move = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Move");
        Dog_Attack = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Attack");
        Dog_Dead = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Dead");

        Cat_Dead = Resources.Load<AudioClip>("SoundEffect/Cat/Cat_Dead");
        Cat_Attack = Resources.Load<AudioClip>("SoundEffect/Cat/Cat_Attack");

        Player_GetDamage = Resources.Load<AudioClip>("SoundEffect/Player/Player_GetDamage");
        Player_Dash = Resources.Load<AudioClip>("SoundEffect/Player/Player_Dash");
        Player_Respawn = Resources.Load<AudioClip>("SoundEffect/Player/Player_Respawn");

        UI_ButtonClick = Resources.Load<AudioClip>("SoundEffect/UI/UI_ButtonClick");
        UI_Select = Resources.Load<AudioClip>("SoundEffect/UI/UI_Select");
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Dog_Move":
                audioSrc.clip = Dog_Move;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
            case "Dog_StopMove":
                audioSrc.loop = false;
                audioSrc.clip = null;
                break;
            case "Dog_Attack":
                audioSrc.PlayOneShot(Dog_Attack);
                break;
            case "Dog_Dead":
                audioSrc.PlayOneShot(Dog_Dead);
                break;

            case "Cat_Attack":
                audioSrc.PlayOneShot(Cat_Attack,0.8f);
                Debug.Log("Attack Play");
                break;
            case "Cat_Dead":
                audioSrc.PlayOneShot(Cat_Dead);
                break;

            case "Player_Dash":
                audioSrc.PlayOneShot(Player_Dash);
                audioSrc.loop = false;
                audioSrc.Pause();            
                break;
            case "Player_GetDamage":
                audioSrc.PlayOneShot(Player_GetDamage);
                break;
            case "Player_Respawn":
                audioSrc.PlayOneShot(Player_Respawn);
                break;

            case "UI_ButtonClick":
                audioSrc.PlayOneShot(UI_ButtonClick,0.8f);
                break;
            case "UI_Select":
                audioSrc.PlayOneShot(UI_Select, 0.8f);
                break;
        }
    }
}
