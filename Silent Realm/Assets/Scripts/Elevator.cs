using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.transform.parent == null)
        {
            other.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    private float origSpeed;
    private void OnTransition(bool cutsceneActive)
    {
        if (cutsceneActive)
        {
            origSpeed = animator.speed;
            animator.speed = 0;
        }
        else
        {
            animator.speed = origSpeed;
        }
    }

    public void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.TRANSITION, OnTransition);
    }

    public void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION, OnTransition);
    }
}
