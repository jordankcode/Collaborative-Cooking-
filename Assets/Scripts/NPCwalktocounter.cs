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

        if (atPointA)
        {
            atPointA = false;
            agent.enabled = true;
            animator.SetBool("IsSitting", false);
            animator.SetBool("IsWalking", true); // force it on immediately
            agent.SetDestination(pointB.position);
            ObjectiveManager.instance.ShowObjective("Order: Make a Pepperoni Pizza!");
            StartCoroutine(WaitUntilArrived(pointB.position, OnArriveAtCounter));
        }
        else
        {
            atPointA = true;
            agent.enabled = true;
            animator.SetBool("IsSitting", false);
            animator.SetBool("IsWalking", true); // force it on immediately
            agent.SetDestination(pointA.position);
            ObjectiveManager.instance.HideObjective();
        }
    }

    IEnumerator WaitUntilArrived(Vector3 destination, System.Action onArrived)
    {
        // Wait for path to start calculating
        yield return new WaitForSeconds(0.5f);

        // Wait until agent is close enough
        while (agent.enabled && agent.remainingDistance > agent.stoppingDistance + 0.1f)
        {
            yield return null;
        }

        onArrived?.Invoke();
    }

    void OnArriveAtCounter()
    {
        isSitting = true;
        agent.enabled = false;

        // Snap to sit position
        if (chairSitPoint != null)
        {
            transform.position = chairSitPoint.position;
            transform.rotation = chairSitPoint.rotation;
        }

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSitting", true);
        Debug.Log("NPC arrived and sitting");
    }

    public void StandUp()
    {
        isSitting = false;
        animator.SetBool("IsSitting", false);
        agent.enabled = true;
        atPointA = true;
        agent.SetDestination(pointA.position);
        ObjectiveManager.instance.HideObjective();
    }
}