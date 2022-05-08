using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    public Button startGameButton;
    public Animator anim;

    void Start()
    {
        startGameButton.onClick.AddListener(FadeOut);
    }

    private void FadeOut()
    {
        Messenger.Broadcast(GameEvent.START_PRESSED);
        StartCoroutine(LoadGameeAfterFade());
    }

    private IEnumerator LoadGameeAfterFade()
    {
        anim.SetTrigger("FadeOut");
        float animLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        ChangeToGameScene();
    }

    public void ChangeToGameScene()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
