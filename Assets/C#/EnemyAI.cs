using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; 
    public float stoppingDistance = 5f;
    public GameObject projectilePrefab; 
    public Transform projectileSpawnPoint; 
    public float shootingInterval;
    public float health;
    public GameObject coinPrefab;

    private float shootTimer;
    private NavMeshAgent navMeshAgent;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        shootTimer = shootingInterval; 
    }

    void Update()
    {
        if (Manager.Instance.isPaused) return;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > stoppingDistance)
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                navMeshAgent.SetDestination(transform.position); 
                HandleShooting();
            }

            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
    }

    void HandleShooting()
    {
        shootTimer -= Time.deltaTime; 

        if (shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootingInterval; 
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
           GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Destroy(bullet, 3);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
           Manager.Instance.AddCoin(3);
           SpawnCoins();
            Destroy(gameObject); 
        }
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(coinPrefab, new Vector3 (transform.position.x, transform.position.y+3f, transform.position.z), Quaternion.identity);
        }
    }
}
