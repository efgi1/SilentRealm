using UnityEngine;
using UnityEngine.InputSystem;

public class PotScoop : MonoBehaviour
{
    public bool active = true;
    public bool playerNear = false;

    private PlayerInput input;
    private PotEmitter potEmitter;
    private bool inCutscene = false;

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
        input.Player.Scoop.performed += OnScoop;
        potEmitter = GetComponent<PotEmitter>();
    }

    void OnScoop(InputAction.CallbackContext context)
    {
        if (playerNear && active && !inCutscene && !GameManager.Instance.GameLost)
        {
            potEmitter.EmitPot();
            Destroy(this.transform.GetChild(0).gameObject);
            Destroy(this.transform.GetChild(1).gameObject);
            active = false;
            Messenger<bool>.Broadcast(GameEvent.NEAR_POT, false);
            GameManager.Instance.Score++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerNear = true;
            if (active)
            {
                Messenger<bool>.Broadcast(GameEvent.NEAR_POT, true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerNear = false;
            Messenger<bool>.Broadcast(GameEvent.NEAR_POT, false);
        }
    }

    void OnTransition(bool start)
    {
        inCutscene = start;
    }
}
