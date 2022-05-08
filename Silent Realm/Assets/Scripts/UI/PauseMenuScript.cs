using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuScript : MonoBehaviour
{
    public Button defaultSelectedButton;
    public GameObject controlsPanel;
    
    private CanvasGroup canvasGroup;
    private bool inCutscene = false;
    private PlayerInput input;

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.TRANSITION, OnTransition);
        input.Player.Enable();
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION, OnTransition);
        input.Player.Disable();
    }

    void Awake()
    {
        input = new PlayerInput();
        input.Player.Pause.performed += OnPause;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (GameManager.Instance.ReturningToTitle)
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0f;
        }
    }

    void OnPause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.ReturningToTitle)
        {
            return;
        }

        if (!inCutscene && !GameManager.Instance.GameLost)
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
                Messenger<bool>.Broadcast(GameEvent.PAUSED, false);
                controlsPanel.SetActive(false);
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
                Messenger<bool>.Broadcast(GameEvent.PAUSED, true);

                defaultSelectedButton.Select();
            }
        }
    }

    void OnTransition(bool start)
    {
        inCutscene = start;
    }
}
