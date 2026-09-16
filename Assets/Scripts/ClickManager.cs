using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : MonoBehaviour
{
    public InputActionReference interactAction;

    void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            NPCwalktocounter npc = hit.collider.GetComponentInParent<NPCwalktocounter>();
            if (npc != null)
            {
                if (npc.IsWaitingForOrder())
                {
                    npc.ReceivePizza(); // NPC is sitting waiting on an order — try to deliver
                }
                else
                {
                    npc.Toggle(); // otherwise, normal walk-to-counter / walk-back toggle
                }
            }
        }
    }
}