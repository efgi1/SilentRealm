using UnityEngine;

public class PotEmitter : MonoBehaviour
{
    public AudioClip[] potSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void EmitPot()
    {
        AudioClip randClip = potSounds[Random.Range(0, potSounds.Length)];
        audioSource.PlayOneShot(randClip);
    }
}
