using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public int health = 3;
    public int damage = 10;
    public float knockbackForce = 5f;

    [Header("Drops")]
    public GameObject coinPickupPrefab;  // assign coin pickup prefab here
    public int coinsToDrop = 1;

    private Transform player;
    public event Action OnDeath;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("Player not found! Make sure the Player has tag 'Player'");
    }

    void Update()
    {
        if (player == null) return;

        // Move toward player
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    // -----------------------
    // DAMAGE HANDLING
    // -----------------------
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            DropCoins();
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    // -----------------------
    // KNOCKBACK / COLLISION
    // -----------------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // Apply damage
        if (stats != null)
            stats.TakeDamage(damage);

        // Apply knockback
        if (playerRb != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }
    }

    // -----------------------
    // COIN DROP
    // -----------------------
    void DropCoins()
    {
        if (coinPickupPrefab == null) return;

        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f; // randomize spawn
            Instantiate(coinPickupPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }
}
