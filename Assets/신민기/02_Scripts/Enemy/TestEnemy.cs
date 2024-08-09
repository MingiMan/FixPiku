using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class TestEnemy : MonoBehaviour
{
    enum AIState
    {
        Idle,Patrolling,Chasing,Attacking
    }

    [Header("Pagrol")]
    [SerializeField] Transform wayPoints;
    [SerializeField] float waitAtPoint = 2f;
    int currentWayPoint;
    float waitCounter;

    [Header("Components")]
    [SerializeField] Animator animator;
    NavMeshAgent agent;

    [Header("AI States")]
    [SerializeField] AIState currentState;

    [Header("Chasing")]
    [SerializeField] float chaseRange;

    [Header("Suspicious")]
    [SerializeField] float suspiciousTime;
    float timeSinceLastSawPlayer;

    [Header("Attack")]
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackTime = 2f;
    float timeToAttack;

    GameObject player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        waitCounter = waitAtPoint;
        timeSinceLastSawPlayer = suspiciousTime;
        timeToAttack = attackTime;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch(currentState)
        {
            case AIState.Idle:

                if(waitCounter > 0)
                    waitCounter -= Time.deltaTime;

                else
                {
                    currentState = AIState.Patrolling;
                    agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
                }

                if (distanceToPlayer <= chaseRange)
                    currentState = AIState.Chasing;

                break;

            case AIState.Patrolling:
                if(agent.remainingDistance <= 0.2f)
                {
                    currentWayPoint++;
                    if(currentWayPoint >= wayPoints.childCount)
                    {
                        currentWayPoint = 0;
                    }
                    currentState = AIState.Idle;
                    waitCounter = waitAtPoint;
                }
                if (distanceToPlayer <= chaseRange)
                    currentState = AIState.Chasing;
                break;
            case AIState.Chasing:
                agent.SetDestination(player.transform.position);
                if(distanceToPlayer > chaseRange)
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    timeSinceLastSawPlayer -= Time.deltaTime;

                    if(timeSinceLastSawPlayer <= 0)
                    {
                        currentState = AIState.Idle;
                        timeSinceLastSawPlayer = suspiciousTime;
                        agent.isStopped = false;
                    }
                }

                if(distanceToPlayer <= attackRange)
                {
                    currentState = AIState.Attacking;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                }
                break;
            case AIState.Attacking:
                transform.LookAt(player.transform.position, Vector3.up);

                break;
        }
    }

    //protected virtual void PlayerDetaticon()
    //{
    //    sphereCastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("HOUSE"));

    //    foreach (var hit in sphereCastHits)
    //    {
    //        if (hit.collider.CompareTag("HOUSE"))
    //        {
    //            Vector3 directionToBase = (hit.transform.position - transform.position).normalized;
    //            Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
    //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.5f);
    //        }
    //    }
    //}
}
