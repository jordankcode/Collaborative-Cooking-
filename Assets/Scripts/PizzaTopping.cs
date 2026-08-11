using UnityEngine;

public class PizzaTopping : MonoBehaviour
{
    // Current topping counts
    public int pepperoniCount = 0;
    public int cheeseCount = 0;
    public int sauceCount = 0;

    // Required amounts for the order
    public int requiredPepperoni = 3;
    public int requiredCheese = 2;
    public int requiredSauce = 1;

    // Max allowed before NPC rejects
    public int maxPepperoni = 5;
    public int maxCheese = 4;
    public int maxSauce = 2;

    public void AddPepperoni()
    {
        pepperoniCount++;
        Debug.Log("Pepperoni count: " + pepperoniCount);
    }

    public void AddCheese()
    {
        cheeseCount++;
        Debug.Log("Cheese count: " + cheeseCount);
    }

    public void AddSauce()
    {
        sauceCount++;
        Debug.Log("Sauce count: " + sauceCount);
    }

    public bool IsPizzaAcceptable()
    {
        // Check if there are enough toppings
        if (pepperoniCount < requiredPepperoni)
        {
            Debug.Log("Not enough pepperoni");
            return false;
        }
        if (cheeseCount < requiredCheese)
        {
            Debug.Log("Not enough cheese");
            return false;
        }
        if (sauceCount < requiredSauce)
        {
            Debug.Log("Not enough sauce");
            return false;
        }

        // Check if there are too many toppings
        if (pepperoniCount > maxPepperoni)
        {
            Debug.Log("Too much pepperoni");
            return false;
        }
        if (cheeseCount > maxCheese)
        {
            Debug.Log("Too much cheese");
            return false;
        }
        if (sauceCount > maxSauce)
        {
            Debug.Log("Too much sauce");
            return false;
        }

        return true;
    }

    public string GetRejectionReason()
    {
        if (pepperoniCount < requiredPepperoni) return "Not enough pepperoni!";
        if (cheeseCount < requiredCheese) return "Not enough cheese!";
        if (sauceCount < requiredSauce) return "Not enough sauce!";
        if (pepperoniCount > maxPepperoni) return "Too much pepperoni!";
        if (cheeseCount > maxCheese) return "Too much cheese!";
        if (sauceCount > maxSauce) return "Too much sauce!";
        return "Something is wrong with this pizza!";
    }
}