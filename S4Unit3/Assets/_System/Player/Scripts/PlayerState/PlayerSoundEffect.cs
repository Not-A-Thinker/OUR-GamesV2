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
        if(_audioSource.isActiveAndEnabled)
        {
            _audioSource.clip = OnMoveSound;
            _audioSource.loop = true;
            _audioSource.Play();
        }      
    }
    public void OnDashPlay()
    {
        if (_audioSource.isActiveAndEnabled)
        {
            _audioSource.clip = OnDashSound;
            _audioSource.Play();
        }         
    }
    public void OnRespawnPlay()
    {
        if (_audioSource.isActiveAndEnabled)
        {
            _audioSource.clip = OnRespawnSound;
            _audioSource.Play();
        }
    }
    public void OnAttackPlay()
    {
        if (_audioSource.isActiveAndEnabled)
        {
            _audioSource.clip = OnAttackSound;
            _audioSource.Play();
        }        
    }
    public void OnResetSound()
    {
        if (_audioSource.isActiveAndEnabled)
        {
            _audioSource.clip = null;
            _audioSource.loop = false;
            _audioSource.Stop();
        }          
    }
    public void OnDamagePlay()
    {
        if (_audioSource.isActiveAndEnabled)
        {
            _audioSource.clip = OnDamagedSound;
            _audioSource.Play();
        }
    }
}
