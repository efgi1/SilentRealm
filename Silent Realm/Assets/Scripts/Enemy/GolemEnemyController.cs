using UnityEngine;
using UnityEngine.AI;

public class GolemEnemyController : MonoBehaviour
{
    public float attackDistance = 1f;
    public float maxSoundDistance = 15f;
    public AudioClip swordSwing;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem explosionEffect;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    //TODO create actual states
    private bool chase = false;
    private bool attacking = false;
    private bool swordIsSwinging = false;

    private float attacktime = 2;
    private GolemStepEmitter stepEmitter;
    private AudioSource audioSource;

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger<bool>.AddListener(GameEvent.TRANSITION_GOLEM, OnTransitionGolem);
        Messenger.AddListener(GameEvent.GAME_LOST, OnGameLost);
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION_GOLEM, OnTransitionGolem);
        Messenger.RemoveListener(GameEvent.GAME_LOST, OnGameLost);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        agent.updatePosition = false;
        stepEmitter = GetComponent<GolemStepEmitter>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled || !anim.enabled)
            return;

        // Default State Inactive
        if (attacking)
        {
            // Attack State
            attacking = attacktime > 0;
            attacktime -= Time.deltaTime;
            return;
        }
        anim.SetBool("attack", false);

        if (chase)
        {
            // Chase State
            GameManager gm = GameManager.Instance;
            if (gm.PlayerNavMeshPosition != null)
            {
                agent.SetDestination(gm.PlayerNavMeshPosition.Value);
            }
            else
            {
                //Debug.Log("COULD NOT LOCATE PLAYER!" + Time.timeSinceLevelLoad);
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Wait for player State
                    var rotation = Quaternion.LookRotation(player.transform.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1);
                }
            }

            if (Vector3.Distance(transform.position, player.transform.position) < attackDistance && !attacking)
            {
                // Begin attack State
                attacktime = 1f;
                anim.SetBool("attack", true);
                anim.SetFloat("velx", 0);
                anim.SetFloat("vely", 0);
                agent.velocity = Vector3.zero;
                attacking = true;
            }
        }
        else
        {
            // Return state
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, 1);
            }
        }

        var localVel = transform.InverseTransformDirection(agent.velocity);
        anim.SetFloat("velx", localVel.z);
        anim.SetFloat("vely", localVel.x);


        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        if (worldDeltaPosition.magnitude > agent.radius / 3)
        {
            transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 position = anim.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
        agent.nextPosition = transform.position;
    }

    private void OnEnemyActiveChanged(bool enemyActive)
    {
        anim.SetBool("active", enemyActive);
        if (enemyActive)
        {
            agent.destination = player.transform.position;
            agent.isStopped = false;
        }
        else
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            Quaternion rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            Instantiate(explosionEffect, position, rotation);

            agent.isStopped = true;
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            agent.Warp(originalPosition);
        }
        chase = enemyActive;
    }

    private void OnTransitionGolem(bool playSound)
    {
        anim.SetBool("active", true);
    }

    private void OnGameLost()
    {
        anim.enabled = false;
        agent.enabled = false;
        audioSource.Stop();
    }

    private void Step()
    {
        float volume = 1 - (Vector3.Distance(player.transform.position, transform.position) / maxSoundDistance);
        volume = volume < 0 ? 0 : volume;
        //Debug.Log(volume);
        stepEmitter.EmitFootstep(volume);
    }

    // called from CollisionReporter on hitbox child
    public void HandlePlayerHit(Vector3 collisionPoint)
    {
        bool attacking = swordIsSwinging && anim.GetCurrentAnimatorStateInfo(0).IsName("SingleAttack");
        int damage = 1;
        if (attacking)
        {
            Instantiate(hitEffect, collisionPoint, Quaternion.identity);
            Messenger<int, Vector3>.Broadcast(GameEvent.PLAYER_DAMAGED, damage, collisionPoint);
        }
    }

    private void SwingSword()
    {
        audioSource.PlayOneShot(swordSwing);
    }

    public void SetSwinging(int swinging)
    {
        this.swordIsSwinging = swinging == 1 ? true : false;
    }
}
