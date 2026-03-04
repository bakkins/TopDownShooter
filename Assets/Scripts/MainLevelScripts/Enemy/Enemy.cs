using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    [HideInInspector] public int maxHealth;
    public int health = 3;
    public int damage = 10;
    public float knockbackForce = 5f;

    [Header("Drops")]
    public GameObject coinPickupPrefab;
    public int coinsToDrop = 1;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    
    protected Transform player; // Changed to protected so subclasses can see player

    protected virtual void Awake() 
    {
        maxHealth = health; 
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;
        MoveTowardPlayer();
    }

    // Moved movement to its own function so children can call it easily
    protected void MoveTowardPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.enemyDamage);

        OnHealthChanged?.Invoke(health, Mathf.Max(health, 0));

        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        DropCoins();
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    // Logic for colliding with player remains here as all enemies deal contact damage
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (stats != null) stats.TakeDamage(damage);

        if (playerRb != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void DropCoins()
    {
        if (coinPickupPrefab == null) return;
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Instantiate(coinPickupPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }
}