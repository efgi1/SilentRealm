using UnityEngine;

public class GhostEnemyController : MonoBehaviour
{
    public float speed = 3;
    public float attackDistance = 2f;
    public float rotationSpeed = 2f;

    public AudioClip sighSound;
    public AudioClip swingSound;

    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem explosionEffect;

    private GameObject player;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 destination;
    private bool chase = false;
    private Animator anim;
    private bool attacking = false;
    private float attacktime = 1f;
    private AudioSource audioSource;
    private bool hasSighed = false;
    private bool swordIsSwinging = false;

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.AddListener(GameEvent.TRANSITION_GHOST, OnTransitionGhost);
        Messenger.AddListener(GameEvent.GAME_LOST, OnGameLost);
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger.RemoveListener(GameEvent.TRANSITION_GHOST, OnTransitionGhost);
        Messenger.RemoveListener(GameEvent.GAME_LOST, OnGameLost);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        destination = originalPosition;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (attacking)
        {
            // Attack State
            attacking = attacktime > 0;
            attacktime -= Time.deltaTime;
            return;
        }
        anim.SetBool("Attack", false);

        if (chase)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < attackDistance && !attacking)
            {
                // Begin attack State
                attacktime = 1f;
                anim.SetBool("Attack", true);
                attacking = true;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < 4 && !hasSighed && chase)
            {
                audioSource.PlayOneShot(sighSound);
                hasSighed = true;
            }
        }

    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GameLost) return;
        float step = speed * Time.deltaTime;

        if (chase)
        {
            destination = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destination, step);

            Vector3 relativePos = destination - transform.position;
            if (relativePos != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnEnemyActiveChanged(bool enemyActive)
    {
        anim.SetBool("Active", enemyActive);
        if (enemyActive)
        {
            chase = true;
            anim.SetBool("Moving", true);
        }
        else
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            Quaternion rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            Instantiate(explosionEffect, position, rotation);

            chase = false;
            anim.SetBool("Moving", false);
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            hasSighed = false;
        }
    }

    private void OnTransitionGhost()
    {
        anim.SetBool("Active", true);
    }

    private void OnGameLost()
    {
        anim.enabled = false;
        audioSource.Stop();
    }

    public void HandlePlayerHit(Vector3 collisionPoint)
    {
        bool attacking = swordIsSwinging && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        int damage = 1;
        if (attacking)
        {
            Instantiate(hitEffect, collisionPoint, Quaternion.identity);
            Messenger<int, Vector3>.Broadcast(GameEvent.PLAYER_DAMAGED, damage, collisionPoint);
        }
    }

    private void PlaySwingSound()
    {
        audioSource.PlayOneShot(swingSound);
    }

    public void SetSwinging(int swinging)
    {
        this.swordIsSwinging = swinging == 1 ? true : false;
    }
}
