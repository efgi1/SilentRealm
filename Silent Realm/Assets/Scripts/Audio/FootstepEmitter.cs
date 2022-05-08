using UnityEngine;

public class FootstepEmitter : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void EmitFootstep()
    {
        AudioClip randClip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(randClip);
    }
}
