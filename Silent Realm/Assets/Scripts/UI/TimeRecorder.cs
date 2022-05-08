using UnityEngine;

public class TimeRecorder : MonoBehaviour
{
    private static TimeRecorder _instance;
    public static TimeRecorder Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public float totalPlayTime = 0.0f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnGameWon()
    {
        totalPlayTime = Time.timeSinceLevelLoad;
    }

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.GAME_WON, OnGameWon);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.GAME_WON, OnGameWon);
    }
}
