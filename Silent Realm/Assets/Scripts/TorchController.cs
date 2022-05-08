using UnityEngine;

public class TorchController : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.AddListener(GameEvent.TRANSITION_TORCH, OnTransitionTorch);
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.RemoveListener(GameEvent.TRANSITION_TORCH, OnTransitionTorch);
    }

    private void OnEnemyActiveChanged(bool active)
    {
        if (!active)
        {
            foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
            {
                particleSystem.Play();
            }
            GetComponentInChildren<Light>().intensity = 1.0f;
        }
    }

    private void OnTransitionTorch()
    {
        foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Stop();
        }
        GetComponentInChildren<Light>().intensity = 0.0f;
    }
}
