using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    private Transform target; 
    private Vector3 targetDirection;

    private void Start()
    {
        List<GameObject> allEnemies = new List<GameObject>();
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("EnemyFly"));

        if (allEnemies.Count > 0)
        {
            Transform nearestEnemy = allEnemies[0].transform;
            float minDistance = Vector3.Distance(transform.position, nearestEnemy.position);

            foreach (GameObject enemy in allEnemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }

            target = nearestEnemy;
        }
        else
        {
            Destroy(gameObject); // Видаляємо пулю, якщо ворогів немає
        }
    }

    private void Update()
    {
        if (target != null)
        {
            targetDirection = (target.position - transform.position).normalized;
            transform.position += targetDirection * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy"))
    {
        EnemyAI enemyScript = other.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    else if (other.CompareTag("EnemyFly"))
    {
        FlyingEnemy enemyScript = other.GetComponent<FlyingEnemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    else if (other.CompareTag("Obstacle"))
    {
        Destroy(gameObject);
    }
}
}
