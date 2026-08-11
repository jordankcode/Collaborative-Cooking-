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

        if (ObjectiveManager1.instance == null)
        {
            Debug.Log("ObjectiveManager1 not found in scene");
            return;
        }

        if (atPointA)
        {
            atPointA = false;
            agent.enabled = true;
            animator.SetBool("IsSitting", false);
            animator.SetBool("IsWalking", true);
            agent.SetDestination(pointB.position);
            ObjectiveManager1.instance.ShowObjective("Order: Make a Pepperoni Pizza!");
            StartCoroutine(WaitUntilArrived(pointB.position, OnArriveAtChair));
        }
        else
        {
            atPointA = true;
            agent.enabled = true;
            animator.SetBool("IsSitting", false);
            animator.SetBool("IsWalking", true);
            agent.SetDestination(pointA.position);
            ObjectiveManager1.instance.HideObjective();
        }
    }

    IEnumerator WaitUntilArrived(Vector3 destination, System.Action onArrived)
    {
        yield return new WaitForSeconds(0.8f);
        Debug.Log("Checking distance to chair");

        while (Vector3.Distance(transform.position, destination) > 2f)
        {
            Debug.Log("Current distance: " + Vector3.Distance(transform.position, destination));
            yield return new WaitForSeconds(0.5f); // log every 0.5 seconds not every frame
        }

        Debug.Log("Distance threshold reached - calling OnArriveAtChair");
        onArrived?.Invoke();
    }

    void OnArriveAtChair()
    {
        Debug.Log("OnArriveAtChair fired");
        isSitting = true;
        agent.enabled = false;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSitting", true);
        Debug.Log("Animator IsSitting = " + animator.GetBool("IsSitting"));
        StartCoroutine(SnapToSitPosition());
    }

    IEnumerator SnapToSitPosition()
    {
        yield return new WaitForSeconds(0.1f);

        if (chairSitPoint != null)
        {
            transform.position = chairSitPoint.position;
            transform.rotation = chairSitPoint.rotation;
            Debug.Log("Snapped to chair position");
        }
        else
        {
            Debug.Log("CHAIRSITPOINT IS NULL - assign it in Inspector");
        }
    }

    public void StandUp()
    {
        isSitting = false;
        animator.SetBool("IsSitting", false);
        agent.enabled = true;
        atPointA = true;
        agent.SetDestination(pointA.position);
        ObjectiveManager1.instance.HideObjective();
    }

}