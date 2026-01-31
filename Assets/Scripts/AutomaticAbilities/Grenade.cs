using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 1f;
    public float radius = 2f;
    public int damage = 10;

    void Start()
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
                e.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
