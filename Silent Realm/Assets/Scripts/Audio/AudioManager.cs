using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip chaseClip;
    public AudioClip idleClip;
    public AudioClip gameWonClip;
    public AudioClip gameLostClip;
    public AudioClip spottedClip;
    public AudioClip startClip;
    public AudioClip buttonHoverClip;

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.TRANSITION, OnTransition);
        Messenger<bool>.AddListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.AddListener(GameEvent.GAME_WON, OnGameWon);
        Messenger.AddListener(GameEvent.GAME_LOST, OnGameLost);
        Messenger.AddListener(GameEvent.START_PRESSED, OnStartPressed);
        Messenger.AddListener(GameEvent.BUTTON_SELECTED, OnButtonSelected);
        Messenger<bool>.AddListener(GameEvent.PAUSED, OnPaused);
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION, OnTransition);
        Messenger<bool>.RemoveListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.RemoveListener(GameEvent.GAME_WON, OnGameWon);
        Messenger.RemoveListener(GameEvent.GAME_LOST, OnGameLost);
        Messenger.RemoveListener(GameEvent.START_PRESSED, OnStartPressed);
        Messenger.RemoveListener(GameEvent.BUTTON_SELECTED, OnButtonSelected);
        Messenger<bool>.RemoveListener(GameEvent.PAUSED, OnPaused);
    }

    private void OnTransition(bool beginning)
    {
        if (beginning && !GameManager.InReturnCutscene)
        {
            ChangeAudio(spottedClip);
        }
    }

    private void OnEnemyActiveChanged(bool chasePlayer)
    {
        audioSource.loop = true;
        if (chasePlayer)
        {
            ChangeAudio(chaseClip);
        }
        else
        {
            ChangeAudio(idleClip);
        }
    }

    private void OnGameWon()
    {
        audioSource.loop = false;
        ChangeAudio(gameWonClip);

        StartCoroutine(FadeAfterMusicEnds());
    }

    private void OnGameLost()
    {
        audioSource.loop = false;
        ChangeAudio(gameLostClip);
    }

    private void OnStartPressed()
    {
        audioSource.loop = false;
        ChangeAudio(startClip);
    }

    private void OnButtonSelected()
    {
        audioSource.PlayOneShot(buttonHoverClip);
    }

    private void OnPaused(bool paused)
    {
        if (paused)
        {
            audioSource.volume = 0.2f;
        }
        else
        {
            audioSource.volume = 1.0f;
        }
    }

    private IEnumerator FadeAfterMusicEnds()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        Messenger.Broadcast(GameEvent.SHOW_RESULTS_SCREEN);
    }

    public void ChangeAudio(AudioClip clip)
    {
        if (audioSource.clip.name == clip.name) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
