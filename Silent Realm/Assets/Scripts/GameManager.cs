using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int initialCollectibleCount;

    private GameObject mainCamera;
    private GameObject golemCamera;
    private GameObject ghostCamera;
    private GameObject torchCamera;
    private GameObject circleCamera;

    private GameObject golemEye;
    private GameObject ghostEye1;
    private GameObject ghostEye2;

    private GameObject player;
    private PlayerInput input;
    private bool playedFullCutscene = false;

    public static bool InReturnCutscene = false;
    public bool ReturningToTitle = false;
    public bool ShowCutscenes { get; set; }

    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            bool valueChanged = _score != value;
            _score = value;

            if (valueChanged)
            {
                Messenger<int>.Broadcast(GameEvent.SCORE_CHANGED, value);
                TimeRemaining = maxTime;
                EnemyActive = false;

                if (value == initialCollectibleCount)
                {
                    AllPotsCollected = true;
                }
            }
        }
    }

    public float maxTime = 10f;
    private float _timeRemaining;
    public float TimeRemaining
    {
        get => _timeRemaining;
        set
        {
            bool valueChanged = _timeRemaining != value;
            _timeRemaining = value;

            if (valueChanged)
            {
                Messenger<float>.Broadcast(GameEvent.TIME_CHANGED, value);

                if (value == 0)
                {
                    StartCoroutine(Transition());
                }
            }
        }
    }

    private IEnumerator Transition()
    {
        // Sound effect.
        Messenger<bool>.Broadcast(GameEvent.TRANSITION, true);
        RenderSettings.ambientLight = new Color(100 / 255f, 90 / 255f, 90 / 255f, 1f);

        if (ShowCutscenes)
        {
            if (!playedFullCutscene)
            {
                playedFullCutscene = true;

                // Golem.
                switchCamera(golemCamera);
                yield return new WaitForSeconds(0.5f);
                setEye(golemEye, true);
                Messenger<bool>.Broadcast(GameEvent.TRANSITION_GOLEM, true);
                yield return new WaitForSeconds(1.5f);

                // Ghost.
                switchCamera(ghostCamera);
                setEye(golemEye, false);
                yield return new WaitForSeconds(0.5f);
                setEye(ghostEye1, true);
                setEye(ghostEye2, true);
                Messenger.Broadcast(GameEvent.TRANSITION_GHOST);
                yield return new WaitForSeconds(1.5f);

                // Torch.
                switchCamera(torchCamera);
                setEye(ghostEye1, false);
                setEye(ghostEye2, false);
                yield return new WaitForSeconds(1);
                Messenger.Broadcast(GameEvent.TRANSITION_TORCH);
                yield return new WaitForSeconds(1);
            }
            else
            {
                Camera golemCam = golemCamera.GetComponent<Camera>();
                Camera ghostCam = ghostCamera.GetComponent<Camera>();
                Camera torchCam = torchCamera.GetComponent<Camera>();
                AudioListener golemListener = golemCamera.GetComponent<AudioListener>();
                AudioListener ghostListener = ghostCamera.GetComponent<AudioListener>();
                AudioListener torchListener = torchCamera.GetComponent<AudioListener>();

                golemCam.rect = new Rect(0f, 0f, 0.334f, 1f);
                ghostCam.rect = new Rect(0.334f, 0f, 0.333f, 1f);
                torchCam.rect = new Rect(0.667f, 0f, 0.333f, 1f);

                golemListener.enabled = false;
                ghostListener.enabled = true;
                torchListener.enabled = false;

                mainCamera.SetActive(false);
                golemCamera.SetActive(true);
                ghostCamera.SetActive(true);
                torchCamera.SetActive(true);

                yield return new WaitForSeconds(0.5f);

                setEye(golemEye, true);
                setEye(ghostEye1, true);
                setEye(ghostEye2, true);
                Messenger<bool>.Broadcast(GameEvent.TRANSITION_GOLEM, false);
                Messenger.Broadcast(GameEvent.TRANSITION_GHOST);

                yield return new WaitForSeconds(0.5f);

                ghostListener.enabled = false;
                torchListener.enabled = true;
                Messenger.Broadcast(GameEvent.TRANSITION_TORCH);

                yield return new WaitForSeconds(1f);

                golemListener.enabled = false;
                ghostListener.enabled = false;
                torchListener.enabled = false;

                setEye(golemEye, false);
                setEye(ghostEye1, false);
                setEye(ghostEye2, false);
            }

            switchCamera(mainCamera);
        }
        else
        {
            Messenger.Broadcast(GameEvent.TRANSITION_TORCH);
        }
        // Start chase.
        EnemyActive = true;
        Messenger<bool>.Broadcast(GameEvent.TRANSITION, false);
    }

    private IEnumerator ReturnCutscene()
    {
        if (ShowCutscenes)
        {
            InReturnCutscene = true;
            Messenger<bool>.Broadcast(GameEvent.TRANSITION, true);
            switchCamera(circleCamera);
            yield return new WaitForSeconds(3f);
            switchCamera(mainCamera);
            Messenger<bool>.Broadcast(GameEvent.TRANSITION, false);
            InReturnCutscene = false;
        }
    }

    private void switchCamera(GameObject camera)
    {
        mainCamera.SetActive(false);
        golemCamera.SetActive(false);
        ghostCamera.SetActive(false);
        torchCamera.SetActive(false);
        circleCamera.SetActive(false);
        camera.SetActive(true);
    }

    private void setEye(GameObject eye, bool on)
    {
        eye.SetActive(on);
    }

    private bool _enemyActive = false;
    public bool EnemyActive
    {
        get => _enemyActive;
        set
        {
            bool valueChanged = _enemyActive != value;
            _enemyActive = value;
            if (valueChanged)
            {
                Messenger<bool>.Broadcast(GameEvent.ENEMY_ACTIVE_CHANGED, value);
            }
        }
    }

    private bool _started = false;
    public bool Started
    {
        get => _started;
        set
        {
            bool valueChanged = _started != value;
            _started = value;
            if (valueChanged)
            {
                TimeRemaining = 0;
            }
        }
    }

    private bool _allPotsCollected = false;
    public bool AllPotsCollected
    {
        get => _allPotsCollected;
        set
        {
            _allPotsCollected = value;
            if (value == true)
            {
                Messenger.Broadcast(GameEvent.ALL_POTS_COLLECTED);
                StartCoroutine(ReturnCutscene());
            }
        }
    }

    private bool _gameWon = false;
    public bool GameWon
    {
        get => _gameWon;
        set
        {
            _gameWon = value;
            if (value == true)
            {
                EnemyActive = false;
                Messenger.Broadcast(GameEvent.GAME_WON);
            }
        }
    }

    private bool _gameLost = false;
    public bool GameLost
    {
        get => _gameLost;
        set
        {
            _gameLost = value;
            if (value == true)
            {
                Messenger.Broadcast(GameEvent.GAME_LOST);
            }
        }
    }

    public Vector3? PlayerNavMeshPosition { get; set; }

    void Awake()
    {
        input = new PlayerInput();
        input.Player.Cutscene.performed += OnCutsceneToggle;
    }

    void Start()
    {
        instance = this;
        TimeRemaining = maxTime;
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Pot");
        initialCollectibleCount = collectibles.Length;

        mainCamera = GameObject.FindWithTag("MainCamera");
        golemCamera = GameObject.FindWithTag("GolemCamera");
        ghostCamera = GameObject.FindWithTag("GhostCamera");
        torchCamera = GameObject.FindWithTag("TorchCamera");
        circleCamera = GameObject.FindWithTag("CircleCamera");
        switchCamera(mainCamera);

        golemEye = GameObject.FindWithTag("GolemEye");
        ghostEye1 = GameObject.FindWithTag("GhostEye1");
        ghostEye2 = GameObject.FindWithTag("GhostEye2");
        setEye(golemEye, false);
        setEye(ghostEye1, false);
        setEye(ghostEye2, false);

        ShowCutscenes = true;
    }

    void OnCutsceneToggle(InputAction.CallbackContext context)
    {
        ShowCutscenes = !ShowCutscenes;

        string toggleText = ShowCutscenes ? "ON" : "OFF";
        Debug.Log("Transition Cutscenes: " + toggleText);
    }

    void Update()
    {
        if (Started && !AllPotsCollected)
        {
            TimeRemaining = Mathf.Max(0, TimeRemaining - Time.deltaTime);
        }

        PlayerNavMeshPosition = null;
        if (EnemyActive)
        {
            for (int i = 0; i < 10; i++)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(player.transform.position, out hit, 1.0f + i, NavMesh.AllAreas))
                {
                    PlayerNavMeshPosition = hit.position;
                    break;
                }
            }
        }
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }
}
