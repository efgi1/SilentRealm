using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemStepEmitter : MonoBehaviour
{ 
    public AudioClip[] stepSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void EmitFootstep(float volume)
    {
        AudioClip randClip = stepSounds[Random.Range(0, stepSounds.Length)];
        audioSource.volume = volume;
        audioSource.PlayOneShot(randClip);
    }
}
