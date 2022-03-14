using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{

    [SerializeField] AudioSource OnMoveSound;
    [SerializeField] AudioSource OnDashSound;
    [SerializeField] AudioSource OnDamagedSound;
    [SerializeField] AudioSource OnAttackSound;
    [SerializeField] AudioSource OnRespawnSound;
    [SerializeField] AudioSource OnDeadSound;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnMovePlay()
    {

    }
    public void OnDashPlay()
    {

    }
    public void OnRespawnPlay()
    {

    }
}
