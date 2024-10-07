using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject itemPrefab1; // The first item to spawn
    public GameObject itemPrefab2; // The second item to spawn with a lower chance
    public float spawnIntervalMin = 2f;
    public float spawnIntervalMax = 5f;
    public float item2SpawnProbability = 0.2f; // Probability (0.0 to 1.0) for item 2 to spawn

    private Transform[] spawnPoints; // Array to hold spawn point transforms
    private float spawnTimer;

    void Start()
    {
        // Get all child spawn points from the parent object
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

        // Start the spawn timer with a random value between min and max interval
        spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void Update()
    {
        // Countdown the spawn timer
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnItem();
            // Reset the spawn timer
            spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
    }

    void SpawnItem()
    {
        // Choose a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Decide which item to spawn based on probability
        GameObject itemToSpawn = Random.value < item2SpawnProbability ? itemPrefab2 : itemPrefab1;

        // Instantiate the chosen item at the spawn point's position
        Instantiate(itemToSpawn, spawnPoint.position, Quaternion.identity);
    }
}
