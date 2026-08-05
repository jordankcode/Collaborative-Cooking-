using UnityEngine;

public class PizzaOven : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject rawPizza;          // The pizza you place in the oven
    public GameObject cookedPizzaPrefab; // Prefab to spawn after cooking
    public float cookTime = 30f;         // Time in seconds

    private bool isCooking = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the raw pizza enters the oven
        if (!isCooking && other.gameObject == rawPizza)
        {
            StartCoroutine(CookPizza());
        }
    }

    private System.Collections.IEnumerator CookPizza()
    {
        isCooking = true;

        // Wait for the cooking duration
        yield return new WaitForSeconds(cookTime);

        // Spawn cooked pizza at the same position/rotation
        Instantiate(cookedPizzaPrefab, rawPizza.transform.position, rawPizza.transform.rotation);

        // Remove the raw pizza
        Destroy(rawPizza);

        isCooking = false;
    }
}
