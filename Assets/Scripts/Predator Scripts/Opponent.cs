using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : MonoBehaviour
{
    public Player player;
    public TerrainData td;
    public GhostTrail gt;
    public enum State { Unaware, Warned, Pursue }
    public State currentState;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator anim;

    private Material playerMaterial;
    private Color originalPlayerColor;
    private State previousState;

    public float moveSpeed = 3f;
    public float sprintSpeedMultiplier = 2f;

    //How long the opponent pauses before walking to the next random position.
    public float walkPauseDuration = 6f;
    private float walkPauseTimer;

    public Vector3 walkPosition;

    //How many frames back in the player position history to store as investigation target.
    public int framesAgo = 10;
    public Vector3 storedPosition;

    public bool Spotted;

    //How long the player must stay in unobstructed sight before the opponent enters Pursue.
    public float pursueThreshold = 7f;
    public float pursueSpeed = 8f;
    private float sightTimer;

    // True after SetDestination has been called for the current walkPosition.
    // Prevents calling SetDestination every frame and avoids the isStopped gate deadlock.
    private bool destinationDispatched = false;

    private const string WallTag = "Wall";
    private const string PlayerTag = "Player";

    //Minimum remaining NavMesh distance before the opponent is considered to have arrived.
    private const float ArrivalThreshold = 0.5f;
    private const float NavMeshSampleRadius = 20f;
    private const float MinWalkDistance = 3f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // Rigidbody must be kinematic and fully frozen so physics never displaces the
        // transform between NavMeshAgent update steps, which causes the snap-back effect.
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Root motion moves the transform via animation data, conflicting with NavMeshAgent's
        // own position updates and causing the opponent to snap/teleport every frame.
        if (anim != null)
        {
            anim.applyRootMotion = false;
        }

        agent.speed = moveSpeed;
        currentState = State.Unaware;
        previousState = State.Unaware;

        // Create a material instance on the player's mesh so the shared asset is never modified.
        SkinnedMeshRenderer playerRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (playerRenderer != null)
        {
            playerMaterial = playerRenderer.material;
            originalPlayerColor = playerMaterial.color;
        }

        ChooseRandomPos();
        // Override the pause timer so roaming starts immediately at game start.
        walkPauseTimer = 0f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Unaware:
                Unaware();
                break;
            case State.Warned:
                Warned();
                break;
            case State.Pursue:
                Pursue();
                break;
        }

        if (Spotted)
        {
            // Refresh storedPosition every frame so the opponent tracks the player's latest position.
            if (player.lastPosition.Count > framesAgo)
            {
                storedPosition = player.lastPosition[player.lastPosition.Count - framesAgo];
            }

            // Accumulate sight time. Only escalate to Pursue — never demote back to Warned once pursuing.
            if (currentState != State.Pursue)
            {
                currentState = State.Warned;
                sightTimer += Time.deltaTime;

                if (sightTimer >= pursueThreshold)
                {
                    currentState = State.Pursue;
                }
            }
        }
        else
        {
            // Player out of sight: reset the sight timer so it must be re-earned continuously.
            sightTimer = 0f;
        }

        // Return to Unaware once the investigation point is reached and the player is no longer visible.
        if (currentState == State.Warned && !Spotted && HasArrived())
        {
            currentState = State.Unaware;
            // Pick a fresh patrol target so re-entry is always in a clean state.
            ChooseRandomPos();
        }

        if (currentState != previousState)
        {
            OnStateChanged(currentState);
            previousState = currentState;
        }
        // Drive the walking animation from the NavMeshAgent's actual velocity.
        if (anim != null)
        {
            anim.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        }
    }
    private void OnStateChanged(State newState)
    {
        if (playerMaterial == null) return;

        switch (newState)
        {
            case State.Unaware:
                playerMaterial.color = originalPlayerColor;
                break;
            case State.Warned:
                playerMaterial.color = Color.yellow;
                break;
            case State.Pursue:
                playerMaterial.color = Color.red;
                break;
        }
    }

    private void Unaware()
    {
        if (!agent.isOnNavMesh) return;

        agent.speed = moveSpeed;

        if (walkPauseTimer > 0f)
        {
            walkPauseTimer -= Time.deltaTime;
            return;
        }

        // Dispatch the destination exactly once per walk cycle.
        if (!destinationDispatched)
        {
            agent.SetDestination(walkPosition);
            destinationDispatched = true;
        }

        if (HasArrived())
        {
            agent.ResetPath();
            ChooseRandomPos();
        }
    }
    public void ChooseRandomPos()
    {
        // Always reset the dispatch flag so the new destination gets sent on the next cycle.
        destinationDispatched = false;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            float randomX = Random.Range(-td.size.x / 4f, td.size.x / 4f);
            float randomZ = Random.Range(-td.size.z / 4f, td.size.z / 4f);
            Vector3 candidate = transform.position + new Vector3(randomX, 0f, randomZ);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, NavMeshSampleRadius, NavMesh.AllAreas)
                && Vector3.Distance(transform.position, hit.position) >= MinWalkDistance)
            {
                walkPosition = hit.position;
                walkPauseTimer = walkPauseDuration;
                return;
            }
        }

        // All attempts failed (e.g. NavMesh too sparse): wait a moment then retry.
        walkPauseTimer = 1f;
    }

    private void Warned()
    {
        anim.SetBool("onPursue", false);

        if (!agent.isOnNavMesh) return;

        agent.speed = moveSpeed * sprintSpeedMultiplier;
        agent.SetDestination(storedPosition);
    }

    private void Pursue()
    {
        if (!agent.isOnNavMesh) return;

        agent.speed = pursueSpeed;
        agent.SetDestination(player.transform.position);

        anim.SetBool("onPursue", true);

    }
    public void HearNoise(Vector3 position)
    {
        storedPosition = position;
        currentState = State.Warned;
    }
    private bool IsWallBetween(Vector3 target)
    {
        Vector3 eyeOffset = Vector3.up * 0.5f;
        Vector3 origin = transform.position + eyeOffset;
        Vector3 destination = target + eyeOffset;
        Vector3 direction = (destination - origin).normalized;
        float distance = Vector3.Distance(origin, destination);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            return hit.collider.CompareTag(WallTag);
        }

        return false;
    }
    private bool HasArrived()
    {
        return agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < ArrivalThreshold;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag) && !IsWallBetween(other.transform.position))
        {
            Spotted = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            // Re-confirm visibility every frame. Spotted resets to false each LateUpdate,
            // so losing LOS clears detection naturally without any explicit reset.
            Spotted = !IsWallBetween(other.transform.position);
        }
    }

    private void LateUpdate()
    {
        // Reset Spotted each frame so OnTriggerStay must actively re-confirm it.
        Spotted = false;
    }
}
