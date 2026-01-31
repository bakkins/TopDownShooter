using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon")]
    public Weapon currentWeapon;      // ScriptableObject
    public Transform firePoint;

    private float fireTimer;

    [Header("References")]
    public Camera cam;                // Camera to get mouse position
    public PlayerStats playerStats;   // For damage multiplier

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && fireTimer >= currentWeapon.fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        int count = currentWeapon.bulletCount;
        float angleStep = (count > 1) ? currentWeapon.spreadAngle / (count - 1) : 0;

        for (int i = 0; i < count; i++)
        {
            float angle = -currentWeapon.spreadAngle / 2 + angleStep * i;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, rotation);

            // Set bullet velocity
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = bullet.transform.up * currentWeapon.bulletSpeed;

            // Apply damage multiplier from PlayerStats
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = Mathf.RoundToInt(currentWeapon.damage * playerStats.damageMultiplier);
                bulletScript.piercing = currentWeapon.piercing;
            }
        }
    }

    public void UpgradeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
    }
}
