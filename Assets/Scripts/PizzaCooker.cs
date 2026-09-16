using UnityEngine;
using System.Collections;

// Put this script on the OVEN / COOKING AREA object.
// Needs a Collider with "Is Trigger" ticked ON, sized for where the raw pizza gets placed.
// This REPLACES the old PizzaCooker.cs, which was an empty template with the wrong class name
// (it was named "NewMonoBehaviourScript" inside, so it could never attach to anything).

public class PizzaCooker : MonoBehaviour
{
    [Header("Drag your cooked pizza prefab in here")]
    [Tooltip("Needs Pickup + PizzaTopping + Rigidbody + Collider, and the tag 'Pizza'")]
    public GameObject cookedPizzaPrefab;

    [Header("Where the finished pizza appears once cooked")]
    public Transform spawnPoint;

    [Header("How long cooking takes, in seconds")]
    public float cookTime = 5f;

    private bool isCooking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCooking) return;

        // only react to a raw pizza — anything with a PizzaTopping component on it
        PizzaTopping rawTopping = other.GetComponentInParent<PizzaTopping>();
        if (rawTopping != null)
        {
            StartCoroutine(CookPizza(other.gameObject, rawTopping));
        }
    }

    private IEnumerator CookPizza(GameObject rawPizza, PizzaTopping rawTopping)
    {
        isCooking = true;

        // remember the topping counts before the raw pizza is destroyed
        int pepperoni = rawTopping.pepperoniCount;
        int cheese = rawTopping.cheeseCount;
        int sauce = rawTopping.sauceCount;

        Debug.Log("Pizza went into the oven. Pepperoni: " + pepperoni + " Cheese: " + cheese + " Sauce: " + sauce);

        Destroy(rawPizza);

        yield return new WaitForSeconds(cookTime);

        if (cookedPizzaPrefab != null && spawnPoint != null)
        {
            GameObject cooked = Instantiate(cookedPizzaPrefab, spawnPoint.position, spawnPoint.rotation);
            PizzaTopping cookedTopping = cooked.GetComponent<PizzaTopping>();

            if (cookedTopping != null)
            {
                // carry the topping counts over onto the finished pizza
                cookedTopping.pepperoniCount = pepperoni;
                cookedTopping.cheeseCount = cheese;
                cookedTopping.sauceCount = sauce;
            }
            else
            {
                Debug.Log("Cooked pizza prefab has no PizzaTopping component — counts won't carry over!");
            }

            Debug.Log("Pizza cooked and ready to serve.");
        }
        else
        {
            Debug.Log("Cooked pizza prefab or spawn point not set on PizzaCooker!");
        }

        isCooking = false;
    }
}
