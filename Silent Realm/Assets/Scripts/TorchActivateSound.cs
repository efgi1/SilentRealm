using UnityEngine;

public class TorchActivateSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip blowOut;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        Messenger.AddListener(GameEvent.TRANSITION_TORCH, OnTransitionTorch);
    }

    void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.TRANSITION_TORCH, OnTransitionTorch);
    }

    private void OnTransitionTorch()
    {
        audioSource.PlayOneShot(blowOut);
    }
}
