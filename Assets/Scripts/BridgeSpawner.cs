using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public GameObject bridgePrefab; // Assign the bridge prefab in the Inspector
    public GameObject currentBridge; // Stores the active bridge
    public Transform spawnPoint; // Where the bridge spawns (should be near the player)
    public float growthSpeed = 2f;
    public float forceAmount = 500f; // Adjust the force strength
    public CollectableItem collectableItem;
    public Obstacle obstaclePrefab;

    [SerializeField] private Transform player;
    private bool isGrowing = false;
    private bool isSpawning = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click to spawn a new bridge
        {
            SpawnBridge();
            isGrowing = true;
        }

        if (Input.GetMouseButtonUp(0)) // Release to stop growing & apply force at top
        {
            isGrowing = false;
            isSpawning = false;
            StartCoroutine(ApplyForceAtTop());
        }

        if (isGrowing && currentBridge != null)
        {
            currentBridge.transform.localScale += new Vector3(0, growthSpeed * Time.deltaTime, 0);
        }
    }

    void SpawnBridge()
    {
        if (isSpawning) return;
        isSpawning = true;
        currentBridge = Instantiate(bridgePrefab, spawnPoint.position, Quaternion.identity);
        currentBridge.transform.rotation = player.transform.rotation;
    }

    private IEnumerator ApplyForceAtTop()
    {
        if (currentBridge == null) yield break;

        // Enable Rigidbody Physics
        Rigidbody rb = currentBridge.GetComponent<Rigidbody>();
        rb.isKinematic = false;  // Allow physics to take control
        rb.useGravity = true;     // Enable gravity

        // Calculate the top position of the bridge
        Vector3 topPosition = currentBridge.transform.position + (currentBridge.transform.up * (currentBridge.transform.localScale.y * 2));

        // Apply force at the top
        rb.AddForceAtPosition(Vector3.forward * forceAmount, topPosition, ForceMode.Impulse);

        yield return new WaitForSeconds(3f);

        SpawnItems(currentBridge);
    }
    public void DespawnBridge()
    {
        Destroy(currentBridge);
        currentBridge = null;
    }

    void SpawnItems(GameObject bridge)
    {
        List<Vector3> spawnedPositions = new List<Vector3>();
        float minDistance = 1.0f; // Minimum distance to avoid overlap
        float bridgeLength = bridge.transform.localScale.y; // Get bridge length

        int obstacleCount = Random.Range(1, 2); // Randomize obstacle count (1 to 2)
        int collectibleCount = Random.Range(2, 4); // Randomize collectible count (2 to 4)

        // Then, spawn collectibles
        for (int i = 0; i < collectibleCount; i++)
        {
            Vector3 spawnPosition;
            bool positionValid;
            int attempt = 0;
            int maxAttempts = 10;

            do
            {
                positionValid = true;

                // Calculate position on top of the bridge
                Vector3 bridgeTop = bridge.transform.position - bridge.transform.forward.normalized / 3;

                // Random offset along the bridge
                spawnPosition = bridgeTop + bridge.transform.up.normalized * Random.Range(0, bridgeLength);

                // Ensure spacing from other objects
                foreach (var pos in spawnedPositions)
                {
                    if (Vector3.Distance(spawnPosition, pos) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }

                attempt++;
            } while (!positionValid && attempt < maxAttempts);

            // Spawn collectible if a valid position is found
            if (positionValid)
            {
                Instantiate(collectableItem, spawnPosition, Quaternion.Euler(-90, 0, 0));
                spawnedPositions.Add(spawnPosition);
            }
        }

        // First, spawn obstacles
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 spawnPosition;
            bool positionValid;
            int attempt = 0;
            int maxAttempts = 10;

            do
            {
                positionValid = true;

                // Calculate position on top of the bridge
                Vector3 bridgeTop = bridge.transform.position - bridge.transform.forward.normalized / 3;

                // Random offset along the bridge
                spawnPosition = bridgeTop + bridge.transform.up.normalized * Random.Range(0, bridgeLength);

                // Ensure no overlap
                foreach (var pos in spawnedPositions)
                {
                    if (Vector3.Distance(spawnPosition, pos) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }

                attempt++;
            } while (!positionValid && attempt < maxAttempts);

            // Spawn obstacle if a valid position is found
            if (positionValid)
            {
                Obstacle obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity); // Replace with obstacle prefab
                spawnedPositions.Add(spawnPosition);
            }
        }


    }


}
