using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject imageToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            imageToDisable.SetActive(false);
        }
    }
}