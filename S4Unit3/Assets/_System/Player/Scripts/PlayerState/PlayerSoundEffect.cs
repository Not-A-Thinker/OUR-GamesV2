using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    public static AudioClip Dog_Move, Dog_Attack;
    public static AudioClip Cat_Dead;
    public static AudioClip Player_Damage, Player_Dash, Player_GetDamage;
    public static AudioClip UI_ButtonClick;
    static AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();

        Dog_Move = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Move");
        Dog_Attack = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Attack");

        Cat_Dead = Resources.Load<AudioClip>("SoundEffect/Cat/Cat_Dead");

        Player_GetDamage = Resources.Load<AudioClip>("SoundEffect/Player/Player_GetDamage");
        Player_Dash = Resources.Load<AudioClip>("SoundEffect/Player/Player_Desh");

        UI_ButtonClick = Resources.Load<AudioClip>("SoundEffect/UI/UI_ButtonClick");    
    }

    public static void PlaySound(string clip)
    {
        //Debug.Log(Dog_Move.name);
        
        switch (clip)
        {
            case "Dog_Move":
                audioSrc.clip = Dog_Move;
                audioSrc.loop = true;
                audioSrc.Play();         
                break;
            case "Dog_StopMove":
                audioSrc.loop = false;
                audioSrc.Pause();
                break;
            case "Dog_Attack":
                audioSrc.PlayOneShot(Dog_Attack);
                break;

            case "Cat_Dead":
                audioSrc.PlayOneShot(Cat_Dead);
                break;

            case "Player_Damage":
                audioSrc.PlayOneShot(Player_Damage);
                break;
            case "Player_Dash":
                audioSrc.PlayOneShot(Player_Dash);
                audioSrc.loop = false;
                audioSrc.Pause();            
                break;
            case "Player_GetDamage":
                audioSrc.PlayOneShot(Player_GetDamage);
                break;

            case "UI_ButtonClick":
                audioSrc.PlayOneShot(UI_ButtonClick);
                break;
        }
    }
}
