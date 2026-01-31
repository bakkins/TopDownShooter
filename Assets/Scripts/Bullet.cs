using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public bool piercing = false;
    public Rigidbody2D rb;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (!piercing)
                Destroy(gameObject);
        }
        else if (!hit.CompareTag("Player") && !piercing)
        {
            Destroy(gameObject);
        }
    }
}
