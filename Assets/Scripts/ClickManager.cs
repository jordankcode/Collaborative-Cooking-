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
        if (Camera.main == null)
        {
            Debug.Log("No main camera found");
            return;
        }

        if (Mouse.current == null)
        {
            Debug.Log("No mouse found");
            return;
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            NPCwalktocounter npc = hit.collider.GetComponentInParent<NPCwalktocounter>();
            if (npc != null)
            {
                npc.Toggle();
            }
        }
    }
}