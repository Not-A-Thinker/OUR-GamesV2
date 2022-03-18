using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

public class PlayerAnimator : MonoBehaviour
{
    Animator PlayerAn;
    PlayerSoundEffect playerSound;
    private CinemachineCollisionImpulseSource CCIS;

    private void Awake()
    {
        PlayerAn = GetComponent<Animator>();
        playerSound = GetComponent<PlayerSoundEffect>();
        CCIS = GetComponentInParent<CinemachineCollisionImpulseSource>();
    }

    public void PlayerWalk(bool State)
    {
        PlayerAn.SetBool("IsWalk", State);
        if(gameObject.name == "DogWithBon")
        {
            if (State == true)
                playerSound.OnMovePlay();
            else
                playerSound.OnResetSound();
        }     
    }

    public void PlayerShoot()
    {
        PlayerAn.SetTrigger("Shoot");
    }

    public void PlayerDash(bool State)
    {
        PlayerAn.SetBool("isDash", State);
        if (State == true)
            playerSound.OnDashPlay();
        else
            playerSound.OnResetSound();
        
    }
    public void PlayerDead()
    {
        PlayerAn.SetTrigger("Die");
    }

    public void PlayerRespawn()
    {
        PlayerAn.SetTrigger("Respawn");
    }
    public IEnumerator PlayerDamaged()
    {
        CCIS.enabled = false;
        PlayerAn.SetBool("Dammagerd", true);

        yield return new WaitForSeconds(0.5f);
        PlayerAn.SetBool("Dammagerd", false);
        CCIS.enabled = true;
    }


    public ParticleSystem Smoke;
    public ParticleSystem SpeedLine;
    public ParticleSystem Afterimage;
    public void PlayerSomke()
    {
        if (Smoke != null)
            Smoke.Play();
    }

    public void PlayerDodge()
    {
        if (SpeedLine != null)
            SpeedLine.Play();
        if (Afterimage != null)
            Afterimage.Play();
    }

    public void PlayerDamag()
    {
        if (SpeedLine != null)
            SpeedLine.Play();
    }

}
