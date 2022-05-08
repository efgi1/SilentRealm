using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeEndGame : MonoBehaviour
{
    public Animator fadeAnimator;

    private void OnGameOverScreen()
    {
        StartCoroutine(RespawnAfterGameOver());
    }

    public void OnReturnToTitle()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAfterFade("Title"));
    }

    public void OnShowResultsScreen()
    {
        StartCoroutine(LoadSceneAfterFade("Results"));
    }

    private IEnumerator LoadSceneAfterFade(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            GameManager.Instance.ReturningToTitle = true;
            Messenger.Broadcast(GameEvent.DISSOLVE_PLAYER);
            yield return new WaitForSeconds(1.5f);
        }

        fadeAnimator.SetTrigger("FadeOut");
        float animLength = fadeAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength * 2);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator RespawnAfterGameOver()
    {
        fadeAnimator.speed = 0.25f;
        fadeAnimator.SetTrigger("FadeOut");
        float animLength = fadeAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(9.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnEnable()
    {
        Messenger.AddListener(GameEvent.RETURN_TO_TITLE, OnReturnToTitle);
        Messenger.AddListener(GameEvent.SHOW_RESULTS_SCREEN, OnShowResultsScreen);
        Messenger.AddListener(GameEvent.GAME_OVER_SCREEN, OnGameOverScreen);
    }

    public void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.RETURN_TO_TITLE, OnReturnToTitle);
        Messenger.RemoveListener(GameEvent.SHOW_RESULTS_SCREEN, OnShowResultsScreen);
        Messenger.RemoveListener(GameEvent.GAME_OVER_SCREEN, OnGameOverScreen);
    }
}
