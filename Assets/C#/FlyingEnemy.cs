using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    private Transform player;  
    public float speed = 5f;  
    public float health = 100f; 
    public float detectionRadius = 0.2f; 
    public float descendSpeed = 2f; 
    public float damage = 10f;
    public GameObject coinPrefab;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        
        Vector3 position = transform.position;
        position.y = 2f;
        transform.position = position;
    }

    private void Update()
    {
        if (Manager.Instance.isPaused) return;
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(player.position, transform.position);
            Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

            // Спускатися до гравця, якщо він у радіусі виявлення
            if (distance <= detectionRadius)
            {
                newPosition.y = Mathf.MoveTowards(transform.position.y, player.position.y, descendSpeed * Time.deltaTime);
            }
            else
            {
                newPosition.y = 2f; 
            }

            transform.position = newPosition;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MovePlayer playerScript = collision.gameObject.GetComponent<MovePlayer>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            Destroy(gameObject);
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
            Instantiate(coinPrefab, new Vector3 (transform.position.x, transform.position.y+1f, transform.position.z), Quaternion.identity);
        }
    }
}
