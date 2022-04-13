using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    public static AudioClip Dog_Move, Dog_Attack;
    public static AudioClip Cat_Dead;
    public static AudioClip Player_Damage, Player_Dash;
    public static AudioClip UI_ButtonClick;
    static AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();

        Dog_Move = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Move");
        Dog_Attack = Resources.Load<AudioClip>("SoundEffect/Dog/Dog_Attack");

        Cat_Dead = Resources.Load<AudioClip>("SoundEffect/Cat/Cat_Dead");

        Player_Damage = Resources.Load<AudioClip>("SoundEffect/Player/Player_Damage");
        Player_Dash = Resources.Load<AudioClip>("SoundEffect/Player/Player_Desh");

        UI_ButtonClick = Resources.Load<AudioClip>("SoundEffect/UI/UI_ButtonClick");    
    }

    public static void PlaySound(string clip)
    {
        //Debug.Log(Dog_Move.name);
        
        switch (clip)
        {
            case "Dog_Move":
                audioSrc.PlayOneShot(Dog_Move);
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
                break;

            case "UI_ButtonClick":
                audioSrc.PlayOneShot(UI_ButtonClick);
                break;
        }
    }
}
