using UnityEngine;

// Put this script on the PILE OF SAUSAGES object.
// The pile needs a Collider on it so OnMouseDown works.

public class SausagePileMine : MonoBehaviour
{
    [Header("Drag your sausage slice prefab in here")]
    public GameObject sausageSlicePrefab;

    [Header("Where the new slice appears")]
    public Transform spawnPoint; // an empty object placed just above the pile

    [Header("Optional - stops spam clicking")]
    public float cooldown = 0.5f;
    private float lastSpawnTime = -999f;

    private void OnMouseDown()
    {
        // stop them spawning heaps of slices by clicking fast
        if (Time.time - lastSpawnTime < cooldown) return;

        lastSpawnTime = Time.time;

        if (sausageSlicePrefab != null && spawnPoint != null)
        {
            Instantiate(sausageSlicePrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.Log("Sausage slice prefab or spawn point not set on SausagePile!");
        }
    }
}
