using UnityEngine;

// Put this script on the BLOCK OF CHEESE object.
// The block needs a Collider on it so OnMouseDown works.

public class CheeseBlock : MonoBehaviour
{
    [Header("Drag your little cheese prefab in here")]
    public GameObject littleCheesePrefab;

    [Header("Where the new cheese piece appears")]
    public Transform spawnPoint; // an empty object placed just above the block

    [Header("Optional - stops spam clicking")]
    public float cooldown = 0.5f;
    private float lastSpawnTime = -999f;

    private void OnMouseDown()
    {
        // stop them spawning heaps of cheese by clicking fast
        if (Time.time - lastSpawnTime < cooldown) return;

        lastSpawnTime = Time.time;

        if (littleCheesePrefab != null && spawnPoint != null)
        {
            Instantiate(littleCheesePrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.Log("Little cheese prefab or spawn point not set on CheeseBlock!");
        }
    }
}
