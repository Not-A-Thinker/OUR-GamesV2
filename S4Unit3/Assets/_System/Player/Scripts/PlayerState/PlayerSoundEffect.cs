using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{

    [SerializeField] AudioClip OnMoveSound;
    [SerializeField] AudioClip OnDashSound;
    [SerializeField] AudioClip OnDamagedSound;
    [SerializeField] AudioClip OnAttackSound;
    [SerializeField] AudioClip OnRespawnSound;
    [SerializeField] AudioClip OnDeadSound;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnMovePlay()
    {
        _audioSource.clip = OnMoveSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    public void OnDashPlay()
    {
        _audioSource.clip = OnDashSound;
        _audioSource.Play();
    }
    public void OnRespawnPlay()
    {
        _audioSource.clip = OnRespawnSound;
        _audioSource.Play();
    }
    public void OnAttackPlay()
    {
        _audioSource.clip = OnAttackSound;
        _audioSource.Play();
    }
    public void OnResetSound()
    {
        _audioSource.clip = null;
        _audioSource.loop = false;
        _audioSource.Stop();
    }
}
