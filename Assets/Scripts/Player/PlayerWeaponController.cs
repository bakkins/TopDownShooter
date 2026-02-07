using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float bulletSpeed = 10f;

    private float fireTimer = 0f;

    // Reference to PlayerStats for damage multiplier
    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        if (stats == null)
            Debug.LogError("PlayerStats component not found on Player!");
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Apply PlayerStats damage multiplier
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null && stats != null)
        {
            bulletScript.damage = Mathf.RoundToInt(bulletScript.damage * stats.damageMultiplier);
        }

        // Set Rigidbody2D velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f; // top-down shooter
            rb.linearVelocity = firePoint.up * bulletSpeed;
        }
    }

    public void UpgradeFireRate()
    {
        // Reduce fireRate but keep it at a minimum of 0.05 seconds
        fireRate = Mathf.Max(0.05f, fireRate - 0.03f);
    }
}
