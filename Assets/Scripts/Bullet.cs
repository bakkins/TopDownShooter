using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public Rigidbody2D rb;

    public float lifetime = 2f; // destroy bullet after this time

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.up * speed;

        Destroy(gameObject, lifetime); // auto-delete
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        // Only hit enemies
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        // No obstacle check needed
    }
}
