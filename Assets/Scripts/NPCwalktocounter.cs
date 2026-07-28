using UnityEngine;
using UnityEngine.AI;

public class NPCwalktocounter : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentTarget = pointA;
        agent.SetDestination(pointA.position);
    }

    void Update()
    {
        bool isMoving = agent.velocity.magnitude > 0.01f;
        animator.SetBool("IsWalking", isMoving);
    }

    public void Toggle()
    {
        if (currentTarget == pointA)
        {
            currentTarget = pointB;
            agent.SetDestination(pointB.position);
            ObjectiveManager.instance.ShowObjective("Order: Make a Pepperoni Pizza!");
        }
        else
        {
            currentTarget = pointA;
            agent.SetDestination(pointA.position);
            ObjectiveManager.instance.HideObjective();
        }
    }
}