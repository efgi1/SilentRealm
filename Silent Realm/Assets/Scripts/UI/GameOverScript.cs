using System.Collections;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    private Animator animator;

    void OnEnable()
    {
        Messenger.AddListener(GameEvent.GAME_OVER_SCREEN, OnGameOverScreen);
    }

    void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.GAME_OVER_SCREEN, OnGameOverScreen);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnGameOverScreen()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.97f);
        animator.SetTrigger("FadeIn");
    }
}
