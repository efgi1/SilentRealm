using UnityEngine;

public class GolemActivateSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip activate;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.TRANSITION_GOLEM, OnTransitionGolem);
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION_GOLEM, OnTransitionGolem);
    }

    private void OnTransitionGolem(bool playSound)
    {
        if (playSound)
        {
            audioSource.PlayOneShot(activate);
        }
    }
}
