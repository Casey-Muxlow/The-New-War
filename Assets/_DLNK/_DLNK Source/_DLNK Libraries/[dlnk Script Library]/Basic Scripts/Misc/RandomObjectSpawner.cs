using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public List<GameObject> objectList; // List of game objects to spawn
    public float minSpawnInterval = 1.0f; // Minimum spawn interval
    public float maxSpawnInterval = 2.0f; // Maximum spawn interval
    public List<MonoBehaviour> optionalScripts; // Optional scripts to attach to the spawned object
    public Vector3 minScale = Vector3.one * 0.5f; // Minimum scale of the spawned object
    public Vector3 maxScale = Vector3.one * 2.0f; // Maximum scale of the spawned object
    public bool keepAspectRatio = true; // Flag to keep the aspect ratio when scaling

    private float timer = 0.0f;
    private float spawnInterval;

    private void Start()
    {
        // Set the initial random spawn interval
        GenerateRandomSpawnInterval();
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a new object
        if (timer >= spawnInterval)
        {
            // Reset the timer
            timer = 0.0f;

            // Spawn a random object from the list
            SpawnRandomObject();

            // Generate a new random spawn interval
            GenerateRandomSpawnInterval();
        }
    }

    private void GenerateRandomSpawnInterval()
    {
        // Calculate a random spawn interval between the minimum and maximum values
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnRandomObject()
    {
        // Check if the object list is empty
        if (objectList.Count == 0)
        {
            Debug.LogWarning("No game objects in the list!");
            return;
        }

        // Get a random index within the range of the list
        int randomIndex = Random.Range(0, objectList.Count);

        // Instantiate the randomly selected object at the spawner's position and rotation
        GameObject newObject = Instantiate(objectList[randomIndex], transform.position, transform.rotation);

        // Randomize the scale of the spawned object
        Vector3 randomScale = Vector3.one;
        if (keepAspectRatio)
        {
            // Randomize the scale uniformly
            float randomUniformScale = Random.Range(minScale.x, maxScale.x);
            randomScale = Vector3.one * randomUniformScale;
        }
        else
        {
            // Randomize each axis independently
            randomScale = new Vector3(
                Random.Range(minScale.x, maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z)
            );
        }

        newObject.transform.localScale = randomScale;
        newObject.transform.rotation = this.transform.rotation;

        // Attach optional scripts to the spawned object
        foreach (MonoBehaviour script in optionalScripts)
        {
            // Clone the values of the optional script from the reference game object
            MonoBehaviour clonedScript = newObject.AddComponent(script.GetType()) as MonoBehaviour;
            if (clonedScript != null)
            {
                // Copy the values from the reference script to the cloned script
                System.Reflection.FieldInfo[] fields = script.GetType().GetFields();
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    field.SetValue(clonedScript, field.GetValue(script));
                }
            }
        }
    }
}
