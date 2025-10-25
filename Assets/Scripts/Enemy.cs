using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    private Transform player;
    public event Action OnDeath;

    public int damage = 10;           // How much damage the enemy deals
    public float knockbackForce = 5f; // How strong the knockback is


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        // Deal damage
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);

            // Apply knockback
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Direction from enemy to player
                Vector2 knockbackDir = (collision.gameObject.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // Enemy does NOT die on contact anymore
    }
}
}
