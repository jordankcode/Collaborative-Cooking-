using UnityEngine;
using UnityEngine.AI;

public class NPCwalktocounter : MonoBehaviour
{
    public Transform counterPoint;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.SetDestination(counterPoint.position);
    }

    void Update()
    {
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isMoving);
    }
}