using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Boss1SoundManager : MonoBehaviour
{

    static AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
