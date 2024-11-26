using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab; // Assign your bug prefab in the Inspector
    public float spawnInterval = 1f; // Time in seconds between spawns
    private bool isSpawning = false; // To track if InvokeRepeating has been started

    private void Start()
    {
        // Ensure InvokeRepeating is only called once
        if (!isSpawning)
        {
            InvokeRepeating(nameof(SpawnBug), spawnInterval, spawnInterval);
        }
    }

    private void SpawnBug()
    {
        // Define the spawn position
        float randomX = Random.Range(-8f, 8f);
        float randomY = Random.value > 0.5f ? 6f : -6f; // Top (6) or bottom (-6)
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // Instantiate the bug prefab
        Instantiate(bugPrefab, spawnPosition, Quaternion.identity);
    }
}