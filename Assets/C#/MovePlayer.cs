using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public Slider healthSlider;
    public float _moveSpeed;
    public Joystick joystick;
    public float maxHealth = 100f; 
    public float damageAmount = 10f; 

    public GameObject bulletPrefab; 
    public Transform bulletSpawnPoint; 
    public float shootingInterval = 2f; 

    public GameObject levelTransitionPrefab;
    public int numNextScene;
    
    private float currentHealth;
    private float shootTimer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        shootTimer = shootingInterval;

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void FixedUpdate()
    {
        if (Manager.Instance.isPaused) return;
        _rigidbody.velocity = new Vector3(joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, joystick.Vertical * _moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
        else
        {
            HandleAutomaticShooting();
        }

        CheckForLevelTransition();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        } 

        UpdateHealthUI();
    }


    private void HandleAutomaticShooting()
    {
        shootTimer -= Time.deltaTime; 

        if (FindClosestEnemy() != null && shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootingInterval; 
        }
    }

    private void ShootProjectile()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Destroy(bullet, 3);
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] flyingEnemies = GameObject.FindGameObjectsWithTag("EnemyFly");

        List<GameObject> allEnemies = new List<GameObject>();
        allEnemies.AddRange(enemies);
        allEnemies.AddRange(flyingEnemies);

        if (allEnemies.Count > 0)
        {
            Transform closestEnemy = allEnemies[0].transform;
            float minDistance = Vector3.Distance(transform.position, closestEnemy.position);

            foreach (GameObject enemy in allEnemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            return closestEnemy;
        }

        return null;
    }

    private void CheckForLevelTransition()
    {
        if (FindClosestEnemy() == null)
        {
            if (levelTransitionPrefab != null)
            {
                levelTransitionPrefab.SetActive(true);
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    if (other.CompareTag("ExitLevel"))
    {
        Manager.Instance.loadScene(numNextScene);
    }
    }
}
