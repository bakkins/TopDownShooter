using UnityEngine;
using System;

public enum EnemyType { Melee, Ranged, Tank, Fast, Boss }

public class Enemy : MonoBehaviour
{
    public EnemyType type = EnemyType.Melee;

    public float speed = 2f;
    public int health = 3;
    public int damage = 10;
    public float knockbackForce = 5f;

    [Header("Ranged Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireTimer;

    private Transform player;
    public event Action OnDeath;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        fireTimer += Time.deltaTime;

        switch (type)
        {
            case EnemyType.Melee:
            case EnemyType.Tank:
            case EnemyType.Fast:
                MoveTowardsPlayer();
                break;
            case EnemyType.Ranged:
                ShootAtPlayer();
                break;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    void ShootAtPlayer()
    {
        if (fireTimer >= fireRate)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir * 5f; // bullet speed
            fireTimer = 0f;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
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
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerStats != null)
                playerStats.TakeDamage(damage);

            if (playerRb != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
