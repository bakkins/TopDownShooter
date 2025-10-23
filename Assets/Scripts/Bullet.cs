using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public Rigidbody2D rb;

    void Start()
    {
        // Auto-assign the Rigidbody2D if it's not set
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Make the bullet move forward
        rb.linearVelocity = transform.up * speed;

        // Destroy after 2 seconds to clean up
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
