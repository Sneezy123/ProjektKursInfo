using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerSound : MonoBehaviour
{
    // Public
    public AudioClip[] stepSounds;
    
    // Private
    private AudioSource stepSource;

    void Start()
    {
        stepSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        AudioClip stepClip = stepSounds[Random.Range(0, stepSounds.Length)];
        stepSource.clip = stepClip;
        stepSource.volume = Random.Range(0.4f, 0.45f);
        stepSource.pitch = Random.Range(0.8f, 1.2f);
        stepSource.Play();
        Debug.Log(stepClip.name);
    }
}
