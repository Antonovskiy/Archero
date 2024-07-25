using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    private Transform player;
    private Vector3 targetDirection;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Vector3 directionToPlayer = (player.position - transform.position).normalized; 
        transform.forward = directionToPlayer;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
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
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
