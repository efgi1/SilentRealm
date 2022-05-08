using UnityEngine;

public class StartingCircle : MonoBehaviour
{
    [SerializeField] ParticleSystem lightBeamEffect;

    public void OnEnable()
    {
        Messenger.AddListener(GameEvent.ALL_POTS_COLLECTED, OnAllPotsCollected);
    }

    public void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.ALL_POTS_COLLECTED, OnAllPotsCollected);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.AllPotsCollected && !GameManager.Instance.GameWon)
        {
            GameManager.Instance.GameWon = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.Started)
        {
            GameManager.Instance.TimeRemaining = 0f;
        }
        GameManager.Instance.Started = true;
    }

    void OnAllPotsCollected()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.20f, transform.position.z);
        Quaternion rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        Instantiate(lightBeamEffect, position, rotation);
    }
}
