using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TitleWolfAnimation : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] Transform point1;
    [SerializeField] Transform point2;

    private Transform currentTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        currentTarget = point1;
        agent.SetDestination(currentTarget.position);
    }

    private void Update()
    {
        Running();
    }

    public void Running()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            currentTarget = (currentTarget == point1) ? point2 : point1;
            agent.SetDestination(currentTarget.position);
        }
    }
}
