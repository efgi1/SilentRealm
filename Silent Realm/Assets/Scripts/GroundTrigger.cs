using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TimeRemaining = 0f;
        }
    }
}
