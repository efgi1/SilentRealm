using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject playerLookAt;
    [SerializeField] GameObject playerRagdoll;
    [SerializeField] GameObject ragdollBody;
    [SerializeField] GameObject ragdollFollow;
    [SerializeField] GameObject glider;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject startingCircle;

    private const float MIN_RUN_SPEED = 0.65f;
    private const float MAX_RUN_SPEED = 0.85f;
    private float runSpeed = MIN_RUN_SPEED;

    private int health = 1;
    private bool alive = true;
    private bool won = false;
    private bool run = false;
    private float moveInput;
    private float turnInput;

    private Vector3 velocity;
    private Vector3 gravity;
    private CharacterController controller;
    private Animator animator;
    private PlayerInput input;
    private AudioSource audioSource;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private Vector3 localPlatformPosition;
    private bool alreadyOnPlatform = false;

    private bool _gliding = false;
    public bool Gliding
    {
        get => _gliding;
        set
        {
            _gliding = value;
            animator.SetBool("Gliding", value);
            glider.SetActive(value);
        }
    }

    public void OnEnable()
    {
        Messenger<int, Vector3>.AddListener(GameEvent.PLAYER_DAMAGED, OnPlayerDamaged);
        Messenger<bool>.AddListener(GameEvent.TRANSITION, OnTransition);
        Messenger.AddListener(GameEvent.GAME_WON, OnGameWon);
        input.Player.Enable();
    }

    public void OnDisable()
    {
        Messenger<int, Vector3>.RemoveListener(GameEvent.PLAYER_DAMAGED, OnPlayerDamaged);
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION, OnTransition);
        Messenger.RemoveListener(GameEvent.GAME_WON, OnGameWon);
        input.Player.Disable();
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        input = new PlayerInput();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        playerRagdoll.gameObject.SetActive(false);

        input.Player.Run.started += _ => run = true;
        input.Player.Run.canceled += _ => run = false;
        input.Player.Roll.performed += OnRoll;

        input.Player.Roll.performed += OnGlide;
        input.Player.Glide.performed += OnGlide;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        gravity = Physics.gravity;
        Gliding = false;
    }

    void Update()
    {
        if (!alive)
        {
            Vector3 ragdollPosition = ragdollBody.transform.position;
            playerLookAt.transform.position = new Vector3(ragdollPosition.x, ragdollPosition.y + 0.85f, ragdollPosition.z);
            ragdollFollow.transform.position = new Vector3(ragdollPosition.x, ragdollPosition.y, ragdollPosition.z);
        }

        if (!alive || won || inCutscene)
        {
            return;
        }

        HandleMoveInput();

        if (!Mathf.Approximately(turnInput, 0.0f))
        {
            float turnAmount = turnInput * turnSpeed * Time.deltaTime;
            Vector3 rotation = new Vector3(0f, turnAmount, 0f);
            transform.Rotate(rotation);
        }

        if (controller.enabled)
        {
            if (OnGround(0.2f) || controller.isGrounded)
            {
                velocity = Vector3.zero;
                Gliding = false;
            }
            else
            {
                if (Gliding)
                {
                    velocity = transform.forward * moveInput * 3.5f;
                    velocity += gravity * 0.036f;
                }
                else
                {
                    //velocity += gravity * Time.deltaTime;
                }
            }

            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    void OnAnimatorMove()
    {
        var parent = transform.parent;
        if (parent == null)
        {
            controller.enabled = true;
            alreadyOnPlatform = false;
            if (!Gliding)
            {
                animator.ApplyBuiltinRootMotion();
            }
            return;
        }

        if (alreadyOnPlatform)
        {
            // have already been on platform, maintain the same local position and add root motion movement
            localPlatformPosition += animator.deltaPosition;
        }
        else
        {
            // just became a child of the platform object, use localPosition on platform and add root motion movement
            localPlatformPosition = transform.localPosition;
            localPlatformPosition.y = 0f;
            controller.enabled = false;
            velocity = Vector3.zero;
            Gliding = false;
        }
        Vector3 rootTargetPosition = parent.position + localPlatformPosition; //+ animator.deltaPosition;
        transform.position = rootTargetPosition;
        alreadyOnPlatform = true;

        /* Debug.Log(parent.position);
        Debug.Log(localPlatformPosition);
        Debug.Log(animator.deltaPosition);
        Debug.Log("---------------------"); */
    }

    void ToggleDeath(Vector3 collisionPoint)
    {
        alive = !alive;

        playerModel.gameObject.SetActive(alive);
        playerRagdoll.gameObject.SetActive(!alive);
        controller.enabled = alive;
        animator.enabled = alive;
        transform.parent = null;
        velocity = Vector3.zero;

        if (!alive)
        {
            GameManager.Instance.GameLost = true;
            audioSource.Play();
            animator.Play("Idle");
            //CopyTransform(playerModel.transform, playerRagdoll.transform, force);
            CopyTransform(playerModel.transform, playerRagdoll.transform, collisionPoint);

            virtualCamera.Follow = ragdollFollow.transform;
            virtualCamera.LookAt = playerLookAt.transform;

            CinemachineComponentBase componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is Cinemachine3rdPersonFollow)
            {
                var ThirdPersonCam = componentBase as Cinemachine3rdPersonFollow;
                ThirdPersonCam.CameraDistance = 3.0f;
                ThirdPersonCam.VerticalArmLength = 1.1f;
            }
        }
    }

    void CopyTransform(Transform source, Transform destination, Vector3 collisionPoint)
    {
        for (int i = 0; i < source.childCount; i++)
        {
            Transform nextSourceChild = source.GetChild(i);
            Transform nextDestChild = destination.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;

            Rigidbody destRB = nextDestChild.GetComponent<Rigidbody>();
            if (destRB != null && destRB.name == "Body")
            {
                Vector3 hitDirection = transform.position - collisionPoint;
                hitDirection.y = 0;
                Vector3 force = hitDirection.normalized * 2.5f;
                destRB.velocity = force;
                //destRB.AddForce(force, ForceMode.VelocityChange);
                //Debug.DrawRay(collisionPoint, force, Color.green, 5f, false);

                //destRB.AddExplosionForce(8f, collisionPoint, 3f);
            }

            CopyTransform(nextSourceChild, nextDestChild, collisionPoint);
        }
    }

    void HandleMoveInput()
    {
        Vector2 inputVector = input.Player.Move.ReadValue<Vector2>();
        moveInput = inputVector.y;
        turnInput = inputVector.x;

        // temp solution for arrow keys
        float targetSpeed = run ? MAX_RUN_SPEED : MIN_RUN_SPEED;
        float smoothing = 0.3f * Time.deltaTime;
        if (runSpeed < targetSpeed)
        {
            runSpeed = Mathf.Min(targetSpeed, runSpeed + smoothing);
        }
        else if (runSpeed > targetSpeed)
        {
            runSpeed = Mathf.Max(targetSpeed, runSpeed - smoothing);
        }

        float moveVel = moveInput > 0 ? runSpeed : -runSpeed;
        animator.SetFloat("VelZ", Mathf.Approximately(moveInput, 0.0f) ? 0f : moveVel);
    }

    void OnRoll(InputAction.CallbackContext context)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree - Forward"))
        {
            animator.SetTrigger("PerformRoll");
        }
    }

    void OnGlide(InputAction.CallbackContext context)
    {
        if (Gliding)
        {
            Gliding = false;
            return;
        }

        if (!OnGround(1.0f))
        {
            Gliding = true;
            animator.ResetTrigger("PerformRoll");
        }
    }

    private float prevAnimSpeed = 0.0f;
    private bool inCutscene = false;

    private void OnTransition(bool beginning)
    {
        if (beginning)
        {
            prevAnimSpeed = animator.speed;
            animator.speed = 0f;
            //animator.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            //input.Player.Disable();
            input.Player.Glide.Disable();
            //input.Player.Move.Disable();
            input.Player.Pause.Disable();
            input.Player.Roll.Disable();
            input.Player.Scoop.Disable();

            inCutscene = true;
        }
        else
        {
            //input.Player.Enable();
            input.Player.Glide.Enable();
            //input.Player.Move.Enable();
            input.Player.Pause.Enable();
            input.Player.Roll.Enable();
            input.Player.Scoop.Enable();

            //animator.enabled = true;
            animator.speed = prevAnimSpeed;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            
            inCutscene = false;
        }
    }

    private void OnPlayerDamaged(int damage, Vector3 collisionPoint)
    {
        //GameObject obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), collisionPoint, Quaternion.identity);
        //obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        health -= 1;
        if (health == 0)
        {
            rb.isKinematic = true;
            StartCoroutine(RespawnAfterDeath(collisionPoint));
        }
    }

    private IEnumerator RespawnAfterDeath(Vector3 collisionPoint)
    {
        // change to ragdoll
        ToggleDeath(collisionPoint);

        // load game over screen
        yield return new WaitForSeconds(5.5f);
        Messenger.Broadcast(GameEvent.GAME_OVER_SCREEN);
    }

    public void OnGameWon()
    {
        if (startingCircle != null)
        {
            Gliding = false;
            rb.isKinematic = true;
            rb.detectCollisions = false;
            capsuleCollider.enabled = false;
            input.Player.Disable();
            StartCoroutine(Victory());
        }
        else
        {
            Debug.Log("PlayerController needs StartingCircle object for winning scene.");
        }
    }

    private IEnumerator Victory()
    {
        won = true;
        animator.SetFloat("VelZ", 0.0f);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        yield return new WaitForSeconds(0.5f);

        var circleMid = startingCircle.gameObject.transform;

        Quaternion lookAtMiddle = Quaternion.LookRotation(circleMid.transform.position - transform.position);
        while (1 - Mathf.Abs(Quaternion.Dot(transform.rotation, lookAtMiddle)) > 0.00001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtMiddle, Time.deltaTime * 2.5f);
            yield return null;
        }
        //transform.rotation = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z));

        yield return new WaitForSeconds(1.0f);

        animator.SetFloat("VelZ", 0.1f);
        while (Vector3.Distance(transform.position, circleMid.position) > 0.5f)
        {
            yield return null;
        }
        animator.SetFloat("VelZ", 0.0f);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        transform.position = new Vector3(circleMid.position.x, transform.position.y, circleMid.position.z);
    }

    private bool OnGround(float distance)
    {
        float rayOriginOffset = 0.1f;
        float rayDepth = distance;
        Vector3 charPos = transform.position;

        float totalRayLen = rayOriginOffset + rayDepth;
        Ray ray = new Ray(charPos + Vector3.up * rayOriginOffset, Vector3.down);

        int layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("LightObstructor")) | (1 << LayerMask.NameToLayer("Door"));
        RaycastHit[] hits = Physics.RaycastAll(ray, totalRayLen, layerMask, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.gameObject.CompareTag("Enemy") && !hit.collider.gameObject.CompareTag("NPC"))
            {
                return true;
            }
        }
        return false;
    }
}
