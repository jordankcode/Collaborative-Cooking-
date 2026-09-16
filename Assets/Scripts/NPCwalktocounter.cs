using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCwalktocounter : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform chairSitPoint;
    public Plate assignedPlate; // the plate on this NPC's table — drag it in the Inspector
    public float eatingTime = 2f; // how long the NPC "eats" before standing up and walking off

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

    // lets ClickManager know whether a click should be treated as "deliver pizza" instead of "toggle walk"
    public bool IsWaitingForOrder()
    {
        return isSitting;
    }

    // called by ClickManager when the player clicks this NPC while it's sitting and waiting on an order
    public void ReceivePizza()
    {
        if (!isSitting) return;

        if (assignedPlate == null)
        {
            Debug.Log("No plate assigned to this NPC in the Inspector!");
            return;
        }

        if (!assignedPlate.HasPizza())
        {
            ObjectiveManager1.instance.ShowObjective("Bring me a pizza on the plate!");
            return;
        }

        PizzaTopping topping = assignedPlate.pizzaTopping;

        if (topping.IsPizzaAcceptable())
        {
            ObjectiveManager1.instance.ShowObjective("Order accepted! Thanks!");
            Destroy(assignedPlate.pizzaOnPlate);
            assignedPlate.ClearPlate();
            StartCoroutine(FinishOrderAfterDelay(eatingTime));
        }
        else
        {
            ObjectiveManager1.instance.ShowObjective("Rejected: " + topping.GetRejectionReason());
        }
    }

    IEnumerator FinishOrderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StandUp();
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