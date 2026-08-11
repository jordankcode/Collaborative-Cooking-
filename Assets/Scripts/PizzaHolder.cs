using UnityEngine;

public class PizzaHolder : MonoBehaviour
{
    public static PizzaHolder instance;

    public bool isHoldingPizza = false;
    public GameObject currentPizza;
    public PizzaTopping currentPizzaTopping;

    void Awake()
    {
        instance = this;
    }

    public void PickUpPizza(GameObject pizza)
    {
        isHoldingPizza = true;
        currentPizza = pizza;
        currentPizzaTopping = pizza.GetComponent<PizzaTopping>();
        Debug.Log("Picked up pizza");
    }

    public void DropPizza()
    {
        isHoldingPizza = false;
        currentPizza = null;
        currentPizzaTopping = null;
        Debug.Log("Pizza delivered");
    }
}