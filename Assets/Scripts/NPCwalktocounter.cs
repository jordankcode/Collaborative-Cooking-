using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCwalktocounter : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform chairSitPoint;

    private NavMeshAgent agent;
    private Animator animator;
    private bool atPointA = true;
    private bool isSitting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.SetDestination(pointA.position);
    }

    void Update()
    {
        if (agent.enabled)
        {
            bool isMoving = agent.remainingDistance > agent.stoppingDistance
                            && !agent.pathPending;
            animator.SetBool("IsWalking", isMoving);
        }
    }

    public void Toggle()
    {
        if (isSitting) return;

        if (ObjectiveManager.instance == null)
        {
            Debug.Log("ObjectiveManager not found in scene");
            return;
        }

        if (atPointA)
        {
            atPointA = false;
            agent.enabled = true;
            animator.SetBool("IsSitting", false);
            animator.SetBool("IsWalking", true);
            agent.SetDestination(pointB.position);
            ObjectiveManager.instance.ShowObjective("Order: Make a Pepperoni Pizza!");
            StartCoroutine(WaitUntilArrived(pointB.position, OnArriveAtChair));
        }
        else
        {
            atPointA = true;
            agent.enabled = true;
            animator.SetBool("IsSitting", false);
            animator.SetBool("IsWalking", true);
            agent.SetDestination(pointA.position);
            ObjectiveManager.instance.HideObjective();
        }
    }

    IEnumerator WaitUntilArrived(Vector3 destination, System.Action onArrived)
    {
        yield return new WaitForSeconds(0.8f);

        while (Vector3.Distance(transform.position, destination) > 1f)
        {
            yield return null;
        }

        onArrived?.Invoke();
    }

    void OnArriveAtChair()
    {
        isSitting = true;
        agent.enabled = false;
    }
}