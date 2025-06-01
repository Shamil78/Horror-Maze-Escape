using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public float idleTime = 2f;
    public float walkSpeed = .5f;
    public float chaseSpeed = .4f;
    public float sightDistance = 10f;
    public float loseSightTime = 3f; // Time before returning to patrol
    public float damageInterval = 1f; // Damage every second on contact

    public AudioClip idleSound;
    public AudioClip walkingSound;
    public AudioClip chasingSound;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private float idleTimer = 0f;
    private Transform player;
    private AudioSource audioSource;
    private float lostSightTimer = 0f;
    private float lastDamageTime = 0f; // Timer to track damage intervals

    private enum EnemyState { Idle, Walk, Chase }
    private EnemyState currentState = EnemyState.Idle;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent.acceleration = 2f; // Slower reaction to speed changes
        agent.stoppingDistance = 2f;

        // Ensure enemy finds the player at runtime
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (waypoints.Length > 0)
        {
            SetDestinationToWaypoint();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                idleTimer += Time.deltaTime;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false);
                PlaySound(idleSound);

                if (idleTimer >= idleTime)
                {
                    NextWaypoint();
                }
                DetectPlayer();
                break;

            case EnemyState.Walk:
                idleTimer = 0f;
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsChasing", false);
                PlaySound(walkingSound);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = EnemyState.Idle;
                }
                DetectPlayer();
                break;

            case EnemyState.Chase:
                lostSightTimer += Time.deltaTime;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
                animator.SetBool("IsChasing", true);
                animator.SetBool("IsWalking", false);
                PlaySound(chasingSound);

                if (!DetectPlayer())
                {
                    if (lostSightTimer >= loseSightTime)
                    {
                        currentState = EnemyState.Walk;
                        agent.speed = walkSpeed;
                        SetDestinationToWaypoint();
                    }
                }
                else
                {
                    lostSightTimer = 0f; // Reset lost sight timer if player is seen again
                }
                break;
        }
    }

    private bool DetectPlayer()
    {
        if (player == null) return false;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.position - transform.position).normalized, sightDistance);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                currentState = EnemyState.Chase;
                return true;
            }
        }
        return false;
    }

    private void PlaySound(AudioClip soundClip)
    {
        if (audioSource.clip != soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    private void NextWaypoint()
    {
        if (waypoints.Length == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        SetDestinationToWaypoint();
    }

    private void SetDestinationToWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentState = EnemyState.Walk;
        agent.speed = walkSpeed;
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    // 🔴 NEW: Apply 1 point damage when enemy touches player
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && Time.time - lastDamageTime >= damageInterval)
            {
                playerHealth.TakeDamage(15); // Reduce 1 HP per second
                playerHealth.IncreaseSanity(15);
                lastDamageTime = Time.time; // Reset timer
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = currentState == EnemyState.Chase ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }
}
