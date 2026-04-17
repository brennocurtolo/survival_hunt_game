using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent navAgent;

    private float detectionDistance = 30f;
    private float distanceToPlayer;

    [Header("Speed Settings")]
    public float walkSpeed = 4.5f;
    public float runSpeed = 7f;

    private float timeToSwitchMovement = 8f;
    private float lastSwitchTime = 0f;
    private bool isRunning = false;
    private bool isChasing = false;

    private Vector3 initialPosition;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position;
        navAgent.Warp(initialPosition);
    }

    public void HandleMovement()
    {
        if (player == null)
        {
            StopMovement();
            isChasing = false;
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionDistance)
        {
            isChasing = true;
            MoveTowardsPlayer();
            HandleRunAndWalkSwitch();
        }
        else
        {
            isChasing = false;
            ReturnToOrigin();
        }
    }

    void MoveTowardsPlayer()
    {
        navAgent.SetDestination(player.position);
        navAgent.speed = isRunning ? runSpeed : walkSpeed;
    }

    void ReturnToOrigin()
    {
        float distanceToHome = Vector3.Distance(transform.position, initialPosition);

        if (distanceToHome > 0.5f)
        {
            navAgent.SetDestination(initialPosition);
            navAgent.speed = walkSpeed;
        }
        else
        {
            StopMovement();
        }
    }

    void HandleRunAndWalkSwitch()
    {
        if (Time.time - lastSwitchTime > timeToSwitchMovement)
        {
            isRunning = !isRunning;
            lastSwitchTime = Time.time;
        }
    }

    void StopMovement()
    {
        navAgent.ResetPath();
    }

    public bool IsChasing => isChasing;
    public bool IsRunning => isRunning;
    public float DistanceToPlayer => distanceToPlayer;
    public float DetectionDistance => detectionDistance;
    public Vector3 Velocity => navAgent.velocity;
}