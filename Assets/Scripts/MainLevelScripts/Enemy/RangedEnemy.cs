using UnityEngine;
using System;

public class RangedEnemy : Enemy
{
    public GameObject projectilePrefab;
    public float stopDistance = 6f;
    public float fireRate = 2f;
    private float nextFireTime;

    protected override void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > stopDistance)
        {
            base.Update(); // Move toward player
        }
        else
        {
            // Attack logic
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || player == null) return;

        // Calculate direction from enemy to player
        Vector2 dir = (player.position - transform.position).normalized;

        // Calculate the rotation angle
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Spawn the projectile with the correct rotation
        // The EnemyProjectile script will then use its "transform.right" to move forward
        Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));

        // Play sound if you have it set up
        SoundManager.instance?.PlaySFX(SoundManager.instance.enemyShoot);
    }
}