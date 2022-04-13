using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

public class PlayerAnimator : MonoBehaviour
{
    Animator PlayerAn;
    private CinemachineCollisionImpulseSource CCIS;
    [SerializeField] Collider _Collider;

    private void Awake()
    {
        PlayerAn = GetComponent<Animator>();
        CCIS = GetComponentInParent<CinemachineCollisionImpulseSource>();
    }

    public void PlayerWalk(bool State)
    {
        PlayerAn.SetBool("IsWalk", State);
        if(State==true)
            PlayerSoundEffect.PlaySound("Dog_Move");
    }

    public void PlayerShoot()
    {
        PlayerAn.SetTrigger("Shoot");
    }

    public IEnumerator PlayerDash(float time)
    {
        PlayerAn.SetBool("isDash", true);
        PlayerSoundEffect.PlaySound("Player_Dash");
        _Collider.enabled = false;

        yield return new WaitForSeconds(time);

        PlayerAn.SetBool("isDash", false);
        //playerSound.PlaySound("Dog_Move");
        _Collider.enabled = true;
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
