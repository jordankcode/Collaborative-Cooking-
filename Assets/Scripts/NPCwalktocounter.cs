using UnityEngine;
using UnityEngine.AI;

public class NPCWalkToCounter : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private NavMeshAgent agent;
    private Animator animator;
    private bool atPointA = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (pointA != null)
            agent.SetDestination(pointA.position);
    }

    void Update()
    {
        if (agent != null)
        {
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsWalking", isMoving);
        }
    }

    public void Toggle()
    {
        NavMeshHit navHit;

        if (atPointA)
        {
            if (NavMesh.SamplePosition(pointB.position, out navHit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
                atPointA = false;
                Debug.Log("Moving to Point B");
            }
            else
            {
                Debug.Log("Point B not on NavMesh");
            }
        }
        else
        {
            if (NavMesh.SamplePosition(pointA.position, out navHit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
                atPointA = true;
                Debug.Log("Moving to Point A");
            }
            else
            {
                Debug.Log("Point A not on NavMesh");
            }
        }
    }
}