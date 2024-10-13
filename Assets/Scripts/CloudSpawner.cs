using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs; // Array of cloud prefabs to spawn
    public float spawnInterval = 5f;  // Time between cloud spawns
    public float spawnRangeY = 2f;    // Y-range within which clouds can spawn
    public float spawnPositionX = 10f; // X position to spawn clouds (right side)
    public float despawnPositionX = -10f; // X position to despawn clouds (left side)
    public float cloudSpeed = 1f;      // Speed at which clouds move

    private float timer;

    void Update()
    {
        // Handle cloud spawning based on a timer
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCloud();
            timer = 0f;
        }

        // Move and despawn clouds
        MoveAndDespawnClouds();
    }

    // Spawn a cloud at a random Y position
    void SpawnCloud()
    {
        // Pick a random cloud prefab from the array
        GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

        // Set a random Y position for the cloud
        float randomY = Random.Range(-spawnRangeY, spawnRangeY);

        // Instantiate the cloud at the spawn position
        Vector3 spawnPosition = new Vector3(spawnPositionX, randomY, 0f);
        Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
    }

    // Move all clouds and despawn them if they go out of bounds
    void MoveAndDespawnClouds()
    {
        // Find all cloud objects in the scene
        GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");

        foreach (GameObject cloud in clouds)
        {
            // Move the cloud to the left
            cloud.transform.Translate(Vector3.left * cloudSpeed * Time.deltaTime);

            // If the cloud goes beyond the despawn position, destroy it
            if (cloud.transform.position.x < despawnPositionX)
            {
                Destroy(cloud);
            }
        }
    }
}
