using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   public List<GameObject> itemPrefabs; // List to hold multiple item prefabs
    public List<float> itemSpawnProbabilities; // Corresponding spawn probabilities for each item
    public float spawnIntervalMin = 2f;
    public float spawnIntervalMax = 5f;

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

        // Choose an item to spawn based on probabilities
        GameObject itemToSpawn = GetRandomItem();

        if (itemToSpawn != null)
        {
            // Instantiate the chosen item at the spawn point's position
            Instantiate(itemToSpawn, spawnPoint.position, Quaternion.identity);
        }
    }

    GameObject GetRandomItem()
    {
        // Make sure the lists are valid
        if (itemPrefabs.Count == 0 || itemPrefabs.Count != itemSpawnProbabilities.Count)
        {
            Debug.LogError("Item prefabs and probabilities lists must have the same number of entries.");
            return null;
        }

        // Generate a random value between 0 and the sum of probabilities
        float totalProbability = 0f;
        foreach (float probability in itemSpawnProbabilities)
        {
            totalProbability += probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        // Find the item corresponding to the random value
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            cumulativeProbability += itemSpawnProbabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return itemPrefabs[i];
            }
        }

        return null;
    }
}
