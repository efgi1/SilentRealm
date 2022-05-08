using UnityEngine;

public class GhostActivateSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip activate;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        Messenger.AddListener(GameEvent.TRANSITION_GHOST, OnTransitionGhost);
    }

    void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.TRANSITION_GHOST, OnTransitionGhost);
    }

    private void OnTransitionGhost()
    {
        audioSource.PlayOneShot(activate);
    }
}
