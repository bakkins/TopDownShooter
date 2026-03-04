using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    public bool isEnemyBullet = false; // Check this in the Inspector for the Boss bullet

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyBullet && other.CompareTag("Player"))
        {
            // Replace 'PlayerStats' with whatever script manages your player's health
            other.GetComponent<PlayerStats>().TakeDamage(1); 
            Destroy(gameObject);
        }
        else if (!isEnemyBullet && other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
