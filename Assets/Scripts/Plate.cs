using UnityEngine;

// Put this script on the PLATE object on the counter/table.
// The plate needs a Collider with "Is Trigger" ticked ON, sized to sit just above the plate surface.
// The pizza object needs the tag "Pizza" set on it (same tag SausageStick already checks for).

public class Plate : MonoBehaviour
{
    [Header("Read-only — shows what's currently on the plate")]
    public GameObject pizzaOnPlate;
    public PizzaTopping pizzaTopping;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            pizzaOnPlate = other.gameObject;
            pizzaTopping = other.GetComponent<PizzaTopping>();
            Debug.Log("Pizza placed on plate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == pizzaOnPlate)
        {
            Debug.Log("Pizza removed from plate");
            ClearPlate();
        }
    }

    public bool HasPizza()
    {
        return pizzaOnPlate != null && pizzaTopping != null;
    }

    public void ClearPlate()
    {
        pizzaOnPlate = null;
        pizzaTopping = null;
    }
}
