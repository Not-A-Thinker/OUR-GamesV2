using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    public static AudioClip Dog_Move, Dog_Attack;
    public static AudioClip Cat_Dead;
    public static AudioClip Player_Damage, Player_Desh;
    public static AudioClip UI_ButtonClick;
    static AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();

        Dog_Move = Resources.Load<AudioClip>("Dog_Move");
        Dog_Attack = Resources.Load<AudioClip>("Dog_Attack");

        Cat_Dead = Resources.Load<AudioClip>("Cat_Dead");

        Player_Damage = Resources.Load<AudioClip>("Player_Damage");
        Player_Desh = Resources.Load<AudioClip>("Player_Desh");

        UI_ButtonClick = Resources.Load<AudioClip>("UI_ButtonClick");
    }

    public static void PlaySound(string clip)
    {
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
            case "Player_Desh":
                audioSrc.PlayOneShot(Player_Desh);
                break;

            case "UI_ButtonClick":
                audioSrc.PlayOneShot(UI_ButtonClick);
                break;
        }
    }
}
