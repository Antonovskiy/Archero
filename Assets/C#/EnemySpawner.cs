using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    public float spawnRadius = 1f; 
    
    public int maxAttempts = 10; 
    public LayerMask obstacleLayer; 
    public float defaultYPosition = 0f; 


    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            Vector3 spawnPosition;
            bool validPositionFound = false;
            int attempts = 0;

            // Шукаємо дійсну позицію для спавну
            while (attempts < maxAttempts && !validPositionFound)
            {
                spawnPosition = GetRandomPosition();
                if (!Physics.CheckSphere(spawnPosition, spawnRadius, obstacleLayer))
                {
                    validPositionFound = true;
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }
                attempts++;
            }

        }
    }

    Vector3 GetRandomPosition()
    {
        float minX = Mathf.Min(spawnPoint1.position.x, spawnPoint2.position.x, spawnPoint3.position.x, spawnPoint4.position.x);
        float maxX = Mathf.Max(spawnPoint1.position.x, spawnPoint2.position.x, spawnPoint3.position.x, spawnPoint4.position.x);
        float minZ = Mathf.Min(spawnPoint1.position.z, spawnPoint2.position.z, spawnPoint3.position.z, spawnPoint4.position.z);
        float maxZ = Mathf.Max(spawnPoint1.position.z, spawnPoint2.position.z, spawnPoint3.position.z, spawnPoint4.position.z);

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        float yPos = defaultYPosition; // Встановлюємо висоту за замовчуванням

        return new Vector3(randomX, yPos, randomZ);
    }
}
