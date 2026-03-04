using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 7f;
    public int damage = 10;
    public float lifeTime = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 1. Give the bullet velocity immediately upon spawning
        // We use transform.right because the RangedEnemy script rotates the bullet toward the player
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }

        // 2. Self-destruct after a few seconds to clean up memory
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 3. Only hit the Player
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }

            // Destroy bullet on impact with player
            Destroy(gameObject);
        }

        // 4. Destroy bullet if it hits a wall/environment
        if (collision.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }
}