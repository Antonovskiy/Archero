using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float speed = 5f;
    
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MoveToPlayerAfterDelay(3f));
    }

    IEnumerator MoveToPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject, 3.5f);
        while (true)
        {
            MoveToPlayer();
            yield return null;
        }
    }

    void MoveToPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
