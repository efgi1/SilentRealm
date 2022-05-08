using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text orbsLabel;
    [SerializeField] Image timeBarMask;
    [SerializeField] Image scoopPanel;

    private bool nearPot;
    private bool savedNearPot;

    void OnEnable()
    {
        Messenger<int>.AddListener(GameEvent.SCORE_CHANGED, OnScoreChanged);
        Messenger<float>.AddListener(GameEvent.TIME_CHANGED, OnTimeChanged);
        Messenger<bool>.AddListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.AddListener(GameEvent.ALL_POTS_COLLECTED, OnAllPotsCollected);
        Messenger<bool>.AddListener(GameEvent.NEAR_POT, OnNearPot);
        Messenger<bool>.AddListener(GameEvent.TRANSITION, OnTransition);
    }

    void OnDisable()
    {
        Messenger<int>.RemoveListener(GameEvent.SCORE_CHANGED, OnScoreChanged);
        Messenger<float>.RemoveListener(GameEvent.TIME_CHANGED, OnTimeChanged);
        Messenger<bool>.RemoveListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.RemoveListener(GameEvent.ALL_POTS_COLLECTED, OnAllPotsCollected);
        Messenger<bool>.RemoveListener(GameEvent.NEAR_POT, OnNearPot);
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION, OnTransition);
    }

    void Start()
    {
        orbsLabel.text = getOrbsLabelText(0);
        timeBarMask.fillAmount = 1f;
    }

    private static string getOrbsLabelText(int score)
    {
        int scoreTarget = GameManager.Instance.initialCollectibleCount;
        return $"{score}/{scoreTarget}";
    }

    private void OnScoreChanged(int newScore)
    {
        orbsLabel.text = getOrbsLabelText(newScore);
    }

    private void OnTimeChanged(float remainingTime)
    {
        float maxTime = GameManager.Instance.maxTime;
        float progressRatio = remainingTime / maxTime;
        timeBarMask.fillAmount = progressRatio;
    }

    private void OnEnemyActiveChanged(bool chasePlayer)
    {
        if (chasePlayer)
        {
            // make scene dark, etc
        }
        else
        {
            RenderSettings.ambientLight = new Color(110 / 255f, 120 / 255f, 120 / 255f, 1f);
        }
    }

    private void OnAllPotsCollected()
    {
        orbsLabel.color = Color.green;
    }

    private void OnNearPot(bool nearPot)
    {
        this.nearPot = nearPot;
    }

    void Update()
    {
        if (GameManager.Instance.GameLost)
        {
            nearPot = false;
        }

        float alpha = nearPot ?
            Mathf.Min(1.0f, scoopPanel.color.a + Time.deltaTime * 8) :
            Mathf.Max(0.0f, scoopPanel.color.a - Time.deltaTime * 8);
        scoopPanel.color = new Color(scoopPanel.color.r, scoopPanel.color.b, scoopPanel.color.g, alpha);
    }

    void OnTransition(bool start)
    {
        if (start)
        {
            savedNearPot = nearPot;
            nearPot = false;
        }
        else
        {
            nearPot = savedNearPot;
        }
    }
}
